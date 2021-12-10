using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ArthemyDevelopment.Save;
using UnityEngine;

public class SaveDataManager : MonoBehaviour
{
    private SaveData SD = new SaveData();
    
    public static SaveDataManager current;

    public GameData ActGameData;
    
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
    }

    
}

