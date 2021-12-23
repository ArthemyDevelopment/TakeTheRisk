using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField]private int I_Coste;
    [SerializeField]private int I_AumentoCoste;
    [SerializeField]private Button B_BotonMejora;
    [SerializeField]private TMP_Text Tx_Lv;

    private void OnEnable()
    {

        UpdateInfo();
    }

    void UpdateInfo()
    {
        B_BotonMejora.interactable = CanUpgrade();
        Tx_Lv.text = I_ActLv.ToString();
        I_Coste = I_Coste + (I_AumentoCoste * I_ActLv);
    }
    
    bool CanUpgrade()
    {
        return I_Coste <= PlayerManager.current.I_PuntosMejoras;
    }

    public float UpgradeStat(float f)
    {
        return f;
    }

    public int UpgradeStat(int i)
    {
        return i;
    }
    
    
    
}
