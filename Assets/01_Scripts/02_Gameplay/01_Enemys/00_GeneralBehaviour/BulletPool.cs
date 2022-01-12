using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool current;

    public GameObject G_BulletPrefab;
    public Queue<GameObject> Q_EnemyBullets = new Queue<GameObject>();


    private void Awake()
    {
        if (current == null)
            current = this;
        else if(current != this)
            Destroy(this);
    }


    public GameObject GetBullet() //Solicitar una bala a la pool
    {
        if (Q_EnemyBullets.Count != 0)
        {
            GameObject temp = Q_EnemyBullets.Dequeue();
            return temp;
        }
        else
        {
            GameObject temp = Instantiate(G_BulletPrefab);
            temp.SetActive(false);
            return temp;
        }
    }

    public void StoreBullet(GameObject b) //Regresar una bala a la pool
    {
        b.SetActive(false);
        Q_EnemyBullets.Enqueue(b);
    }
}
