using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetStartingPosition : MonoBehaviour
{
    [SerializeField] private Transform T_EnemyPosition;


    private void Start()
    {
        transform.position = T_EnemyPosition.position;
    }
}
