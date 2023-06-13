using Sirenix.OdinInspector;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [TabGroup("Options")][SerializeField] private float movementSpeed;
    [TabGroup("Options")][SerializeField] private float rotationSpeed;

    [TabGroup("References")] public FloatingJoystick joystick;

    [FoldoutGroup("Debug")][SerializeField] private bool canMove;

    private Vector3 moveDirection;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovement();
        UpdateAnimations();
    }

    private void HandleMovement()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();

        canMove = moveDirection.magnitude != 0 ? true : false;

        playerInput.horizontal = Input.GetAxisRaw("Horizontal");
        playerInput.vertical = Input.GetAxisRaw("Vertical");

        if (playerInput.inputType == InputType.Keyboard)
        {
            moveDirection = new Vector3(playerInput.horizontal, 0, playerInput.vertical);
            joystick.gameObject.SetActive(false);
        }
        else if (playerInput.inputType == InputType.Joystick)
        {
            moveDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
            joystick.gameObject.SetActive(true);
        }

        moveDirection.Normalize();

        transform.Translate(moveDirection * movementSpeed * Time.deltaTime, Space.World);

        HandleRotation();
    }

    private void HandleRotation()
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

        //if (moveDirection != Vector3.zero)
        //{
        //    float angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        //    Quaternion rotation = Quaternion.Euler(0f, angle, 0f);
        //
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        //}
    }

    private void UpdateAnimations()
    {
        anim.SetFloat("Speed", moveDirection.magnitude);
    }
}
