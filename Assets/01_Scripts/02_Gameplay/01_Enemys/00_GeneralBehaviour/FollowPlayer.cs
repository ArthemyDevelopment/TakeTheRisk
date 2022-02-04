using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowPlayer : EnemyMovement
{
    [SerializeField] private EnemyStateMachine ESM;
    
    [SerializeField] private NavMeshAgent Nav_Enemy;
    [SerializeField] private Transform T_Origin;
    public bool B_FollowPlayer = false;
    [SerializeField]private float F_LookAngle;
    [SerializeField]private float F_RotationSmoothing;
    [SerializeField] private Transform T_LookTarget;

    public override void ResetEnemy()
    {
        B_FollowPlayer = false;
    }
    
    public void OnShooting()
    {
        B_FollowPlayer = true;
        T_LookTarget = PlayerManager.current.transform;
        Nav_Enemy.stoppingDistance = 3f;
    }

    public void OnAggro()
    {
        B_FollowPlayer = true;
        T_LookTarget = PlayerManager.current.transform;
        Nav_Enemy.stoppingDistance = 3f;
    }

    public void OnFollow()
    {
        
    }

    public void OnBack()
    {
        B_FollowPlayer = false;
        T_LookTarget = T_Origin;
        Nav_Enemy.ResetPath();
        Nav_Enemy.SetDestination(T_Origin.position);
        Nav_Enemy.stoppingDistance = 0.5f;
    }

    private void Update()
    {
        if (B_FollowPlayer)
        {
            Nav_Enemy.SetDestination(PlayerManager.current.transform.position);
            F_LookAngle = Mathf.Atan2(T_LookTarget.position.z - transform.position.z, T_LookTarget.position.x - transform.position.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0, -F_LookAngle, 0), Time.deltaTime * F_RotationSmoothing);
        }
        

        if (Nav_Enemy.pathStatus == NavMeshPathStatus.PathComplete && ESM.enemyState == EnemyStateMachine.EnemyState.Back)
            ESM.enemyState = EnemyStateMachine.EnemyState.Idle;
        
    }
}
