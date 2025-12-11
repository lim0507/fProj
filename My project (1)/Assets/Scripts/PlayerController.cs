using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float gravity = -9.81f;

    [Header("Camera")]
    public Transform cameraTransform;
    public float mouseSensitivity = 200f;

    float xRotation = 0f;       // 카메라 상하 회전 값
    Vector3 velocity;           // 중력/점프용
    CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        // 마우스 고정
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        LookAround();
        Move();
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 카메라 상하 회전
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // 플레이어 좌우 회전
        transform.Rotate(Vector3.up * mouseX);
    }

    void Move()
    {
        // 플레이어가 땅에 닿았는지 체크
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // 땅에 붙게 하는 값
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * moveSpeed * Time.deltaTime);

        // 점프
        if (controller.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        // 중력 적용
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
