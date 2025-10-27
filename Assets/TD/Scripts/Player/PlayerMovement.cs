
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerMovement : MonoBehaviour
{
    private Vector3 Velocity;
    private Vector3 PlayerMovementInput;
    private Vector2 PlayerMouseInput;
    private float xRotation;
    private float yRotation;
    [SerializeField] private Transform PlayerCamera;
    [SerializeField] CharacterController Controller;
    [SerializeField] float speed;
    [SerializeField] float rotationSpeed;
    

    public static bool moveOn;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        moveOn = true;
    }

    void Update()
    {
        if (moveOn)
        {
            PlayerMovementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            PlayerMouseInput = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
            Move();            
            Rotate();
        }
    }

    private void Move()
    {
        Vector3 movement = transform.TransformDirection(PlayerMovementInput);

        if (Input.GetKey(KeyCode.E))
        {
            Velocity.y = 1f;
        }
        else if (Input.GetKey(KeyCode.Q))
        {
            Velocity.y = -1f;
        }

        if (Input.GetKey(KeyCode.LeftShift))
            speed = 20;
        else
            speed = 10;


        Controller.Move((movement) * (speed * Time.deltaTime));
        Controller.Move((Velocity) * (speed * Time.deltaTime));

        Velocity.y = 0f;
    }

    private void Rotate()
    {
        yRotation += PlayerMouseInput.x * rotationSpeed;
        xRotation -= PlayerMouseInput.y * rotationSpeed;
        xRotation = Mathf.Clamp(xRotation, -10f, 60f);

        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);
        PlayerCamera.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }


    public static void TurnOnPlayerMovement()
    {
        moveOn = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public static void TurnOffPlayerMovement()
    {
        moveOn = false;
        Cursor.lockState = CursorLockMode.None;
    }
}
