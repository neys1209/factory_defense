using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JestCameraMove : MonoBehaviour
{
    public float cameraRotSpeed = 1;
    public float cameraZoomSpeed = 5;
    public float playerSpeed = 5;

    Vector3 CameraRot = Vector3.zero;
    Vector3 SmoothCameraRot = Vector3.zero;
    Vector3 playerMovement = Vector3.zero;

    float cameraZoom = 0;
    Vector3 cameraOffset = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //camera rotate
        if (Input.GetMouseButton(2))
        {
            if (Input.GetAxis("Mouse X") != Mathf.Epsilon)
            {
                CameraRot.y += Input.GetAxis("Mouse X") * cameraRotSpeed;
            }
            if (Input.GetAxis("Mouse Y") != Mathf.Epsilon)
            {
                CameraRot.x += -Input.GetAxis("Mouse Y") * cameraRotSpeed;
                CameraRot.x = Mathf.Clamp(CameraRot.x, -50.0f, 80.0f);
            }
        }
        SmoothCameraRot = Vector3.Slerp(SmoothCameraRot, CameraRot, Time.deltaTime * 10.0f);
        transform.rotation = Quaternion.Euler(SmoothCameraRot);

        //camera zoom
        if (Input.GetAxis("Mouse ScrollWheel") != Mathf.Epsilon)
        {
            cameraZoom += Input.GetAxis("Mouse ScrollWheel") * cameraZoomSpeed;
            cameraZoom = Mathf.Clamp(cameraZoom, 0, 50);
        }

        cameraOffset.z = Mathf.Lerp(cameraOffset.z, cameraZoom, Time.deltaTime * 10);
        Camera.main.transform.localPosition = cameraOffset;

        //player movement
        float delta = playerSpeed * Time.deltaTime;
        if (Input.GetAxis("Horizontal") != 0.0f)
        {
            transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * delta);
        }
        if (Input.GetAxis("Vertical") != 0.0f)
        {
            transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * delta);
        }
        playerMovement.x = transform.position.x;
        playerMovement.z = transform.position.z;
        transform.position = playerMovement;
    }
}
