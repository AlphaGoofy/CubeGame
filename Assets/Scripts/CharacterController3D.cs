using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class CharacterController3D : MonoBehaviour
{
    public float movementSpeed = 2.0f;
    public float sprintMultiplier = 1.5f;
    public float gravity = Physics.gravity.y;
    public float jumpSpeed = 3.0f;
    public float cameraSpeed = 5.0f;

    private float currentSpeed;

    public float airSpeed = 1.5f;

    private Vector3 moveDirection;

    private CharacterController controller;

    public Camera playerCamera;

    private bool freezed = false, falling = false;

    private Vector3 lastGroundPosition;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        playerCamera = Camera.main;
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            if (falling)
            {
                float distance = lastGroundPosition.y - transform.position.y;

                if (distance < 0)
                    distance *= (-1.0f);

                if (distance > 5.0f)
                {
                    Debug.Log("Fall distance: " + distance + "    (dist < 5 is ignored)");
                }

                falling = false;
            }

            lastGroundPosition = transform.position;
        }
        else
        {
            if (!falling)
                falling = true;

        }

        if (Input.GetButton("Sprint"))
        {
            currentSpeed = movementSpeed * sprintMultiplier;
        }
        else
        {
            currentSpeed = movementSpeed;
        }

        if (!(freezed))
        {
            if (controller.isGrounded)
            {
                moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
                moveDirection = transform.TransformDirection(moveDirection);
                moveDirection.Normalize();
                moveDirection *= currentSpeed;

                if (Input.GetButtonDown("Jump"))
                    moveDirection.y = jumpSpeed;

            }
            else
            {
                moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), moveDirection.y, Input.GetAxisRaw("Vertical"));

                float prevMoveDirectionY = moveDirection.y;

                moveDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

                moveDirection.Normalize();

                moveDirection.y = prevMoveDirectionY;

                moveDirection = transform.TransformDirection(moveDirection);

                moveDirection.x *= airSpeed;
                moveDirection.z *= airSpeed;
            }
        }

        moveDirection.y += gravity * Time.deltaTime;

        if (!freezed)
        {
            controller.Move(moveDirection * Time.deltaTime);
        }
        else
        {
            float prevMoveDirectionY = moveDirection.y;

            moveDirection = Vector3.zero;

            moveDirection.y = prevMoveDirectionY;

            moveDirection = transform.TransformDirection(moveDirection);

            moveDirection.y += gravity * Time.deltaTime;

            controller.Move(moveDirection * Time.deltaTime);

        }

        if (!freezed)
        {
            //Spieler auf der X-Achse rotieren
            transform.Rotate(0, Input.GetAxis("Mouse X") * cameraSpeed, 0);

            playerCamera.transform.Rotate(new Vector3(-Input.GetAxis("Mouse Y") * cameraSpeed, 0, 0));//Immer gleich
        }
    }


    public void FreezePlayer(bool b)
    {
        freezed = b;
    }

    public bool IsFreezed()
    {
        return freezed;
    }    
}
