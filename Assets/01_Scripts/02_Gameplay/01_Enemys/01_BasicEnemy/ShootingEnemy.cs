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


    private void OnEnable()
    {
        B_canShoot = true;
        B_isShooting = false;
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
        temp.GetComponent<EnemyBullet>().I_Damage = I_Damage;
        temp.GetComponent<EnemyBullet>().F_Vel = F_BulletSpeed;
        float angle = Mathf.Atan2(T_Target.position.z - transform.position.z, T_Target.position.x - transform.position.x) * Mathf.Rad2Deg;
        temp.transform.rotation = Quaternion.Euler(0, -angle, 0);
        temp.transform.position = transform.position;
        temp.SetActive(true);
    }

    IEnumerator ShootingRate() //Delay de disparar
    {
        yield return new WaitForSeconds(F_FireRate);
        B_canShoot = true;
    }
}
