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
    [FoldoutGroup("Variables"), ShowInInspector, ReadOnly] private int I_TotalLv; //Suma total de niveles de mejora
    [FoldoutGroup("Variables")] public int I_PuntosMejora; //Cuantos puntos de mejora se tienen
    [FoldoutGroup("Variables")] public int I_Coste;  //Cuanto cuesta una nueva mejora
    [FoldoutGroup("Variables")][SerializeField] private float F_AumentoCoste; //Por cuanto se multiplica el coste de mejora con cada nivel

    [FoldoutGroup("References")] public List<Upgrade> Up_Upgrades; //Ref a todos los scripts de upgrades
    [FoldoutGroup("References")] public GameObject G_UpgradeMenuCanvas; //Ref al canvas del menu
    
    //Ref a todos los textos de las stats
    [FoldoutGroup("References/Points")] public TMP_Text Tx_TotalLv;
    [FoldoutGroup("References/Points")] public TMP_Text Tx_ActPoints;
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
        InputController.current.InputManager.Player.UpgradeMenu.performed += OpenUpgradeMenu; //Setear y asegurarse que el input este configurado
        InputController.current.InputManager.UI.UpgradeMenu.performed += OpenUpgradeMenu; //Setear y asegurarse que el input este configurado
        InputController.current.InputManager.UI.Cancel.performed += OpenUpgradeMenu; //Setear y asegurarse que el input este configurado
        UpdateText();
    }

    public void OpenUpgradeMenu(InputAction.CallbackContext call) //Abrir y cerra el menu de mejora
    {
        switch (B_isOpen)
        {
            case false:
                G_UpgradeMenuCanvas.SetActive(true);
                InputController.current.InputManager.UI.Enable();
                InputController.current.InputManager.Player.Disable();
                B_isOpen = true;
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
            PlayerManager.current.SetDamage();
            UpgradeChanges();
        }
    public void UpgradeDmgScale(Upgrade u)
        {
            PlayerManager.current.F_DamageScale = u.UpgradeStat(PlayerManager.current.F_DamageScale);
            PlayerManager.current.SetDamage();
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
        I_Coste = Mathf.RoundToInt(I_Coste * F_AumentoCoste);
        UpdateText();
        foreach (Upgrade u in Up_Upgrades)
        {
            u.UpdateInfo();
        }
    }

    void UpdateText() //Actualiza todos los textos con los valores actuales
    {
        Tx_ActCost.text = I_Coste.ToString();
        Tx_ActPoints.text = I_PuntosMejora.ToString();
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


}

