using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private EnemyStateMachine ESM;
    
    [SerializeField] private NavMeshAgent Nav_Enemy;
    [SerializeField] private Transform T_Origin;
    [SerializeField] private bool B_FollowPlayer = false;

    public void OnShooting()
    {
        B_FollowPlayer = true;
    }

    public void OnAggro()
    {
        B_FollowPlayer = true;
    }

    public void OnFollow()
    {
        
    }

    public void OnBack()
    {
        B_FollowPlayer = false;
        Nav_Enemy.ResetPath();
        Nav_Enemy.SetDestination(T_Origin.position);
    }

    private void Update()
    {
        if (B_FollowPlayer)
            Nav_Enemy.SetDestination(PlayerManager.current.transform.position);
        

        if (Nav_Enemy.pathStatus == NavMeshPathStatus.PathComplete && transform.position == T_Origin.position)
            ESM.enemyState = EnemyStateMachine.EnemyState.Idle;
        
    }
}
