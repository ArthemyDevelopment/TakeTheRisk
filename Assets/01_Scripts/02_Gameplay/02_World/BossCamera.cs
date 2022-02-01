using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera Vcam_Boss;
    //BossController

    private void Awake()
    {
        Vcam_Boss.gameObject.SetActive(false);
        //Add DeactivateCamera to BossDeath
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player/Collider"))
        {
            Vcam_Boss.m_Follow = PlayerManager.current.T_PlayerModel;
            PlayerManager.current.Ev_OnPlayerDeath.AddListener(DeactivateCamera);
            Vcam_Boss.gameObject.SetActive(true);
        }
    }

    void DeactivateCamera()
    {
        Vcam_Boss.gameObject.SetActive(false);
        PlayerManager.current.Ev_OnPlayerDeath.RemoveListener(DeactivateCamera);
    }
}
