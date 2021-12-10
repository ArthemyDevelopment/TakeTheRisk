using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ArthemyDevelopment.Save;
using UnityEngine;

public class DebugSaveGame : MonoBehaviour
{
    private SaveData SD = new SaveData();
    private GameData GD = new GameData(); 
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SD.SaveValue(SaveDataKeys.DataClass, GD);
            string[] savefiles = Directory.GetFiles(Application.persistentDataPath,
                SaveDataPreferences.current.fileFormat,
                SearchOption.AllDirectories);
            int temp = savefiles.Length;
            SD.SaveDataFile(temp);
            Debug.Log("Game File Created " + temp);
        }
    }
}
