using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GraphicManager : MonoBehaviour
{
    public TMP_Dropdown QualityDropDown;
    public TMP_Dropdown WindowDropDown;
    List<string> QTemp = new List<string>();
    List<string> WTemp = new List<string>();
    private void OnDisable()
    {
        QualityDropDown.ClearOptions();
        WindowDropDown.ClearOptions();
    }

    public void SetQualityOption(string s)
    {
        QTemp.Add(s);

        if (QTemp.Count == 5)
        {
            QualityDropDown.AddOptions(QTemp);
            QTemp.Clear();
        }
            
        
    }

    public void SetWindowOptions(string s)
    {
        WTemp.Add(s);

        if (WTemp.Count == 3)
        {
            WindowDropDown.AddOptions(WTemp);
            WTemp.Clear();
        }
    }


    public void ChangeQuality(int i)
    {
        QualitySettings.SetQualityLevel(i);
    }

    public void SetWindows(int i)
    {
       switch(i)
       {
           case 0:
               Screen.fullScreen = false;
               Screen.fullScreenMode = FullScreenMode.Windowed;
               break;
           
           case 1:
               Screen.fullScreen = true;
               Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
               break;
           
           case 2:
               Screen.fullScreen = true;
               Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
               break;
       }
        
    }


    
}
