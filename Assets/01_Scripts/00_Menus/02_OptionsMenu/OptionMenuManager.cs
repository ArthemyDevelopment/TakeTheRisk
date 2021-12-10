using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OptionMenuManager : MonoBehaviour
{
    public static OptionMenuManager current;

    public GameObject G_OptionCanvas;
    public GameObject G_OptionMenu;
    public GameObject G_AudioOption;
    public GameObject G_LanguageOption;
    public GameObject G_GraphicOption;

    public UnityEvent Ev_CloseMenu;


    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else if (current != this)
        {
            Destroy(this);
        }
        DeactiveObject();
        G_OptionCanvas.SetActive(false);
    }

    public void OpenMenu()
    {
        DeactiveObject();
        G_OptionCanvas.SetActive(true);
        G_OptionMenu.SetActive(true);
    }

    public void GoAudio()
    {
        DeactiveObject();
        G_AudioOption.SetActive(true);
    }

    public void GoLanguage()
    {
        DeactiveObject();
        G_LanguageOption.SetActive(true);
    }

    public void GoGraphic()
    {
        DeactiveObject();
        G_GraphicOption.SetActive(true);
    }

    public void CloseMenu()
    {
        DeactiveObject();
        G_OptionCanvas.SetActive(false);
        Ev_CloseMenu.Invoke();
    }
    
    public void DeactiveObject()
    {
        G_OptionMenu.SetActive(false);
        G_AudioOption.SetActive(false);
        G_LanguageOption.SetActive(false);
        G_GraphicOption.SetActive(false);
    }
    
}
