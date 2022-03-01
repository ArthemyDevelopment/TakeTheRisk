using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingZonesScenes : MonoBehaviour
{
    [SerializeField] private ScenesIndex Zone;
    [SerializeField] private List<LoadingZonesScenes> ZonesToLoad;

    [SerializeField] private bool B_isLoaded;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player/Collider"))
        {
            LoadZone();
            LoadingZonesManager.current.SetZone(this);
            if (ZonesToLoad != null)
            {
                foreach (LoadingZonesScenes s in ZonesToLoad)
                {
                    s.LoadZone();
                }
                
            }
            UnLoadZones();
            
        }
    }

    public void LoadZone()
    {
        if (!B_isLoaded)
        {
            B_isLoaded = true;
            SceneManager.LoadSceneAsync((int)Zone, LoadSceneMode.Additive);
            Debug.Log("Loaded Zone" + Zone);
        }
    }

    public void UnLoadZones()
    {
        List<LoadingZonesScenes> temp  = LoadingZonesManager.current.ActZone.ZonesToLoad;
        foreach (LoadingZonesScenes s in temp)
        {
            if (s.B_isLoaded && !ZonesToLoad.Contains(s) && s != this)
            {
                s.B_isLoaded = false;
                SceneManager.UnloadSceneAsync((int)s.Zone);
            }
        }
    }
    
    
}
