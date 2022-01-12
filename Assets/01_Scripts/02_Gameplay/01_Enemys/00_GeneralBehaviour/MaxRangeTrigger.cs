using UnityEngine;

public class MaxRangeTrigger : MonoBehaviour
{
    [SerializeField] private EnemyStateMachine StateMachine;


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy/Hitbox"))
            StateMachine.enemyState = EnemyStateMachine.EnemyState.Back;
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy/Hitbox"))
            StateMachine.enemyState = EnemyStateMachine.EnemyState.Follow;
        
    }
}
