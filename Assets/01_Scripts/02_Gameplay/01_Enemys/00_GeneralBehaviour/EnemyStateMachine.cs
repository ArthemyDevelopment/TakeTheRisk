using System;
using UnityEngine;
using UnityEngine.Events;

public class EnemyStateMachine : MonoBehaviour
{

    public enum EnemyState
    {
        Idle,
        Shooting,
        Aggro,
        Follow,
        Back
    }

    public bool B_CanAggro = true;
    public bool B_CanShoot = true;
    
    [SerializeField]private EnemyState m_enemyState;

    public EnemyState enemyState
    {
        get => m_enemyState;
        set
        {
            m_enemyState = value;
            UpdateState(); 
        }
    }

    public UnityEvent E_Shooting;
    public UnityEvent E_Aggro;
    public UnityEvent E_Follow;
    public UnityEvent E_Back;



    void UpdateState()
    {
        switch (enemyState)
        {
            case EnemyState.Shooting:
                E_Shooting.Invoke();
                break;
            
            case EnemyState.Aggro:
                E_Aggro.Invoke();
                break;
            
            case EnemyState.Follow:
                E_Follow.Invoke();
                break;
            
            case EnemyState.Back:
                E_Back.Invoke();
                break;
            
            default:
                break;
        }
    }

    private void Update()
    {
        if (enemyState == EnemyState.Shooting && PlayerManager.current.B_isDeath)
        {
            enemyState = EnemyState.Back;
        }
    }


    public void ExitShooting()
    {
        B_CanShoot = false;
        switch (B_CanAggro)
        {
            case true:
                enemyState = EnemyState.Aggro;
                break;
            case false:
                enemyState = EnemyState.Back;
                B_CanAggro = true;
                break;
        }
    }


}
