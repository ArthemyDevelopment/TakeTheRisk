using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneGameManager : MonoBehaviour
{
    //TODO: ScenesToOpen
    [SerializeField]private List<EnemyController> EC_ZoneEnemies;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player/Collider"))
        {
            PlayerRespawnManager.current.ZGM_ActZone = this;
            LoadRelatedZones();
            UnLoadUnrelatedZones();
        }
    }


    public void ResetEnemies()
    {
        foreach (EnemyController EC in EC_ZoneEnemies)
        {
            EC.ResetEnemy();
        }
    }

    void LoadRelatedZones() //Load scenes aditive from the conected zones 
    {
        
    }

    void UnLoadUnrelatedZones() //Unload scenes that are not conected zones
    {
        
    }
}
