using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;


public class InputController : MonoBehaviour
{

    public static InputController current;
    
    [SerializeField] private CinemachineVirtualCamera VCam; //Ref a la camara de cinemachines
    [SerializeField] private int I_VcamDefaul; //delay de seguimiento de la camara estando quieto
    [SerializeField] private int I_VcamMove; //delay de seguimiento de la camara moviendose
    [SerializeField] private float F_Velocity; //Velocidad de player
    [SerializeField] private float F_RotationSmoothing;
    public bool B_CanMove = true;//Smoothing de rotacion del player 
    
    private Vector3 V3_Movement; //Placehodler del movimiento antes de aplicarlo
    private float F_LookAngle; //Angulo de rotación de la nave segun direccion de disparo
    private CharacterController CC_Player;
    public Rigidbody Rb_PlayerRigidBody; //Ref del riggidbody
    private Transform T_PlayerTransform; //Ref del transform

    public GameInputManager InputManager; //Declarar el sistema de input


    private void Awake()
    {
        if (current == null)
            current = this;
        else if (current != this)
            Destroy(this);

        CC_Player = GetComponent<CharacterController>();
        CC_Player.detectCollisions = false;


        Application.targetFrameRate = 60;
        InputManager = new GameInputManager();
        //Asignar referencias por si acaso y activar el movimiento del player
        Rb_PlayerRigidBody = GetComponent<Rigidbody>();
        T_PlayerTransform = GetComponent<Transform>();
        InputManager.Player.Enable();


        //Setear que el movimiento se actualize con cada cambio de input
        InputManager.Player.Movement.started += Move;
        InputManager.Player.Movement.performed += Move;
        InputManager.Player.Movement.canceled += Move;
        
        //Cambiar delay de seguimiento cuando se empieza y termina el movimiento
        InputManager.Player.Movement.started += CameraStartMove;
        InputManager.Player.Movement.canceled += CameraStopMove;

        //Setear el cambio de rotación con el input, started y canceled no aplican por el tipo de input
        InputManager.Player.Shooting.performed += Rotate;
        

    }

   

    void Move(InputAction.CallbackContext call) //Modificar y editar placeholder de movimiento
    {
        Vector2 temp = call.ReadValue<Vector2>();
        V3_Movement = new Vector3(temp.x, 0, temp.y);
        if (V3_Movement.magnitude > 1)
        {
            V3_Movement = V3_Movement.normalized;
        }

    }

    void Rotate(InputAction.CallbackContext call) //Setear angulo de rotación segun input
    {
        Vector2 temp = call.ReadValue<Vector2>();
        if (temp.magnitude > 0.1f)
        {
            F_LookAngle = Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg;
            
        }
    }

    void CameraStartMove(InputAction.CallbackContext call) //modificar delay de camara cuando se mueve
    {
        VCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = I_VcamMove;
        VCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = I_VcamMove;
    }

    void CameraStopMove(InputAction.CallbackContext call) //modificar delay de camara cuando se esta quieto 
    {
        VCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_XDamping = I_VcamDefaul;
        VCam.GetCinemachineComponent<CinemachineFramingTransposer>().m_YDamping = I_VcamDefaul;
    }

    private void LateUpdate() //Actualizar rotación
    {
        T_PlayerTransform.rotation = Quaternion.Lerp(T_PlayerTransform.rotation,Quaternion.Euler(0, -F_LookAngle, 0), Time.deltaTime * F_RotationSmoothing);
        if (B_CanMove)
            CC_Player.SimpleMove(V3_Movement * F_Velocity);
    }

    private void FixedUpdate() //Actualizar movimiento
    {
        //Rb_PlayerRigidBody.MovePosition(Rb_PlayerRigidBody.position + (V3_Movement * F_Velocity * Time.fixedDeltaTime));
    }
}
