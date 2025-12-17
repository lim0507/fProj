using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public bool inputLocked = false;

    [Header("Camera")]
    public Transform cameraTransform;
    public float mouseSensitivity = 200f;

    float xRotation = 0f;
    Vector3 velocity;

    CharacterController controller;
    public CraftingPanel craftingPanel;

    // 추가
    public float groundCheckDistance = 0.2f;
    public LayerMask groundMask;
    bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (craftingPanel != null && craftingPanel.IsOpen)
            return;

        if (inputLocked)
            return;

        GroundCheck();
        LookAround();
        Move();

        
        if (Input.GetMouseButtonDown(0))
        {
            GetComponent<WeaponManager>().Attack();
        }
    }

    void GroundCheck()
    {
        // 플레이어 중심 아래 방향으로 레이캐스트
        isGrounded = Physics.Raycast(transform.position, Vector3.down,
                                     controller.height / 2 + groundCheckDistance,
                                     groundMask);

        // 땅에 닿으면 y속도 초기화
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // 수정됨: controller.isGrounded 대신 isGrounded 사용
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}

