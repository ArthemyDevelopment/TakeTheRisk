using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponInventory : MonoBehaviour
{
    public static WeaponInventory current;
    
    [SerializeField]private Button[] B_WeaponsButtons;
    [SerializeField] private Image Im_ActualWeapon;
    [SerializeField] private TMP_Text Tx_Damage;
    [SerializeField] private TMP_Text Tx_Speed;
    [SerializeField] private TMP_Text Tx_Size;


    private void Awake()
    {
        if (current == null)
            current = this;
        else if(current != this)
            Destroy(this);
    }

    private void OnEnable()
    {
        UpdateButtons();
        GetActWeapon();
    }

    public void UpdateButtons()
    {
        for (int i = 0; i < B_WeaponsButtons.Length; i++)
        {
            if (PlayerManager.current.Wp_ObtainedWeaponsSO[i] != null)
            {
                Debug.Log(i);
                Weapon refwe = PlayerManager.current.Wp_ObtainedWeaponsSO[i];
                Button temp = B_WeaponsButtons[i];
                temp.interactable = true;
                temp.GetComponent<Image>().sprite = refwe.Sp_WeaponImage;
                temp.GetComponent<Image>().preserveAspect = true;
            }
            else
            {
                DeactiveButton(i);
            }
        }
    }

    void GetActWeapon()
    {
        switch (PlayerManager.current.Wp_ActWeapon)
        {
            case Weapons.none:

                break;
            case Weapons.starter:
                ChangeWeapon(0);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    void DeactiveButton(int id)
    {
        Button temp = B_WeaponsButtons[id];
        temp.interactable = false;
        //temp.GetComponent<Image>().enabled = false;
        
    }

    public void ChangeWeapon(int id)
    {
        UpdateButtons();
        DeactiveButton(id);
        PlayerManager.current.ChangeWeapon(id);
        Weapon refwe = PlayerManager.current.Wp_ObtainedWeaponsSO[id];
        Tx_Damage.text = refwe.I_Damage.ToString();
        Tx_Speed.text = refwe.F_Velocity.ToString("F2",CultureInfo.InvariantCulture);
        Tx_Size.text = refwe.F_Size.ToString("F2", CultureInfo.InvariantCulture);
    }
}
