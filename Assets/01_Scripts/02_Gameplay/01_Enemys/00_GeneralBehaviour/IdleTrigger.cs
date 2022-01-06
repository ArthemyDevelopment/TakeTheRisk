using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleTrigger : MonoBehaviour
{
    [SerializeField] private EnemyStateMachine StateMachine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy/Hitbox"))
            StateMachine.enemyState = EnemyStateMachine.EnemyState.Idle;
        
    }
}
