using UnityEngine;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{

    public GameObject dragLine = null;
    LineRenderer lineRanderer;

    public float cameraRotSpeed = 1;
    public float cameraZoomSpeed = 5;
    public float playerSpeed = 5;
    

    Vector3 CameraRot = Vector3.zero;
    Vector3 SmoothCameraRot = Vector3.zero;
    Vector3 playerMovement = Vector3.zero; 
    Vector3 DragPosition = Vector3.zero;
    Vector3 DragEndPosition = Vector3.zero;
    Vector3[] DragRect = new Vector3[4];

    float cameraZoom = 0;
    float smoothCameraZoom = 0;
    Vector3 cameraOffset = Vector3.zero;
    

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = transform.position;
        lineRanderer = dragLine.GetComponent<LineRenderer>();
        lineRanderer = dragLine.GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //camera rotate
        if (Input.GetMouseButton(1))
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
        
        cameraOffset.z = Mathf.Lerp(cameraOffset.z, cameraZoom,Time.deltaTime * 10);
        SceneData.instance.camera.transform.localPosition = cameraOffset;

        //player movement
        float delta = playerSpeed * Time.deltaTime;
        if (  Input.GetAxis("Horizontal") != 0.0f)
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

        //selet unit
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(SceneData.instance.camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                    {
                        dragLine.SetActive(true);
                        DragPosition = hit.point;
                    }
                }
            }
            if (DragPosition != Vector3.zero && Input.GetMouseButton(0))
            {
                if (Physics.Raycast(SceneData.instance.camera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit))
                {
                    if (hit.transform.gameObject.layer == LayerMask.NameToLayer("Ground"))
                    {
                        DragEndPosition = hit.point;
                        setDragRect(DragPosition,DragEndPosition,DragPosition.y+ 1);
                        lineRanderer.SetPositions(DragRect);
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.LeftShift))
        {
            if (DragPosition != DragEndPosition)
            {
                SetCurrentUnit();
            }
            dragLine.SetActive(false);
            DragPosition = Vector3.zero;
            DragEndPosition = Vector3.zero;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            foreach (GameObject g in SceneData.instance.CurrentUnit)
            {
                g.active = !(g.active);
            }
        }

    }
    private void setDragRect(Vector3 point1, Vector3 point2, float y)
    {
        DragRect[0] = point1;
        DragRect[1].x = point1.x;
        DragRect[1].z = point2.z;
        DragRect[2] = point2;
        DragRect[3].x = point2.x;
        DragRect[3].z = point1.z;
        for (int i = 0; i < 4; i++)
        {
            DragRect[i].y = y;
        }
    }
    void SetCurrentUnit()
    {
        SceneData.instance.CurrentUnit.Clear();
        List<GameObject> units = UnitManager.instance.UnitList;
        Rect rect = new Rect(
            DragPosition.x < DragEndPosition.x? DragPosition.x : DragEndPosition.x,
            DragPosition.z < DragEndPosition.z ? DragPosition.z : DragEndPosition.z,
            DragPosition.x > DragEndPosition.x ? DragPosition.x - DragEndPosition.x : DragEndPosition.x - DragPosition.x,
            DragPosition.z > DragEndPosition.z ? DragPosition.z - DragEndPosition.z : DragEndPosition.z - DragPosition.z
            );
        Vector2 point = Vector2.zero;

        for (int i = 0; i < units.Count; i++)
        {
            if (units[i] != null)
            {
                point.x = units[i].transform.position.x;
                point.y = units[i].transform.position.z;
                if (rect.Contains(point))
                {
                    SceneData.instance.CurrentUnit.Add(units[i]);
                    units[i].GetComponent<AirUnit>().StartMoveToTarget(transform.position);
                }
            }
        }        
    }
}

