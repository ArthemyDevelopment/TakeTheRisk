using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDoor : MonoBehaviour
{
    [SerializeField]private bool B_isOpen = false;
    [SerializeField] private GameObject NewRoom;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player/Collider"))
        {
            if (!B_isOpen)
            {
                B_isOpen = true;
                CombineMeshRooms.current.CombineMeshs(NewRoom);
            }
        }
    }
}
