using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(0)]
public class PlayerManager : MonoBehaviour
{
    #region Vars

    public static PlayerManager current;

    
    #region ----------------------PLAYER STATS----------------------------

    [FoldoutGroup("Player Stats"), Title("Health", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("Player Stats")]public int I_MaxHealth;
    [FoldoutGroup("Player Stats")][SerializeField]private int _actHealth;
    [FoldoutGroup("Player Stats")]public int I_ActHealth { get => _actHealth; set => HealthCheck(value);}
    [FoldoutGroup("Player Stats"),Title("Damage", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("Player Stats")] public int I_BaseDamage;
    [FoldoutGroup("Player Stats")]public float F_DamageScale;
    [FoldoutGroup("Player Stats"), ReadOnly]public int I_ActDamage;
    [FoldoutGroup("Player Stats"),Title("Heal", titleAlignment: TitleAlignments.Centered)] 
    [FoldoutGroup("Player Stats")]public int I_Healling;
    [FoldoutGroup("Player Stats")]public int I_ActHeals;
    [FoldoutGroup("Player Stats")]public int I_MaxHeals;
    [FoldoutGroup("Player Stats"), ShowInInspector, ReadOnly][SerializeField]private bool B_CanSufferDmg = true;
    [FoldoutGroup("Player Stats"),ShowInInspector, ReadOnly]private bool B_CanHeal = true;
    [FoldoutGroup("Player Stats"),Title("Parry", titleAlignment: TitleAlignments.Centered)] 
    [FoldoutGroup("Player Stats")]public float F_ParryTime;
    [FoldoutGroup("Player Stats")]public float F_ParryDuration;
    [FoldoutGroup("Player Stats")]public float F_ParryCD;
    [FoldoutGroup("Player Stats"),ShowInInspector, ReadOnly]private bool B_CanParry = true;
    [FoldoutGroup("Player Stats")] [SerializeField] private GameObject G_ParryArea;
    [FoldoutGroup("Player Stats"), Title("Self Damage", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("Player Stats")] public int I_SelfDamage;
    [FoldoutGroup("Player Stats"), ShowInInspector, ReadOnly] private bool B_CanSelfDamage = true;
    [FoldoutGroup("Player Stats"),Title("Player Level", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("Player Stats"), ShowInInspector,ReadOnly, PropertySpace(SpaceAfter = 15)]private int I_ActLv;
    #endregion
    
    #region ----------------------Shooting----------------------------
    [FoldoutGroup("Shooting"), Title("Bullet", titleAlignment: TitleAlignments.Centered)] public GameObject G_BulletPrefab;
    [FoldoutGroup("Shooting")] public int I_BulletPoolSize;
    [FoldoutGroup("Shooting")] public Queue<GameObject> Q_BulletPool = new Queue<GameObject>();
    [FoldoutGroup("Shooting"),Title("Shooting", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("Shooting")] public Transform T_ShootingPoint;
    [FoldoutGroup("Shooting")] public float F_ShootDelay;
    [FoldoutGroup("Shooting")][SerializeField, ReadOnly]public bool B_CanShoot = true;
    [FoldoutGroup("Shooting")][SerializeField, ReadOnly] private bool B_IsShooting = false;
    [FoldoutGroup("Shooting"), PropertySpace(SpaceAfter = 15)][SerializeField] private float F_ShootAngle;
    #endregion
    
    #region ----------------------GUI----------------------------
    [FoldoutGroup("GUI")][SerializeField] private Slider Sl_LifeBar;
    [FoldoutGroup("GUI")][SerializeField] private RectTransform Rt_LifeBar;
    [FoldoutGroup("GUI")][SerializeField] private TMP_Text Tx_Heals;
    #endregion
    
    #region -------------------Other References-----------------
    [FoldoutGroup("Other References")] [SerializeField] private List<Material> M_ShipMaterials;
    #endregion
    
    #endregion

    #region Unity Methods
    
    void Awake() //Setear singleton
    {
        if (!current)
            current = this;
        else if (current != this)
            Destroy(this);
    }

    private void Start() //Setear input para actualizar direccion de los disparos y resetar vida
    {
        InputController.current.InputManager.Player.Shooting.performed += ShootAngle;
        InputController.current.InputManager.Player.Healing.performed += Heal;
        InputController.current.InputManager.Player.Parry.performed += Parry;
        InputController.current.InputManager.Player.SelfDamage.performed += SelfDmg;
        I_ActHealth = I_MaxHealth;
        SetDamage();
        SetPool();

    }

    private void Update() //Comprobar si se puede disparar y hacerlo cuando corresponda
    {
        if (B_CanShoot && B_IsShooting)
        {
            B_CanShoot = false;
            GameObject Gtemp = GetBullet();
            Gtemp.transform.position = T_ShootingPoint.position;
            Gtemp.transform.rotation = Quaternion.Euler(0,-F_ShootAngle,0);
            Gtemp.SetActive(true);
            StartCoroutine(ShootDelay());
        }
    }
    
    #endregion

    #region BasicsMethods

    void HealthCheck(int i) //en cada cambio del valor de vida, comprueba que no supere la vida maxima y si la vida baja a/o 0, la setea en cero y llama al metodo de muerte
    {
        _actHealth = i;
        if (_actHealth > I_MaxHealth)
            _actHealth = I_MaxHealth;
        else if (_actHealth <= 0)
        {
            _actHealth = 0;
            Death();
        }
        
    }
    
    private void SetPool() //Iniciacion del object pool de balas
    {
        for (int i = 0; i < I_BulletPoolSize; i++)
        {
            StoreBullet(Instantiate(G_BulletPrefab));
        }
    }
    
    public void SetDamage() //Configura dano del player considerando un porcentaje de vida faltante
    {
        float temp = I_BaseDamage + ((I_MaxHealth-I_ActHealth)* F_DamageScale);
        I_ActDamage = (int)temp;
        
    }
    
    
    IEnumerator Invencibility(float t) //Corutina que controla los periodos de invencibilidad, ya sea por parry como por dano
    {
        B_CanSufferDmg = false;
        foreach (Material m in M_ShipMaterials)
        {
            m.color = new Color(m.color.r, m.color.g, m.color.b, 0.7f);
        }
        yield return new WaitForSeconds(t);
        foreach (Material m in M_ShipMaterials)
        {
            m.color = new Color(m.color.r, m.color.g, m.color.b, 1);
        }
        B_CanSufferDmg = true;
    }
    
    #endregion
    
    #region Shooting
    public void ShootAngle(InputAction.CallbackContext call) //Define angulo de disparo y cambia si se puede disparar o no
    {
        Vector2 temp = call.ReadValue<Vector2>();
        if (temp.magnitude > 0.1f)
        {
            B_IsShooting = true;
            F_ShootAngle = Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg;
        }
        else
            B_IsShooting = false;
        
    }

    GameObject GetBullet() //Solicita un bala a la pool, de no haber disponibles crea una nueva
    {
        if (Q_BulletPool.Count != 0)
            return Q_BulletPool.Dequeue();
        else
        {
            GameObject temp = Instantiate(G_BulletPrefab);
            temp.SetActive(false);
            return temp;
        }

    }

    public void StoreBullet(GameObject g) //Regresa balas a la pool
    {
        g.SetActive(false);
        Q_BulletPool.Enqueue(g);
    }

    IEnumerator ShootDelay() //Delay entre cada disparo
    {
        yield return new WaitForSeconds(F_ShootDelay);
        B_CanShoot = true;
    }
    
    #endregion

    #region Damage

    private void OnTriggerEnter(Collider other) //Comprueba la colision con una bala
    {
        if (other.CompareTag("Enemy/Bullet") && B_CanSufferDmg)
        {
            B_CanSufferDmg = false;
            ApplyDamage(other.GetComponent<EnemyBullet>().I_Damage);
            BulletPool.current.StoreBullet(other.gameObject);
        }
    }

    void ApplyDamage(int i) //Aplica el dano segun el stat dado por la bala enemiga
    {

            I_ActHealth -= i;
            StartCoroutine(Invencibility(0.3f));
            UpdateHud();
    }

    void Death() //Controla los comportamientos de muerte
    {
        
    }

    void SelfDmg(InputAction.CallbackContext call) //Input de self damage
    {
        if (B_CanSelfDamage && I_ActHealth > I_SelfDamage)
        {
            B_CanSelfDamage = false;
            I_ActHealth -= I_SelfDamage;
            SetDamage();
            UpdateHud();
            StartCoroutine(SelfDmgCD());
        }
    }

    IEnumerator SelfDmgCD()// Corutina delay para SelfDamage
    {
        yield return new WaitForSeconds(0.2f);
        B_CanSelfDamage = true;
    }

    #endregion
    
    #region HUD

    public void UpdateHud() //Actualizar la informacion del HUD
    {
        Sl_LifeBar.maxValue = I_MaxHealth;
        Sl_LifeBar.value = I_ActHealth;
        Rt_LifeBar.sizeDelta = new Vector2(HealthBarMap(I_MaxHealth), Rt_LifeBar.sizeDelta.y);
        Tx_Heals.text = I_ActHeals.ToString();
        

    }

    int HealthBarMap(int value)//Actualizar la barra de vida
    {
        return (value - 100) * (1250 - 300) / (1382 - 100) + 300;
    }

    

    #endregion

    #region Healling

    public void Heal(InputAction.CallbackContext call) //Input de curarse
    {
        if (B_CanHeal && I_ActHeals != 0)
        {
            B_CanHeal = false;
            StartCoroutine(HealCD());
            I_ActHeals--;
            I_ActHealth += I_Healling;
            UpdateHud();
        }
        
        
    }

    IEnumerator HealCD()//Delay para curarse
    {
        yield return new WaitForSeconds(0.5f);
        B_CanHeal = true;
    }

    #endregion
    
    #region Parry

    public void Parry(InputAction.CallbackContext call) //Input de Parry
    {
        if (B_CanParry)
        {
            B_CanParry = false;
            StartCoroutine(ParryCD());
            StartCoroutine(ActiveParry());

        }
    }

    public void SuccesParry()//Control de lograr el parry
    {
        StartCoroutine(Invencibility(F_ParryDuration));
    }

    IEnumerator ParryCD()//Delay para hacer parry
    {
        yield return new WaitForSeconds(F_ParryCD);
        B_CanParry = true;
    }

    IEnumerator ActiveParry() //Activar objeto de trigger de parry
    {
        G_ParryArea.SetActive(true);
        yield return new WaitForSeconds(F_ParryTime);
        G_ParryArea.SetActive(false);
    }
    
    #endregion

}
