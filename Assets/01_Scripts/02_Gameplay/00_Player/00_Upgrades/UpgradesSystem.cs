using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesSystem : MonoBehaviour
{


    #region Upgrades Methods

    /*
    Each method take as param the script of the upgrade and apply the return method
    UpgradeStat to the var from PlayerManager
    */
    public void UpgradeMaxHealth(Upgrade u)
        {
            PlayerManager.current.I_MaxHealth = u.UpgradeStat(PlayerManager.current.I_MaxHealth);
        }
    public void UpgradeHeal(Upgrade u)
        {
            PlayerManager.current.I_Healling = u.UpgradeStat(PlayerManager.current.I_Healling);
        }
    public void UpgradeSelfDmg(Upgrade u)
        {
            PlayerManager.current.I_SelfDamage = u.UpgradeStat(PlayerManager.current.I_SelfDamage);
        }
    public void UpgradeBaseDmg(Upgrade u)
        {
            PlayerManager.current.I_BaseDamage = u.UpgradeStat(PlayerManager.current.I_BaseDamage);
        }
    public void UpgradeDmgScale(Upgrade u)
        {
            PlayerManager.current.F_DamageScale = u.UpgradeStat(PlayerManager.current.F_DamageScale);
        }
    public void UpgradeNumHeals(Upgrade u)
        {
            PlayerManager.current.I_HealUse = u.UpgradeStat(PlayerManager.current.I_HealUse);
        }
    public void UpgradeParry(Upgrade u)
        {
            PlayerManager.current.F_ParryTime = u.UpgradeStat(PlayerManager.current.F_ParryTime);
        }
    public void UpgradeCDParry(Upgrade u)
        {
            PlayerManager.current.F_ParryCD = u.UpgradeStat(PlayerManager.current.F_ParryCD);
        }

    #endregion

}

