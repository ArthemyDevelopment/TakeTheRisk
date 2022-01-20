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

    private void Start()
    {
        InputController.current.InputManager.UI.Cancel.performed += LeaveSafePoint;
        InputController.current.InputManager.UI.SafepointMenu.performed += LeaveSafePoint;
    }


    private void Update()
    {
        if (B_InSafeSpace)
        {
            Vector3 temp = Vector3.MoveTowards(PlayerManager.current.transform.position, T_CenterPoint.position, F_ConectSpeed * Time.deltaTime);
            InputController.current.Rb_PlayerRigidBody.MovePosition(temp);
            if (Vector3.Distance(PlayerManager.current.transform.position, T_CenterPoint.position) < 0.3f)
                B_InSafeSpace = false;
        }
    }

    void LeaveSafePoint(InputAction.CallbackContext callbackContext)
    {
        InputController.current.B_CanMove = true;
        PlayerManager.current.B_CanShoot = true;
        StartCoroutine(OpenUpgradeMenu(0));
    }


    void EnterSafePoint()
    {
        InputController.current.B_CanMove = false;
        PlayerManager.current.B_CanShoot = false;
        B_InSafeSpace = true;
        
        StartCoroutine(OpenUpgradeMenu(0.5f));
    }

    IEnumerator OpenUpgradeMenu(float t)
    {
        yield return new WaitForSeconds(t);
        UpgradesSystem.current.OpenUpgradeMenu();
    }
}
