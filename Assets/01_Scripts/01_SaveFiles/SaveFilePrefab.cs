using System;
using System.IO;
using ArthemyDevelopment.Save;
using Ludiq.PeekCore;
using UnityEngine;
using TMPro;

public class SaveFilePrefab : MonoBehaviour
{
    public SaveData SD = new SaveData();
    private GameData SaveFileData;

    public int FileIndex;
    
    public TMP_Text Tx_LevelText;
    public TMP_Text Tx_PlayerLv;

    public void GetGameData()
    {
        Debug.Log("Game Data " + FileIndex);
        SD.LoadDataFile(FileIndex);
        if (SD.SuccesLoad)
        {
            SD.LoadValue(SaveDataKeys.DataClass, out SaveFileData);
            ActulizeInfo();
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    void ActulizeInfo()
    {
        Tx_LevelText.text = Enum.GetName(typeof(Level), SaveFileData.ActLevel);
        Tx_PlayerLv.text = SaveFileData.TotalLv.ToString();
    }



}
