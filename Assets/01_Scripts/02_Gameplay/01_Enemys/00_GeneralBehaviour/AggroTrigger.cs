
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
        else
        {
            StateMachine.enemyState = EnemyStateMachine.EnemyState.Follow;
            StateMachine.B_CanAggro = false;
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player/Collider"))
        {
            StateMachine.B_CanAggro = true;
        }

        if (other.CompareTag("Enemy/Hitbox") && StateMachine.enemyState == EnemyStateMachine.EnemyState.Back)
        {
            StateMachine.B_CanAggro = true;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player/Collider"))
        {
            switch (StateMachine.enemyState)
            {
                case EnemyStateMachine.EnemyState.Idle:
                    StateMachine.enemyState = EnemyStateMachine.EnemyState.Aggro; 
                    break;
                case EnemyStateMachine.EnemyState.Shooting:
                    
                    
                    break;
                case EnemyStateMachine.EnemyState.Aggro:
                    
                    
                    break;
                case EnemyStateMachine.EnemyState.Follow:
                    if(StateMachine.B_CanShoot)
                        StateMachine.enemyState = EnemyStateMachine.EnemyState.Shooting;
                    else
                        StateMachine.enemyState = EnemyStateMachine.EnemyState.Aggro;
                    break;
                case EnemyStateMachine.EnemyState.Back:
                    if (StateMachine.B_CanAggro)
                    {
                        if(StateMachine.B_CanShoot)
                            StateMachine.enemyState = EnemyStateMachine.EnemyState.Shooting;
                        else
                            StateMachine.enemyState = EnemyStateMachine.EnemyState.Aggro;
                    }
                    break;

            }

        }


        
    }
}
