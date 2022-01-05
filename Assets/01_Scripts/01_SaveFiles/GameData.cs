
using ArthemyDevelopment.Save;
using UnityEngine;


public class GameData : ISaveClass
{
    
    public Vector3 PlayerPosition;
    
    
    //----------Stats--------------//
    public int MaxHealth;
    public int ActHealth;
    public int MaxHeals;
    public int ActHeals;
    public int Healling;
    public int SelfDmg;
    public int BaseDmg;
    public float DmgScale;
    public float ParryDuration;
    public float ParryCD;
    
    //-----Upgrades------//
    public int ActPoints;
    public int ActCost;
    public int ActLvMaxHealth;
    public int ActLvHeals;
    public int ActLvHealling;
    public int ActLvSelfDmg;
    public int ActLvBaseDmg;
    public int ActLvDmgScale;
    public int ActLvParry;
    public int ActLvParryCD;

    public Weapons ActWeapon;
    
    
    public int TotalLv;
    
    public string Date;

    public Level ActLevel;
}
