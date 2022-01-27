using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneGameManager : MonoBehaviour
{
    [SerializeField]private List<EnemyController> EC_ZoneEnemies;




    public void ResetEnemies()
    {
        foreach (EnemyController EC in EC_ZoneEnemies)
        {
            EC.ResetEnemy();
        }
    }
}
