using System;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public int I_Damage;
    private Vector3 V3_StartPos;
    private float F_Dist;
    public float F_MaxDist;
    public float F_Vel;


    private void OnEnable()
    {
        V3_StartPos = transform.position;
        F_Dist = 0;
    }

    private void Update()
    {
        transform.position += transform.right * (F_Vel * Time.deltaTime);
        F_Dist = Vector3.Distance(transform.position, V3_StartPos);
        if (F_Dist > F_MaxDist)
            BulletPool.current.StoreBullet(this.gameObject);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("World/Wall"))
        {
            BulletPool.current.StoreBullet(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        Debug.Log("Destroyed Bullet");
    }
}
