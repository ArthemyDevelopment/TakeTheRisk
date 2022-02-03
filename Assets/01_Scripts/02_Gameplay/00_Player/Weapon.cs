using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Weapon")]
public class Weapon : ScriptableObject
{
    public int I_WeaponDamage;
    [PreviewField]public Mesh Ms_WeaponModel;
    [PreviewField]public Sprite Sp_WeaponImage;
    
}
