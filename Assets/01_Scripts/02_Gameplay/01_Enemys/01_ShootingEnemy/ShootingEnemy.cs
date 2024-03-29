using System;
using System.Collections;
using Unity.Collections;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [ReadOnly, SerializeField]private Transform T_Target;
    [ReadOnly, SerializeField]private bool B_isShooting;
    [ReadOnly, SerializeField]private bool B_canShoot = true;
    [SerializeField]private int I_Damage;
    [SerializeField]private float F_BulletSpeed;
    [SerializeField]private float F_FireRate;
    [SerializeField]private float I_MaxDistance;


    private void OnEnable()
    {
        B_canShoot = true;
        B_isShooting = false;
        T_Target = PlayerManager.current.transform;
    }

    private void Update()//Disparar cuando se cumplan las condiciones
    {
        if (B_canShoot && B_isShooting)
        {
            B_canShoot = false;
            Shoot();
            StartCoroutine(ShootingRate());
        }
    }

    public void UpdateShooting(bool b) //Actualizar si se esta en rango de disparar o no
    {
        B_isShooting = b;
    }

    void Shoot() //Disparar una bala y modificar los valores
    {
        GameObject temp = BulletPool.current.GetBullet();
        EnemyBullet EB = temp.GetComponent<EnemyBullet>();
        EB.I_Damage = I_Damage;
        if(F_BulletSpeed != 0)
            EB.F_Vel = F_BulletSpeed;
        if(I_MaxDistance !=0)
            EB.F_MaxDist = I_MaxDistance;
        //float angle = Mathf.Atan2(T_Target.position.z - transform.position.z, T_Target.position.x - transform.position.x) * Mathf.Rad2Deg;
        temp.transform.rotation = transform.rotation;
        temp.transform.position = transform.position;
        temp.SetActive(true);
    }

    IEnumerator ShootingRate() //Delay de disparar
    {
        yield return new WaitForSeconds(F_FireRate);
        B_canShoot = true;
    }
}
