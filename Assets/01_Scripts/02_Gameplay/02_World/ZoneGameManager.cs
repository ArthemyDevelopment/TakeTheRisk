using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneGameManager : MonoBehaviour
{

    [SerializeField]private List<EnemyController> EC_ZoneEnemies;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player/Collider"))
        {
            PlayerRespawnManager.current.ZGM_ActZone = this;

        }
    }


    public void ResetEnemies()
    {
        foreach (EnemyController EC in EC_ZoneEnemies)
        {
            EC.ResetEnemy();
        }
    }
    
}
