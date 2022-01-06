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

    public static PlayerManager current;

    
    //----------------------PLAYER STATS----------------------------//
    [FoldoutGroup("Player Stats"),Title("Health", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("Player Stats")]public int I_MaxHealth;
    [FoldoutGroup("Player Stats")]public int I_ActHealth;
    [FoldoutGroup("Player Stats"),Title("Damage", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("Player Stats")] public int I_BaseDamage;
    [FoldoutGroup("Player Stats")]public float F_DamageScale;
    [FoldoutGroup("Player Stats"), ReadOnly]public int I_ActDamage;
    [FoldoutGroup("Player Stats"),Title("Heal", titleAlignment: TitleAlignments.Centered)] 
    [FoldoutGroup("Player Stats")]public int I_Healling;
    [FoldoutGroup("Player Stats")]public int I_ActHeals;
    [FoldoutGroup("Player Stats")]public int I_MaxHeals;
    [FoldoutGroup("Player Stats"),ShowInInspector, ReadOnly]private bool B_CanHeal = true;
    [FoldoutGroup("Player Stats"),Title("Parry", titleAlignment: TitleAlignments.Centered)] 
    [FoldoutGroup("Player Stats")]public float F_ParryTime;
    [FoldoutGroup("Player Stats")]public float F_ParryDuration;
    [FoldoutGroup("Player Stats")]public float F_ParryCD;
    [FoldoutGroup("Player Stats"),ShowInInspector, ReadOnly]private bool B_CanParry = true;
    [FoldoutGroup("Player Stats"), Title("Self Damage", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("Player Stats")] public int I_SelfDamage;
    [FoldoutGroup("Player Stats"), ShowInInspector, ReadOnly] private bool B_CanSelfDamage = true;
    [FoldoutGroup("Player Stats"),Title("Player Level", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("Player Stats"), ShowInInspector,ReadOnly, PropertySpace(SpaceAfter = 15)]private int I_ActLv;
    
    //----------------------Shooting----------------------------//
    [FoldoutGroup("Shooting"), Title("Bullet", titleAlignment: TitleAlignments.Centered)] public GameObject G_BulletPrefab;
    [FoldoutGroup("Shooting")] public int I_BulletPoolSize;
    [FoldoutGroup("Shooting")] public Queue<GameObject> Q_BulletPool = new Queue<GameObject>();
    [FoldoutGroup("Shooting"),Title("Shooting", titleAlignment: TitleAlignments.Centered)]
    [FoldoutGroup("Shooting")] public Transform T_ShootingPoint;
    [FoldoutGroup("Shooting")] public float F_ShootDelay;
    [FoldoutGroup("Shooting"), ShowInInspector, ReadOnly] private bool B_CanShoot = true;
    [FoldoutGroup("Shooting"), ShowInInspector, ReadOnly] private bool B_IsShooting = false;
    [FoldoutGroup("Shooting"), ShowInInspector, ReadOnly, PropertySpace(SpaceAfter = 15)] private float F_ShootAngle;

    //----------------------GUI----------------------------//
    [FoldoutGroup("GUI"),ShowInInspector] private Slider Sl_LifeBar;
    [FoldoutGroup("GUI"),ShowInInspector] private TMP_Text Tx_Heals;
    [FoldoutGroup("GUI"),ShowInInspector] private TMP_Text Tx_ActDamage;


    void Awake()
    {
        if (!current)
            current = this;
        else if (current != this)
            Destroy(this);
    }

    private void Start() //Setear input para actualizar dirección de los disparos y resetar vida
    {
        InputController.current.InputManager.Player.Shooting.performed += ShootAngle;
        I_ActHealth = I_MaxHealth;
        I_ActDamage = SetDamage();
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

    #region Initial configuration

    private void SetPool() //Iniciacion del object pool de balas
    {
        for (int i = 0; i < I_BulletPoolSize; i++)
        {
            StoreBullet(Instantiate(G_BulletPrefab));
        }
    }
    
    private int SetDamage() //Configura daño del player considerando un porcentaje de vida faltante
    {
        float temp = I_BaseDamage + ((I_MaxHealth-I_ActHealth)* F_DamageScale);
        return (int)temp;
        
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
            return Instantiate(G_BulletPrefab);

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

}
