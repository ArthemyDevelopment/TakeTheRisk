using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpgradesSystem : MonoBehaviour
{
    public static UpgradesSystem current;
    
    [FoldoutGroup("Variables"), ShowInInspector, ReadOnly] private bool B_isOpen;
    [FoldoutGroup("Variables"), ShowInInspector, ReadOnly] private int I_TotalLv;
    [FoldoutGroup("Variables")] public int I_PuntosMejora;
    [FoldoutGroup("Variables")] public int I_Coste;
    [FoldoutGroup("Variables"), ShowInInspector] private int I_AumentoCoste;

    [FoldoutGroup("References")] public List<Upgrade> Up_Upgrades;
    [FoldoutGroup("References")] public GameObject G_UpgradeMenuCanvas;
    [FoldoutGroup("References")] public TMP_Text Tx_TotalLv;
    [FoldoutGroup("References")] public TMP_Text Tx_ActPoints;
    [FoldoutGroup("References")] public TMP_Text Tx_ActCost;
    


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
        UpdateText();
    }

    public void OpenUpgradeMenu(InputAction.CallbackContext call)
    {
        switch (B_isOpen)
        {
            case false:
                G_UpgradeMenuCanvas.SetActive(true);
                B_isOpen = true;
                break;
            
            case true:
                G_UpgradeMenuCanvas.SetActive(false);
                B_isOpen = false;
                break;
        }
    }

    #region Upgrades Methods

    /*
    Each method take as param the script of the upgrade and apply the return method
    UpgradeStat to the var from PlayerManager
    */
    public void UpgradeMaxHealth(Upgrade u)
        {
            PlayerManager.current.I_MaxHealth = u.UpgradeStat(PlayerManager.current.I_MaxHealth);
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
            PlayerManager.current.I_HealUse = u.UpgradeStat(PlayerManager.current.I_HealUse);
            UpgradeChanges();
        }
    public void UpgradeParry(Upgrade u)
        {
            PlayerManager.current.F_ParryTime = u.UpgradeStat(PlayerManager.current.F_ParryTime);
            UpgradeChanges();
        }
    public void UpgradeCDParry(Upgrade u)
        {
            PlayerManager.current.F_ParryCD = u.UpgradeStat(PlayerManager.current.F_ParryCD);
            UpgradeChanges();
        }


    void UpgradeChanges()
    {
        I_TotalLv++;
        I_PuntosMejora -= I_Coste;
        I_Coste += I_AumentoCoste;
        UpdateText();
        foreach (Upgrade u in Up_Upgrades)
        {
            u.UpdateInfo();
        }
    }

    void UpdateText()
    {
        Tx_ActCost.text = I_Coste.ToString();
        Tx_ActPoints.text = I_PuntosMejora.ToString();
        Tx_TotalLv.text = I_TotalLv.ToString();
    }

    #endregion

}

