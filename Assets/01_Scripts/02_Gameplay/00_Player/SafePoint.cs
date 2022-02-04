using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SafePoint : MonoBehaviour
{
    [SerializeField] private Transform T_CenterPoint;
    [SerializeField] private float F_ConectSpeed;
    [SerializeField] private bool B_InSafeSpace = false;
    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player/Collider"))
        {
            EnterSafePoint();
        }
    }

    private void OnEnable()
    {
        InputController.current.InputManager.UI.Cancel.started += LeaveSafePoint;
        InputController.current.InputManager.UI.SafepointMenu.started += LeaveSafePoint;
    }

    void OnDisable()
    {
        InputController.current.InputManager.UI.Cancel.started -= LeaveSafePoint;
        InputController.current.InputManager.UI.SafepointMenu.started -= LeaveSafePoint;
    }


    private void Update() //Movimiento automatico hacia el centro del safepoint
    {
        if (B_InSafeSpace)
        {
            Vector3 temp = Vector3.MoveTowards(PlayerManager.current.transform.position, T_CenterPoint.position, F_ConectSpeed * Time.deltaTime);
            InputController.current.Rb_PlayerRigidBody.MovePosition(temp);
            if (Vector3.Distance(PlayerManager.current.transform.position, T_CenterPoint.position) < 0.3f)
                B_InSafeSpace = false;
        }
    }

    void LeaveSafePoint(InputAction.CallbackContext callbackContext) //Funcionamiento para salir de un safepoint
    {
        InputController.current.B_CanMove = true;
        PlayerManager.current.B_CanShoot = true;
        UpgradesSystem.current.B_CanUpgrade = false;
    }


    void EnterSafePoint() //Funcionamiento al entrar a un safepoint, se pierde el control del player, se permite mejorar y se abre automaticamente el menu
    {
        InputController.current.B_CanMove = false;
        PlayerManager.current.B_CanShoot = false;
        B_InSafeSpace = true;
        PlayerManager.current.ResetHealth();
        UpgradesSystem.current.B_CanUpgrade = true;
        PlayerRespawnManager.current.G_LastSafePoint = gameObject;
        StartCoroutine(OpenUpgradeMenu(0.5f));
    }

    IEnumerator OpenUpgradeMenu(float t)//corutina middlepoint para anadir delay a la apertura de menu
    {
        yield return new WaitForSeconds(t);
        UpgradesSystem.current.OpenUpgradeMenu();
    }
}
