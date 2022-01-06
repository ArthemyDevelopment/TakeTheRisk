using UnityEngine;

public class AggroTrigger : MonoBehaviour
{
    [SerializeField] private EnemyStateMachine StateMachine;
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy/Hitbox"))
        {
            StateMachine.B_CanAggro = false;
            StateMachine.enemyState = EnemyStateMachine.EnemyState.Follow;
        }

        if (other.CompareTag("Player/Collider") && StateMachine.enemyState != EnemyStateMachine.EnemyState.Shooting)
            StateMachine.enemyState = EnemyStateMachine.EnemyState.Back;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player/Collider"))
        {
            
            if (StateMachine.enemyState == EnemyStateMachine.EnemyState.Idle)
            {
                StateMachine.B_CanAggro = true;
                StateMachine.enemyState = EnemyStateMachine.EnemyState.Aggro;
            }
            else if (StateMachine.enemyState == EnemyStateMachine.EnemyState.Follow)
                StateMachine.enemyState = EnemyStateMachine.EnemyState.Aggro;
            
            else if (StateMachine.enemyState == EnemyStateMachine.EnemyState.Back)
            {
                if (StateMachine.B_CanAggro)
                    StateMachine.enemyState = EnemyStateMachine.EnemyState.Aggro;
                
                else
                    StateMachine.B_CanAggro = true;
            }
            
        }

        if (other.CompareTag("Enemy/Hitbox"))
        {
            if (StateMachine.B_CanAggro)
                StateMachine.enemyState = EnemyStateMachine.EnemyState.Aggro;
            else
                StateMachine.B_CanAggro = true;

        }
        
    }
}
