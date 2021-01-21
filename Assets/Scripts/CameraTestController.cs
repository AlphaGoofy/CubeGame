using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTestController : MonoBehaviour
{

    public Vector3 moveDirection;

    public float cameraSpeed = 2.5f;

    void Start()
    {
        
    }

    void Update()
    {
        moveDirection = Vector3.zero;
        /*moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        moveDirection.Normalize();*/

        transform.parent.Rotate(0, Input.GetAxis("Mouse X") * cameraSpeed, 0);
        transform.Rotate(-Input.GetAxis("Mouse Y") * cameraSpeed, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector3.right;
        }

        if(Input.GetKey(KeyCode.Q))
        {
            moveDirection += Vector3.up;
        }
        if (Input.GetKey(KeyCode.E))
        {
            moveDirection += Vector3.down;
        }

        moveDirection.Normalize();

        transform.parent.Translate(moveDirection * (Input.GetKey(KeyCode.LeftShift) ? 12.0f : 6.0f) * Time.deltaTime);

    }
}
