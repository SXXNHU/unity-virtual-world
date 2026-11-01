using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float mouseSensitivity = 2f;
    public float verticalLookLimit = 80f;
    
    public Transform cameraTransform;

    private Animator animator;
    private CharacterController controller;
    private float verticalRotation = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); 
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        LookAround();
    }

    void Move()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float moveY = 0f;

        Vector3 move = transform.right * moveX + transform.forward * moveZ + transform.up * moveY;
        controller.Move(move * moveSpeed * Time.deltaTime);

        float horizontalSpeed = new Vector2(moveX, moveZ).magnitude;
        if (animator != null)
        {
            animator.SetFloat("Speed_f", horizontalSpeed); 
        }
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalLookLimit, verticalLookLimit);

        cameraTransform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }
}
