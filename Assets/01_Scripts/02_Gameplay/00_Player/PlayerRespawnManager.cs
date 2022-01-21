using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawnManager : MonoBehaviour
{
    public static PlayerRespawnManager current;
    
    public GameObject G_LastSafePoint; //El ultimo safe point que se uso
    [SerializeField] private Animator An_LoadCurtain; //Cortina de carga al respawnear
    public ZoneGameManager ZGM_ActZone; //Manager que controla cada zona en especifico
    

    private void Awake()
    {
        if (current == null)
            current = this;
        else if(current!=this)
            Destroy(this);
    }

    public IEnumerator OnPlayerDeath() //Corutina que controla la cortina de pausa y el reseteo al respawnear
    {
        //TODO:SetCurtaionAnimationON
        yield return new WaitForSeconds(5f);
        PlayerManager.current.ResetPlayer(G_LastSafePoint.transform);
        ZGM_ActZone.ResetEnemies();
        yield return new WaitForSeconds(0.3f);
        //CurtainOFF
    }
}