using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Weapon")]
public class Weapon : ScriptableObject
{
    public int I_Damage;
    public float F_Velocity;
    public float F_Size;
    [PreviewField]public Mesh Ms_WeaponModel;
    [PreviewField]public Sprite Sp_WeaponImage;
    
}
