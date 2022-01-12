using UnityEngine;

public class ShootingTrigger : MonoBehaviour
{
    [SerializeField] private EnemyStateMachine StateMachine;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player/Collider"))
        {
            StateMachine.B_CanShoot = true;
        if(StateMachine.enemyState == EnemyStateMachine.EnemyState.Aggro)
            StateMachine.enemyState = EnemyStateMachine.EnemyState.Shooting;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player/Collider"))
            StateMachine.ExitShooting();
        
    }
}
