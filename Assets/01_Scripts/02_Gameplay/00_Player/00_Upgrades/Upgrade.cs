using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(1)]
public class Upgrade : MonoBehaviour
{
    enum TypeOfUpgrade
    {
        sum,
        rest,
        mult
    }
    
    
    [SerializeField]private TypeOfUpgrade type_Upgrade;
    [SerializeField]private int I_ActLv;
    [SerializeField]private float F_RatioMejora;
    [SerializeField]private Button B_BotonMejora;
    [SerializeField]private TMP_Text Tx_Lv;

    private void OnEnable()
    {

        UpdateInfo();
    }

    public void UpdateInfo()
    {
        B_BotonMejora.interactable = CanUpgrade();
        Tx_Lv.text = I_ActLv.ToString();
    }
    
    
    bool CanUpgrade()
    {
        return UpgradesSystem.current.I_Coste <= UpgradesSystem.current.I_PuntosMejora;
    }

    public float UpgradeStat(float f)
    {
        float temp = 0;
        switch (type_Upgrade)
        {
            case TypeOfUpgrade.sum:
                temp = (f + F_RatioMejora);
                break;
            
            case TypeOfUpgrade.rest:
                temp = (f - F_RatioMejora);
                break;
            
            case TypeOfUpgrade.mult:
                temp = (f * F_RatioMejora);
                break;
        }
        I_ActLv++;
        return temp;

    }

    public int UpgradeStat(int i)
    {
        int temp = 0;
        switch (type_Upgrade)
        {
            case TypeOfUpgrade.sum:
                temp = (int)(i + F_RatioMejora);
                break;
            
            case TypeOfUpgrade.rest:
                temp = (int)(i - F_RatioMejora);
                break;
            
            case TypeOfUpgrade.mult:
                temp = (int)(i * F_RatioMejora);
                break;
        }
        I_ActLv++;

        return temp;
    }


    
    
    
}
