
using ArthemyDevelopment.Save;
using UnityEngine;


public class GameData : ISaveClass
{
    public Vector2 PlayerPosition;
    
    public int HealsItems;
    public int ActHealth;
    public int MaxHealth;
    public int DamageScaleUpgrade;
    public int MaxHealthUpgrade;
    public int HealtItemsUpgrade;
    public int TotalLv;
    
    public string Date;

    public Level ActLevel;
}
