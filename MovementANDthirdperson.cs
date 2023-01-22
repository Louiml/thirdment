using UnityEngine;

public class MovementANDthirdperson : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float sprintSpeed = 10.0f;
    public float jumpForce = 5.0f;
    public float gravity = 9.81f;
    public float cameraSensitivity = 2.0f;
    public Transform cameraTransform;
    public float cameraDistance = 5.0f;
    public float cameraHeight = 2.0f;
    private bool isJumping = false;
    private bool isSprinting = false;
    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 desiredMoveDirection = forward * vertical + right * horizontal;
        moveDirection.x = desiredMoveDirection.x;
        moveDirection.z = desiredMoveDirection.z;

        if (Input.GetButtonDown("Jump") && !isJumping)
        {
            isJumping = true;
            moveDirection.y = jumpForce;
        }
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }

        if (isSprinting)
        {
            moveDirection *= sprintSpeed;
        }
        else
        {
            moveDirection *= moveSpeed;
        }

        moveDirection.y -= gravity * Time.deltaTime;
        CharacterController controller = GetComponent<CharacterController>();
        controller.Move(moveDirection * Time.deltaTime);

        if (controller.isGrounded)
        {
            isJumping = false;
        }

        rotationX += Input.GetAxis("Mouse X") * cameraSensitivity;
        rotationY -= Input.GetAxis("Mouse Y") * cameraSensitivity;
        rotationY = Mathf.Clamp(rotationY, -90.0f, 90.0f);

        cameraTransform.localRotation = Quaternion.Euler(rotationY, rotationX, 0);
        transform.rotation = Quaternion.Euler(0, rotationX, 0);
        cameraTransform.position = transform.position - cameraDistance * cameraTransform.forward + cameraHeight * Vector3.up;
    }
}
