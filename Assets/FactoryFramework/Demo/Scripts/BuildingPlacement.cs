using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FactoryFramework;

public class BuildingPlacement : MonoBehaviour
{
    [Header("Event Channels to Handle State")]
    public VoidEventChannel_SO startPlacementEvent;
    public VoidEventChannel_SO finishPlacementEvent;
    public VoidEventChannel_SO cancelPlacementEvent;
    
    [Header("Building Prefabs")]
    public GameObject Miner;
    public GameObject Processor;
    public GameObject Factory;
    public GameObject Storage;
    public GameObject Splitter;
    public GameObject Merger;
    public GameObject Assembler;
    private GameObject current;

    [Header("Visual Feedback Building Materials")]
    public Material originalMaterial;
    public Material greenPlacementMaterial;
    public Material redPlacementMaterial;

    // list of valid materials so we don't change any other materials
    private Material[] validMaterials {
        get { 
            return new Material[3] { originalMaterial, greenPlacementMaterial, redPlacementMaterial }; 
        } 
    }

    [Header("Controls")]
    public KeyCode CancelKey = KeyCode.Escape;

    private enum State
    {
        None,
        PlaceBuilding,
        RotateBuilding
    }
    private State state;
    private bool RequiresResourceDepoist = false;

    // building placement variables to track
    private Vector3 mouseDownPos;
    private float mouseHeldTime = 0f;
    private float secondsHoldToRotate = .333f;

    private void OnEnable()
    {
        // listen to the cancel event to force cancel placement from elsewhere in the code
        cancelPlacementEvent.OnEvent += ForceCancel;
    }
    private void OnDisable()
    {
        // stop listening
        cancelPlacementEvent.OnEvent -= ForceCancel;
    }

    private void ForceCancel()//강제 취소
    {
        if (current != null)   //만약 current(저장되어 있는 오브젝트)가 null이 아니면 : 어떤 값이 저장되어 있으면
        {
            Destroy(current.gameObject);  //current에 저장되어 있는 오브젝트를 파괴하라
        }
        current = null;                   // current를 null로 설정
        this.state = State.None;          // 현재 상태를 None로 설정
    }

    public void PlaceMiner() => StartPlacingBuilding(Miner, true);
    public void PlaceProcessor() => StartPlacingBuilding(Processor);
    public void PlaceFactory() => StartPlacingBuilding(Factory);
    public void PlaceAssembler() => StartPlacingBuilding(Assembler);
    public void PlaceStorage() => StartPlacingBuilding(Storage);
    public void PlaceSplitter() => StartPlacingBuilding(Splitter);
    public void PlaceMerger() => StartPlacingBuilding(Merger);

    public void StartPlacingBuilding(GameObject prefab, bool requireDeposit=false)
    {
        cancelPlacementEvent?.Raise();
        RequiresResourceDepoist = requireDeposit;
        // spawn a prefab and start placement
        if (!TryChangeState(State.PlaceBuilding))    // PlaceBuilding 상태가 참이 아니면 
            return;                                 // 그냥 빠져 나가라.
        
        current = Instantiate(prefab);              //State.Building 이 참이면 지정된 prefab를 생성해서 current에 저장하라
        current.name = prefab.name;
        // don't let building "work" until placement is finished
        Building b = current.GetComponent<Building>();    //
        b.enabled = false;
        // init material to ghost
        ChangeMatrerial(greenPlacementMaterial);
        
    }

    private void ChangeMatrerial(Material mat)
    {
        foreach (MeshRenderer mr in current?.GetComponentsInChildren<MeshRenderer>())
        {
            // dont change materials that shouldn't be changed!
            if (validMaterials.Contains(mr.sharedMaterial))
                mr.sharedMaterial = mat;
        }
    }

    private void HandleIdleState()
    {
        // right click to delete
        if (Input.GetMouseButtonDown(1))  // 마우스 오른쪽 클릭이 참이면
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);  //마우스 위치에 ray를 투사하고
            foreach (RaycastHit hit in Physics.RaycastAll(ray, 100f))     // 투사한  ray가 부딪히는 곳을 hit에 저장
            {
                if (hit.collider.gameObject.TryGetComponent<Building>(out Building building))  // 만약 hit가 건물이고
                {
                    foreach(Socket socket in building.gameObject.GetComponentsInChildren<Socket>())  // 건물의 socket이면
                    {
                        // FIXME remove this from sockets
                    }
                    Destroy(building.gameObject);                                                    //건물을 파괴하라  
                    return;
                }
            }
        }
        return;
    }
    private void HandlePlaceBuildingState()
    {
        // move building with mouse pos
        Vector3 groundPos = transform.position;                            
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);                   //마우스 위치에 ray를 투사해서

        foreach (RaycastHit hit in Physics.RaycastAll(ray, 100f))                      //ray가 부딪힌 곳에 대해 
        {
            // this will only place buildings on terrain. feel free to change this!
            if (hit.collider.TryGetComponent<Terrain>(out Terrain terrain))            //ray가 terrain에 투사되면
            {
                groundPos = hit.point;                                                 //그 위치를 groundPos에 저장하라
            }
        }


        current.transform.position = groundPos;                                         //groundPos 값을 current 오브젝트의 위치로 설정하라
        bool valid = ValidLocation();
        // left mouse button to try to place building
        if (Input.GetMouseButtonDown(0) && valid)                                       //만약 마우스 좌클릭한 위치가 valid 하면
        {
            // try to change state to rotate the building
            if (TryChangeState(State.RotateBuilding))                                   //RotateBuilding 상태로 변환이 되면
                mouseDownPos = groundPos;                                               // groundPos의 값을 mouseDownPos에 설정하라.
        }

    }
    private void HandleRotateBuildingState()
    {
        // wait for mouse to be held for X seconds until building rotation is allowed
        // this prevents quick clicks resulting in seemingly random building rotations
        mouseHeldTime += Time.deltaTime;
        if (mouseHeldTime > secondsHoldToRotate)
        {
            bool valid = ValidLocation();
            // get new ground position to rotate towards
            Vector3 dir = current.transform.forward;
            // rotate the building!
            Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            foreach (RaycastHit hit in Physics.RaycastAll(mouseRay, 100f))
            {
                // thios demo script will only use a terrain object as the "ground"
                if (hit.collider.TryGetComponent<Terrain>(out Terrain terrain))
                    current.transform.forward = (mouseDownPos - hit.point).normalized;
            }
            current.transform.position = mouseDownPos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            TryChangeState(State.None);
        }
    }

    private bool ValidLocation()
    {
        if (current == null) return false;
        // this only works with box xcolliders because thats an assumption we made with the demo prefabs!
        if (current.TryGetComponent<BoxCollider>(out BoxCollider col))
        {
            bool onResourceDeposit = false;
            foreach (Collider c in Physics.OverlapBox(col.transform.TransformPoint(col.center), col.size/2f, col.transform.rotation))
            {
                if (c.tag == "Building" && c.gameObject != current.gameObject)
                {
                    // colliding something!
                    if (ConveyorLogisticsUtils.settings.SHOW_DEBUG_LOGS)
                        Debug.LogWarning($"Invalid placement: {current.gameObject.name} collides with {c.gameObject.name}");
                    ChangeMatrerial(redPlacementMaterial);
                    return false;
                }
                // check for resources
                if (c.tag == "Resources")
                {
                    onResourceDeposit = true;
                }
            }
            if (RequiresResourceDepoist && !onResourceDeposit)
            {
                if (ConveyorLogisticsUtils.settings.SHOW_DEBUG_LOGS)
                    Debug.LogWarning($"Invalid placement: {current.gameObject.name} requries placement near Resource Deposit");
                ChangeMatrerial(redPlacementMaterial);
                return false;
            }
        }
        ChangeMatrerial(greenPlacementMaterial);
        return true;
    }

    private bool TryChangeState(State desiredState)
    {
        if (desiredState == State.PlaceBuilding)
        {
            if (state != State.None || current != null)
            {
                // if currently placing a building, cancel it
                Destroy(current);
                state = State.None;
                cancelPlacementEvent?.Raise();
            }
            mouseHeldTime = 0f;
            this.state = desiredState;
            // trigger event
            startPlacementEvent?.Raise();
            return true;
        }
        if (desiredState == State.RotateBuilding)
        {
            this.state = desiredState;
            return true;
        }
        if (desiredState == State.None)
        {   
            // if we weren't placing a building, ignore
            if (current == null)
            {
                this.state = desiredState;
                return true;
            }

            // make sure building placement and rotation is valid
            if (ValidLocation())
            {
                // finish placing building and enable it
                this.state = desiredState;
                ChangeMatrerial(originalMaterial);
                Building b = current.GetComponent<Building>();
                b.enabled = true;
                current = null;
                // trigger event
                finishPlacementEvent?.Raise();
                return true;
            }
            else
            {
                this.state = State.PlaceBuilding;
                mouseHeldTime = 0f;
                return false;
            }
        }
        return false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(CancelKey))
        {
            if (current != null)
            {
                Destroy(current.gameObject);
                cancelPlacementEvent?.Raise();
            }
            current = null;
            state = State.None;
        }

        switch (state)
        {
            case State.RotateBuilding:
                HandleRotateBuildingState();
                break;
            case State.None:
                HandleIdleState();
                break;
            case State.PlaceBuilding:
                HandlePlaceBuildingState();
                break;
        }

    }

}
