using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObtention : MonoBehaviour
{
    [SerializeField] private Weapons Wp_weapon;
    [SerializeField] private Weapon Wp_WeaponToObtain;
    [SerializeField] private bool B_isObtained;
    [SerializeField] private int id;
    
#if UNITY_EDITOR
    [SerializeField] private bool B_isDebugging;

    private void Awake()
    {
        if(B_isDebugging && PlayerPrefs.HasKey("Weapon" + id))
        {
            PlayerPrefs.DeleteKey("Weapon"+id);
        }
    }
#endif
    private void OnEnable()
    {
        if (PlayerPrefs.HasKey("Weapon" + id))
        {
            B_isObtained = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player/Collider"))
        {
            if (!B_isObtained)
            {
                B_isObtained = true;
                PlayerPrefs.SetInt("Weapon" + id, 1);
                PlayerManager.current.Wp_ObtainedWeaponsSO.Add(Wp_WeaponToObtain);
                PlayerManager.current.Wp_ObtainedWeapons.Add(Wp_weapon);
            }
        }
    }
}
