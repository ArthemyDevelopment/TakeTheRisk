using System;
using UnityEngine;

public class PlayerBullets : MonoBehaviour
{
    private Vector3 V3_StartPos;
    private Vector3 V3_ActPos;
    public float F_MaxDist;
    public float F_Velocity;

    private void OnEnable()
    {
        V3_StartPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * (F_Velocity * Time.deltaTime);
        V3_ActPos = transform.position;
        float dist = Vector3.Distance(V3_ActPos, V3_StartPos);
        if(dist>F_MaxDist)
            PlayerManager.current.StoreBullet(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("World/Wall"))
        {
            PlayerManager.current.StoreBullet(this.gameObject);
        }
    }
}
