using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogosSceneManager : MonoBehaviour
{
    public GameObject G_Logos;
    public GameObject G_LaguageSelection;
    public Animator An_LogosAnim;
    
    private void Start()
    {
        if(PlayerPrefs.HasKey("ADLocalizationIndex"))
        {
            G_LaguageSelection.SetActive(false);
            G_Logos.SetActive(true);
            An_LogosAnim.Play("start");    
        }

        else
        {
            G_LaguageSelection.SetActive(true);
            G_Logos.SetActive(false);
            
        }
    }


    public void SelectedLanguage()
    {
        G_LaguageSelection.SetActive(false);
        G_Logos.SetActive(true);
        An_LogosAnim.Play("start");
    }
}
