using System.Collections;
using System.Collections.Generic;
using System.IO;
using ArthemyDevelopment.Save;
using UnityEngine;

public class SaveFilesManager : MonoBehaviour
{
    public GameObject G_SaveFilePrefab;
    public GameObject G_parentContent;

    private void OnEnable()
    {
        GetFiles();
    }

    private void OnDisable()
    {
        CleanSaveFiles();
    }

    void GetFiles()
    {
        for (int i = 0; i < G_parentContent.transform.childCount; i++)
        {
            G_parentContent.transform.GetChild(i).GetComponent<SaveFilePrefab>().GetGameData();
        }
    }



    void CleanSaveFiles()
    {   
        
    }
}
