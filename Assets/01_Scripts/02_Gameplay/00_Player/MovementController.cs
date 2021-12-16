using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;


public class MovementController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera VCam;
    [SerializeField] private int I_VcamDefaul;
    [SerializeField] private int I_VcamMove;
    [SerializeField] private float F_Velocity;
    [SerializeField] private float F_RotationSmoothing;
    private Vector3 V3_Movement;
    private float F_LookAngle;
    private Rigidbody Rb_PlayerRigidBody;
    private Transform T_PlayerTransform;
    
    
    

    private GameInputManager InputManager;


    private void Awake()
    {
        Application.targetFrameRate = 60;
        InputManager = new GameInputManager();
        Rb_PlayerRigidBody = GetComponent<Rigidbody>();
        T_PlayerTransform = GetComponent<Transform>();
        InputManager.Player.Enable();


        InputManager.Player.Movement.started += Move;
        InputManager.Player.Movement.started += CameraStartMove;
        InputManager.Player.Movement.performed += Move;
        InputManager.Player.Movement.canceled += Move;
        InputManager.Player.Movement.canceled += CameraStopMove;

        //InputManager.Player.Shooting.started += Rotate;
        InputManager.Player.Shooting.performed += Rotate;
        //InputManager.Player.Shooting.canceled += Rotate;
    }

    void Move(InputAction.CallbackContext call)
    {
        Vector2 temp = call.ReadValue<Vector2>();
        V3_Movement = new Vector3(temp.x, 0, temp.y);
        if (V3_Movement.magnitude > 1)
        {
            V3_Movement = V3_Movement.normalized;
        }

    }

    void Rotate(InputAction.CallbackContext call)
    {
        Vector2 temp = call.ReadValue<Vector2>();
        if (temp.magnitude > 0.1f)
        {
            F_LookAngle = Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg;
            
        }
    }

    void CameraStartMove(InputAction.CallbackContext call)
    {
        VCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = I_VcamMove;
        VCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = I_VcamMove;
    }

    void CameraStopMove(InputAction.CallbackContext call)
    {
        VCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = I_VcamDefaul;
        VCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = I_VcamDefaul;
    }

    private void LateUpdate()
    {
        var obRot = Quaternion.Euler(0, -F_LookAngle, 0);
        T_PlayerTransform.rotation = Quaternion.Lerp(T_PlayerTransform.rotation,obRot, Time.deltaTime * F_RotationSmoothing);
    }

    private void FixedUpdate()
    {
        Rb_PlayerRigidBody.MovePosition(Rb_PlayerRigidBody.position + (V3_Movement * F_Velocity * Time.fixedDeltaTime));
    }
}
