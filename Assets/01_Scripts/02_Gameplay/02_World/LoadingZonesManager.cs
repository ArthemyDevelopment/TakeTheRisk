using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingZonesManager : MonoBehaviour
{
    public static LoadingZonesManager current;


    public LoadingZonesScenes ActZone;
    public LoadingZonesScenes PrevZone;
    
    private void Awake()
    {
        if (current == null)
            current = this;
        else if(current != this)
            Destroy(this);
    }

    public void SetZone(LoadingZonesScenes s)
    {
        PrevZone = ActZone;
        ActZone = s;
    }
}
