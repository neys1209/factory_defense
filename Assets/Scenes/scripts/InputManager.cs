using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public Transform player;
    public Camera playerCamera;

    public float cameraRotSpeed = 1;
    public float playerSpeed = 5;



    new Vector3 CameraRot = Vector3.zero;
    new Vector3 SmoothCameraRot = Vector3.zero;

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
        SmoothCameraRot = Vector3.Slerp(SmoothCameraRot, CameraRot, Time.deltaTime * 10);
        player.transform.rotation = Quaternion.Euler(SmoothCameraRot);

        
        if (Input.GetKey(KeyCode.W))
        {
            player.transform.Translate(Vector3.forward * playerSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            player.transform.Translate(Vector3.forward * -playerSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            player.transform.Translate(Vector3.left * playerSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            player.transform.Translate(Vector3.right * playerSpeed * Time.deltaTime);
        }


        


    }
}
