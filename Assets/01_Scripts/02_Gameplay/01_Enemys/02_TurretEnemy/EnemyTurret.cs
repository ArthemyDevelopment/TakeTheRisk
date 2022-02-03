using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurret : EnemyMovement
{
    public enum TurretType
    {
        Follow, 
        Stand
    }

    [SerializeField] private TurretType turretType;
    [SerializeField] private Transform T_Player;
    [SerializeField] private bool B_isLooking;
    [SerializeField] private float F_RotationSmoothing;
    [SerializeField] private float F_LookAngle;

    public override void ResetEnemy()
    {
        B_isLooking = false;
    }

    private void OnEnable()
    {
        T_Player = PlayerManager.current.transform;
        
    }


    public void OnShooting()
    {
        //TODO:Set Visual clue
        B_isLooking = true;
    }
    
    public void OnAggro()
    {
        //TODO:Set Visual clue
        B_isLooking = true;
    }

    public void OnFollow()
    {
        
    }

    public void OnBack()
    {
        //TODO:Unset Visual clue
        B_isLooking = false;
    }


    private void Update()
    {
        if (B_isLooking)
        {
            switch (turretType)
            {
                case TurretType.Follow:
                    F_LookAngle = Mathf.Atan2(T_Player.position.z - transform.position.z, T_Player.position.x - transform.position.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Lerp(transform.rotation,Quaternion.Euler(0, -F_LookAngle, 0), Time.deltaTime * F_RotationSmoothing);
                    break;
                case TurretType.Stand:
                    
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            
        }
    }
}
