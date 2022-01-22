using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private Slider Sl_HealthBar;
    [SerializeField] private int I_MaxHealth;
    [SerializeField] private int I_ActHealth;
    [SerializeField] private GameObject G_EnemyParent;
    [SerializeField] private int I_PointsToGive;


    private void OnEnable()
    {
        Sl_HealthBar.maxValue = I_MaxHealth;
        Sl_HealthBar.value = I_MaxHealth;
        I_ActHealth = I_MaxHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player/Bullet"))
        {
            ApplyDamage();
            PlayerManager.current.StoreBullet(other.gameObject);
        }
    }

    void ApplyDamage()
    {
        I_ActHealth -= PlayerManager.current.I_ActDamage;
        Sl_HealthBar.value = I_ActHealth;
        if (I_ActHealth <= 0)
        {
            Death();
        }

    }

    void Death()
    {
        //TODO:Destroyed effect
        UpgradesSystem.current.GetPoints(I_PointsToGive);
        G_EnemyParent.SetActive(false);
    }
}
