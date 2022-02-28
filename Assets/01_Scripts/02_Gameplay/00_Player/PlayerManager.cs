using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[DefaultExecutionOrder(-1)]
public class PlayerManager : MonoBehaviour
{
    #region Vars

    public static PlayerManager current;

    
    #region ----------------------PLAYER STATS----------------------------

    [FoldoutGroup("Player Stats"), Title("Health", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("Player Stats")]public int I_MaxHealth;
    [FoldoutGroup("Player Stats")][SerializeField]private int _actHealth;
    [FoldoutGroup("Player Stats")]public int I_ActHealth { get => _actHealth; set => HealthCheck(value);}
    [FoldoutGroup("Player Stats")] public bool B_isDeath;
    [FoldoutGroup("Player Stats"),Title("Damage", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("Player Stats")] public int I_BaseDamage;
    [FoldoutGroup("Player Stats")]public float F_DamageScale;
    [FoldoutGroup("Player Stats"), ReadOnly] public int I_ActDamage => SetDamage();

    [FoldoutGroup("Player Stats"),Title("Heal", titleAlignment: TitleAlignments.Centered)] 
    [FoldoutGroup("Player Stats")]public int I_Healling;
    [FoldoutGroup("Player Stats")]public int I_ActHeals;
    [FoldoutGroup("Player Stats")]public int I_MaxHeals;
    [FoldoutGroup("Player Stats"), ShowInInspector, ReadOnly][SerializeField]private bool B_CanSufferDmg = true;
    [FoldoutGroup("Player Stats"),ShowInInspector, ReadOnly]private bool B_CanHeal = true;
    [FoldoutGroup("Player Stats"),Title("Parry", titleAlignment: TitleAlignments.Centered)] 
    [FoldoutGroup("Player Stats")]public float F_ParryTime;
    [FoldoutGroup("Player Stats")]public float F_ImprovSpeed;
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
    [FoldoutGroup("Shooting")] private Queue<GameObject> Q_BulletPool = new Queue<GameObject>();
    [FoldoutGroup("Shooting"),Title("Shooting", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("Shooting")] public Transform T_ShootingPoint;
    [FoldoutGroup("Shooting")] public float F_ShootDelay;
    [FoldoutGroup("Shooting")][SerializeField, ReadOnly]public bool B_CanShoot = true;
    [FoldoutGroup("Shooting")][SerializeField, ReadOnly] private bool B_IsShooting = false;
    [FoldoutGroup("Shooting"), PropertySpace(SpaceAfter = 15)][SerializeField] private float F_ShootAngle;
    #endregion
    
    #region ----------------------Weapon----------------------------

    [FoldoutGroup("Weapon")] [SerializeField]private GameObject G_WeaponsParent;
    [FoldoutGroup("Weapon")] private GameObject G_ActWeaponModel;
    [FoldoutGroup("Weapon")] public Weapons[] Wp_ObtainedWeapons = new Weapons[12];
    [FoldoutGroup("Weapon")] public Weapon[] Wp_ObtainedWeaponsSO = new Weapon[12];    
    [FoldoutGroup("Weapon")] [SerializeField] private Weapon Wp_WeaponSO;
    [FoldoutGroup("Weapon")] public Weapons Wp_ActWeapon;
    
    #endregion
    #region ----------------------GUI----------------------------
    [FoldoutGroup("GUI")][SerializeField] private Image Im_LifeBar;
    [FoldoutGroup("GUI")][SerializeField] private Image Im_DelayLifeBar;
    [FoldoutGroup("GUI")][SerializeField] private Image Im_HealLifeBar;
    [FoldoutGroup("GUI")][SerializeField] private TMP_Text Tx_Heals;
    [FoldoutGroup("GUI")] [SerializeField] private float F_LerpTimeHealthBar;
    [FoldoutGroup("GUI")] [SerializeField] private float F_AmountDelayHealthBar;
    #endregion
    
    #region -------------------Other References-----------------
    [FoldoutGroup("Other References")] [SerializeField] private List<Renderer> MR_PlayerMeshRenderer;
    [FoldoutGroup("Other References")] public Transform T_PlayerModel;
    [FoldoutGroup("Other References")] public CharacterController CC_Player;
    
    #endregion
    
    #region ---------------------Events---------------------

    [FoldoutGroup("Events")] public UnityEvent Ev_OnPlayerDeath;
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
        SetPool();
        UpdateHud();
    }

    private void OnDisable()
    {
        InputController.current.InputManager.Player.Shooting.performed -= ShootAngle;
        InputController.current.InputManager.Player.Healing.performed -= Heal;
        InputController.current.InputManager.Player.Parry.performed -= Parry;
        InputController.current.InputManager.Player.SelfDamage.performed -= SelfDmg;
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

    public void ChangeWeapon(int id)
    {
        Wp_ActWeapon = Wp_ObtainedWeapons[id];
        Wp_WeaponSO = Wp_ObtainedWeaponsSO[id];
        if(G_ActWeaponModel!=null)
            Destroy(G_ActWeaponModel);
        G_ActWeaponModel = Instantiate(Wp_WeaponSO.G_WeaponModel, G_WeaponsParent.transform);
    }
    
    void HealthCheck(int i) //en cada cambio del valor de vida, comprueba que no supere la vida maxima y si la vida baja a/o 0, la setea en cero y llama al metodo de muerte
    {
        _actHealth = i;
        Debug.Log(_actHealth);
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
    
    public int SetDamage() //Configura dano del player considerando un porcentaje de vida faltante
    {
        float temp = I_BaseDamage + ((I_MaxHealth-I_ActHealth)* F_DamageScale);
        if (Wp_ActWeapon != Weapons.none)
            temp += Wp_WeaponSO.I_Damage;
        return (int)temp;
        
    }

    void ChangeMaterialsTransparency(float f) //Modifica la transparencia de la nave para el fin que sea necesario
    {
        foreach (Renderer ren in MR_PlayerMeshRenderer)
        {
            foreach (Material m in ren.materials)
            {
                m.color = new Color(m.color.r, m.color.g, m.color.b, f);
            }
        }
    }
    
    IEnumerator Invencibility(float t) //Corutina que controla los periodos de invencibilidad, ya sea por parry como por dano
    {
        if (!B_isDeath)
        {
            B_CanSufferDmg = false;
            ChangeMaterialsTransparency(0.3f);
            yield return ScriptsTools.GetWait(t);
            ChangeMaterialsTransparency(1f);
            B_CanSufferDmg = true;
        }
    }

    public void ResetHealth()
    {
        I_ActHealth = I_MaxHealth;
        I_ActHeals = I_MaxHeals;
        UpdateHud();
        Im_DelayLifeBar.fillAmount = Im_LifeBar.fillAmount;
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
        yield return ScriptsTools.GetWait(F_ShootDelay);
        B_CanShoot = true;
    }
    
    #endregion

    #region Damage

    private void OnTriggerEnter(Collider other) //Comprueba la colision con una bala
    {
        if (other.CompareTag("Enemy/Bullet") && B_CanSufferDmg && !B_isDeath)
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
        StopCoroutine(DelayHealthBar());
        StartCoroutine(DelayHealthBar());

    }

    void SelfDmg(InputAction.CallbackContext call) //Input de self damage
    {
        if (B_CanSelfDamage && I_ActHealth > I_SelfDamage)
        {
            B_CanSelfDamage = false;
            I_ActHealth -= I_SelfDamage;
            UpdateHud();
            StartCoroutine(SelfDmgCD());
        }
    }

    IEnumerator SelfDmgCD()// Corutina delay para SelfDamage
    {
        yield return ScriptsTools.GetWait(0.2f);
        B_CanSelfDamage = true;
    }

    #endregion
    
    #region HUD

    public void UpdateHud() //Actualizar la informacion del HUD
    {

        Im_LifeBar.fillAmount = HealthBarFillMap(I_ActHealth);
        Im_LifeBar.rectTransform.sizeDelta =new Vector2(HealthBarSizeMap(I_MaxHealth), Im_LifeBar.rectTransform.sizeDelta.y);
        Tx_Heals.text = I_ActHeals.ToString();
        

    }

    float HealthBarFillMap(int health)
    {
        return ScriptsTools.MapValues(health, 0, I_MaxHealth, 0, 1);
    }

    int HealthBarSizeMap(int value)//Actualizar tamano de la barra de vida
    {
        return (int)ScriptsTools.MapValues(value, 100, 1382, 300, 1250);
    }

    IEnumerator DelayHealthBar()
    {
        yield return ScriptsTools.GetWait(1);
        while (Im_LifeBar.fillAmount < Im_DelayLifeBar.fillAmount)
        {
            Im_DelayLifeBar.fillAmount -= F_AmountDelayHealthBar;
            yield return ScriptsTools.GetWait(F_LerpTimeHealthBar);
        }

        if (Im_DelayLifeBar.fillAmount < Im_LifeBar.fillAmount)
            Im_DelayLifeBar.fillAmount = Im_LifeBar.fillAmount;
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
        yield return ScriptsTools.GetWait(0.5f);
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
        //TODO:Parry Sound
        StartCoroutine(Invencibility(F_ParryDuration));
        StartCoroutine(ParryVel(F_ParryDuration));
    }

    IEnumerator ParryVel(float t)
    {
        InputController.current.F_Velocity += F_ImprovSpeed;
        yield return ScriptsTools.GetWait(t);
        InputController.current.F_Velocity -= F_ImprovSpeed;
    }

    IEnumerator ParryCD()//Delay para hacer parry
    {
        yield return ScriptsTools.GetWait(F_ParryCD);
        B_CanParry = true;
    }

    IEnumerator ActiveParry() //Activar objeto de trigger de parry
    {
        G_ParryArea.SetActive(true);
        yield return ScriptsTools.GetWait(F_ParryTime);
        G_ParryArea.SetActive(false);
    }
    
    #endregion

    #region DeathSystem
    
    void Death() //Controla los comportamientos de muerte
    {
        //TODO:DestoyedParticles
        ChangeMaterialsTransparency(0f);
        Ev_OnPlayerDeath.Invoke();
        B_isDeath = true;
        B_CanSufferDmg = false;
        B_CanShoot = false;
        B_CanHeal = false;
        B_CanSelfDamage = false;
        B_CanParry = false;
        InputController.current.B_CanMove = false;
        StartCoroutine(PlayerRespawnManager.current.OnPlayerDeath());
    }
    
    public void ResetPlayer(Transform safePoint) //Resetea al Player despues de morir
    {
        CC_Player.enabled = false;
        gameObject.transform.position = safePoint.position;
        CC_Player.enabled = true;
        ChangeMaterialsTransparency(1f);
        B_CanSufferDmg = true;
        B_CanShoot = true;
        B_CanHeal = true;
        B_CanSelfDamage = true;
        B_CanParry = true;
        InputController.current.B_CanMove = true;
        ResetHealth();
        B_isDeath = false;
    }
    
    #endregion
}
