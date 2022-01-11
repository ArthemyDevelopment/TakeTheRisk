using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int I_Damage;
    private Vector3 V3_StartPos;
    
    [SerializeField] private float F_MaxDist;
    [SerializeField] private float F_Vel;


    private void OnEnable()
    {
        V3_StartPos = transform.position;
    }

    private void Update()
    {
        transform.position += transform.right * (F_Vel * Time.deltaTime);
        float dist = Vector3.Distance(transform.position, V3_StartPos);
        if (dist > F_MaxDist)
            BulletPool.current.StoreBullet(this.gameObject);
    }
}
