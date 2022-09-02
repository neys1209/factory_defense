using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public Camera playerCamera;

    public float cameraRotSpeed = 1;
    public float playerSpeed = 5;



    Vector3 CameraRot = Vector3.zero;
    Vector3 SmoothCameraRot = Vector3.zero;
    Vector3 PlayerMovement = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        //player movement
        if (Input.GetMouseButton(1))
        {
            if (Input.GetAxis("Mouse X") != Mathf.Epsilon)
            {
                CameraRot.y += Input.GetAxis("Mouse X") * cameraRotSpeed;
            }
            if (Input.GetAxis("Mouse Y") != Mathf.Epsilon)
            {
                CameraRot.x += -Input.GetAxis("Mouse Y") * cameraRotSpeed;
            }
        }
        SmoothCameraRot = Vector3.Slerp(SmoothCameraRot, CameraRot, Time.deltaTime * 10.0f);
        transform.rotation = Quaternion.Euler(SmoothCameraRot);

        
        if (  Input.GetAxis("Horizontal") != 0.0f)
        {
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime);
        }
        if (Input.GetAxis("Vertical") != 0.0f)
        {
            transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * playerSpeed * Time.deltaTime);
        }

        
    }
}
