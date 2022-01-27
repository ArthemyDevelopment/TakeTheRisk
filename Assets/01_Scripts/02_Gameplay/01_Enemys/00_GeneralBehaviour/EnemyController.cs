using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private GameObject G_Enemy;
    [SerializeField] private EnemyStateMachine ESM;
    [SerializeField] private EnemyMovement EM;


    public void ResetEnemy()
    {
        G_Enemy.transform.position = transform.position;
        ESM.enemyState = EnemyStateMachine.EnemyState.Idle;
        ESM.B_CanAggro = true;
        ESM.B_CanShoot = false;
        EM.B_FollowPlayer = false;
        gameObject.SetActive(true);
    }

}
