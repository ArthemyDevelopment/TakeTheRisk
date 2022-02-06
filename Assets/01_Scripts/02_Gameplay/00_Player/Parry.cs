using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Parry : MonoBehaviour
{
    public UnityEvent Ev_OnSuccesParry;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy/Bullet"))
        {
            Ev_OnSuccesParry.Invoke();
        }
    }
}
