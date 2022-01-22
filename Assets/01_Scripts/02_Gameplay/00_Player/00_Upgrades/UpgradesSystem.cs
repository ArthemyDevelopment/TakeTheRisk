using System.Collections.Generic;
using System.Globalization;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradesSystem : MonoBehaviour
{
    
    /*
     * 
     */
    public static UpgradesSystem current;//Singleton
    
    [FoldoutGroup("Variables"), ShowInInspector, ReadOnly] private bool B_isOpen; //Comprueba si se abrio el menu
    [FoldoutGroup("Variables"), ReadOnly] public bool B_CanUpgrade; //Comprueba si se puede upgradear
    [FoldoutGroup("Variables"), ShowInInspector, ReadOnly] private int I_TotalLv; //Suma total de niveles de mejora
    [FoldoutGroup("Variables")] public int I_PuntosMejora; //Cuantos puntos de mejora se tienen
    [FoldoutGroup("Variables")] public int I_Coste;  //Cuanto cuesta una nueva mejora
    [FoldoutGroup("Variables")][SerializeField]private float F_PrevValueScale; //Aumento de coste en base al valor anterior
    [FoldoutGroup("Variables")][SerializeField]private float F_LvValueScale; //Aumento de coste en base al nivel
    [FoldoutGroup("Variables")][SerializeField]private int I_FlatSumScale; //Aumento de coste en base a un valor plano

    [FoldoutGroup("References")] public List<Upgrade> Up_Upgrades; //Ref a todos los scripts de upgrades
    [FoldoutGroup("References")] public GameObject G_UpgradeMenuCanvas; //Ref al canvas del menu
    
    //Ref a todos los textos de las stats
    [FoldoutGroup("References/Points")] public TMP_Text Tx_TotalLv;
    [FoldoutGroup("References/Points")] public TMP_Text Tx_ActPoints;
    [FoldoutGroup("References/Points")] public TMP_Text Tx_HudPoints;
    [FoldoutGroup("References/Points")] public TMP_Text Tx_ActCost;
    [FoldoutGroup("References/Stats")] public TMP_Text Tx_ActVida;
    [FoldoutGroup("References/Stats")] public TMP_Text Tx_MaxVida;
    [FoldoutGroup("References/Stats")] public TMP_Text Tx_ActHeals;
    [FoldoutGroup("References/Stats")] public TMP_Text Tx_MaxHeals;
    [FoldoutGroup("References/Stats")] public TMP_Text Tx_Healling;
    [FoldoutGroup("References/Stats")] public TMP_Text Tx_SelfDmg;
    [FoldoutGroup("References/Stats")] public TMP_Text Tx_BaseDmg;
    [FoldoutGroup("References/Stats")] public TMP_Text Tx_DmgScale;
    [FoldoutGroup("References/Stats")] public TMP_Text Tx_FinalDmg;
    [FoldoutGroup("References/Stats")] public TMP_Text Tx_ParryCD;
    [FoldoutGroup("References/Stats")] public TMP_Text Tx_Parry;
    
    private void Awake()
    {
        if (current == null)
            current = this;
        else if(current != this)
            Destroy(this);
    }

    private void Start()
    {
        InputController.current.InputManager.Player.UpgradeMenu.performed += OpenUpgradeMenu;
    }

    private void OpenUpgradeMenu(InputAction.CallbackContext obj)//Middle point entre la deteccion de input y el methodo de abrir el menu
    {
        OpenUpgradeMenu();
    }


    public void OpenUpgradeMenu() //Abrir y cerra el menu de mejora
    {
        switch (B_isOpen)
        {
            case false:
                G_UpgradeMenuCanvas.SetActive(true);
                InputController.current.InputManager.UI.Enable();
                InputController.current.InputManager.Player.Disable();
                B_isOpen = true;
                UpdateText();
                break;
            
            case true:
                G_UpgradeMenuCanvas.SetActive(false);
                InputController.current.InputManager.Player.Enable();
                InputController.current.InputManager.UI.Disable();
                B_isOpen = false;
                break;
        }
    }
    


    //Cada metodo toma como parametro el script de upgrade y cambia el valor de la stat aplicando la formula de upgradear
    #region Upgrades Methods 

    public void UpgradeMaxHealth(Upgrade u)
        {
            PlayerManager.current.I_MaxHealth = u.UpgradeStat(PlayerManager.current.I_MaxHealth);
            PlayerManager.current.I_ActHealth = PlayerManager.current.I_MaxHealth;
            UpgradeChanges();
        }
    public void UpgradeHeal(Upgrade u)
        {
            PlayerManager.current.I_Healling = u.UpgradeStat(PlayerManager.current.I_Healling);
            UpgradeChanges();
        }
    public void UpgradeSelfDmg(Upgrade u)
        {
            PlayerManager.current.I_SelfDamage = u.UpgradeStat(PlayerManager.current.I_SelfDamage);
            UpgradeChanges();
        }
    public void UpgradeBaseDmg(Upgrade u)
        {
            PlayerManager.current.I_BaseDamage = u.UpgradeStat(PlayerManager.current.I_BaseDamage);
            UpgradeChanges();
        }
    public void UpgradeDmgScale(Upgrade u)
        {
            PlayerManager.current.F_DamageScale = u.UpgradeStat(PlayerManager.current.F_DamageScale);
            UpgradeChanges();
        }
    public void UpgradeNumHeals(Upgrade u)
        {
            PlayerManager.current.I_MaxHeals = u.UpgradeStat(PlayerManager.current.I_MaxHeals);
            PlayerManager.current.I_ActHeals = PlayerManager.current.I_MaxHeals;
            UpgradeChanges();
        }
    public void UpgradeParry(Upgrade u)
        {
            PlayerManager.current.F_ParryDuration = u.UpgradeStat(PlayerManager.current.F_ParryDuration);
            UpgradeChanges();
        }
    public void UpgradeCDParry(Upgrade u)
        {
            PlayerManager.current.F_ParryCD = u.UpgradeStat(PlayerManager.current.F_ParryCD);
            UpgradeChanges();
        }

    #endregion

    void UpgradeChanges() //Aplica los cambios a los valores despues de cada mejora, tanto de nivel total, puntos actual, coste del sig nivel e información en pantalla
    {
        I_TotalLv++;
        I_PuntosMejora -= I_Coste;
        if (I_TotalLv < 160)
        {
            int temp = Mathf.RoundToInt(I_Coste * F_PrevValueScale) + Mathf.RoundToInt((I_TotalLv+1) * F_LvValueScale) + I_FlatSumScale;
            I_Coste = temp;
        }
        
        UpdateText();
        foreach (Upgrade u in Up_Upgrades)
        {
            u.UpdateInfo();
        }
    }

    void UpdateText() //Actualiza todos los textos con los valores actuales
    {
        if (I_TotalLv < 160)
            Tx_ActCost.text = I_Coste.ToString();
        else
            Tx_ActCost.text = "Max";
        Tx_ActPoints.text = I_PuntosMejora.ToString();
        Tx_HudPoints.text = I_PuntosMejora.ToString();
        Tx_TotalLv.text = I_TotalLv.ToString();
        Tx_ActVida.text = PlayerManager.current.I_ActHealth.ToString();
        Tx_MaxVida.text = PlayerManager.current.I_MaxHealth.ToString();
        Tx_ActHeals.text = PlayerManager.current.I_ActHeals.ToString();
        Tx_MaxHeals.text = PlayerManager.current.I_MaxHeals.ToString();
        Tx_Healling.text = PlayerManager.current.I_Healling.ToString();
        Tx_SelfDmg.text = PlayerManager.current.I_SelfDamage.ToString();
        Tx_BaseDmg.text = PlayerManager.current.I_BaseDamage.ToString();
        Tx_DmgScale.text = PlayerManager.current.F_DamageScale.ToString("F2",CultureInfo.InvariantCulture);
        Tx_FinalDmg.text = PlayerManager.current.I_ActDamage.ToString();
        Tx_ParryCD.text = PlayerManager.current.F_ParryCD.ToString("F2",CultureInfo.InvariantCulture);
        Tx_Parry.text = PlayerManager.current.F_ParryDuration.ToString("F2",CultureInfo.InvariantCulture);


    }

    public void GetPoints(int i) //Metodo para obtener los puntos al derrotar a un enemigo
    {
        I_PuntosMejora += i;
        Tx_HudPoints.text = I_PuntosMejora.ToString();
        
    }


}

