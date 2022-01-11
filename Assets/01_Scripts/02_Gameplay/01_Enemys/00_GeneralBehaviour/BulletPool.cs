using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool current;

    public GameObject G_BulletPrefab;
    public Queue<GameObject> Q_EnemyBullets;


    private void Awake()
    {
        if (current == null)
            current = this;
        else if(current != this)
            Destroy(this);
    }


    public GameObject GetBullet()
    {
        if (Q_EnemyBullets.Count != 0)
        {
            GameObject temp = Q_EnemyBullets.Dequeue();
            temp.SetActive(true);
            return temp;
        }
        else
        {
            GameObject temp = Instantiate(G_BulletPrefab);
            return temp;
        }
    }

    public void StoreBullet(GameObject b)
    {
        b.SetActive(false);
        Q_EnemyBullets.Enqueue(b);
    }
}
