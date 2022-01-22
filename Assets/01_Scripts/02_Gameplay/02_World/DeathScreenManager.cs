using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


public class DeathScreenManager : MonoBehaviour
{
    public static DeathScreenManager current;
    [SerializeField] private Animator An_DeathScreen;
    [SerializeField] private List<String> S_Tips;
    [SerializeField] private TMP_Text Tx_tipsText;
    


    private void Awake()
    {
        if (current == null)
            current = this;
        else if(current != this)
            Destroy(this);
    }

    public void GetTipsStrings(string s)
    {
        S_Tips.Add(s);
    }



    public void DeathScreen(bool b)
    {
        switch (b)
        {
            case true:
                An_DeathScreen.Play("StartAnimation");
                Tx_tipsText.text = S_Tips[Random.Range(0, S_Tips.Count)];
                break;
            case false:
                An_DeathScreen.Play("EndAnimation");
                break;
        }
    }
    
}



