using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]private Image Im_LifeBar;
    [SerializeField] private Image Im_DelayBar;
    [SerializeField] private float F_AmountDelayBar;
    [SerializeField] private float F_DurationDelayBar;
    [SerializeField] private int I_MaxHealth;
    [SerializeField] private int I_ActHealth;
    [SerializeField] private GameObject G_EnemyParent;
    [SerializeField] private int I_PointsToGive;


    private void OnEnable()
    {
        Im_LifeBar.fillAmount = 1;
        Im_DelayBar.fillAmount = 1;
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
        Im_LifeBar.fillAmount = ScriptsTools.MapValues(I_ActHealth, 0, I_MaxHealth, 0, 1);
        StartCoroutine(DelayBar());
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

    IEnumerator DelayBar()
    {
        yield return ScriptsTools.GetWait(0.2f);
        while (Im_LifeBar.fillAmount < Im_DelayBar.fillAmount)
        {
            Im_DelayBar.fillAmount -= F_AmountDelayBar;
            yield return ScriptsTools.GetWait(F_DurationDelayBar);
        }

        if (Im_DelayBar.fillAmount < Im_LifeBar.fillAmount)
            Im_DelayBar.fillAmount = Im_LifeBar.fillAmount;
    }
    

}
