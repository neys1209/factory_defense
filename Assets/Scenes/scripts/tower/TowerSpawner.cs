using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSpawner : MonoBehaviour
{
    //회전
    private TowerTemplate.Dir dir = TowerTemplate.Dir.Down;

    private TowerTemplate towerTem;

    [SerializeField]
    private TowerTemplate[] towerTemplate;   //타워 정보
    [SerializeField]
    private GameObject towerPrefab;

    public LayerMask layermask;
    public GameObject removePress;

    [SerializeField]
    private TowerDataViewer towerDataViewer;

    public GameObject gridRine;

    private bool isOnTowerButton = false;

    private bool reMove;

    [SerializeField]
    private GameObject follwTowerClone = null;

    private int towerType;


    private Vector3 placedObjcetWorldPosition;

    //  private Ground tile ;

    int x;
    int y;
    int z;
    int cellSize;
    private void Awake()
    {
        reMove = false;
        cellSize = 1;
        // tile = GetComponent<Ground>();
        // towerTem = towerTemplate[0];

    }



    public Vector3 GetWorldPosition(int x, int z)
    {
        return new Vector3(x, 0, z);
    }

    public void GetXYZ(Vector3 worldPosition, out int x, out int y, out int z)
    {
        x = Mathf.FloorToInt(worldPosition.x);
        y = Mathf.FloorToInt(worldPosition.y);
        z = Mathf.FloorToInt(worldPosition.z);
    }

    public float GetCellSize()
    {
        return cellSize;
    }

    List<Vector3Int> gridPositionList;
    public Ground[] tiles;

    bool onPanel;
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject()) // UI이미지 클릭시 뒤에 배경 클릭 차단
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
            {
                GetXYZ(hit.point, out int x, out int y, out int z);

                if (follwTowerClone != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                {
                    // follwTowerClone.transform.position = new Vector3(Mathf.FloorToInt(hit.point.x),
                    // hit.point.y, Mathf.FloorToInt(hit.point.z));

                   

                    Vector2Int rotationOffset = towerTemplate[towerType].GetRotationOffset(dir);

                    placedObjcetWorldPosition = GetWorldPosition(x, z) +
                           new Vector3(rotationOffset.x, Mathf.FloorToInt(hit.point.y), rotationOffset.y) * GetCellSize();

                    follwTowerClone.transform.position = placedObjcetWorldPosition;

                    follwTowerClone.transform.rotation = Quaternion.Euler(0, towerTemplate[towerType].GetRotationAngle(dir), 0);


                    if (Input.GetMouseButtonDown(0))
                    {
                        gridPositionList = towerTemplate[towerType].GetGridPositionList(new Vector3Int(x, y, z), dir);

                        bool canBuild = true;
                        foreach (Vector3Int gridPositionn in gridPositionList)
                        {
                            foreach (Ground tileee in tiles)
                            {
                                Vector3 poss = new Vector3(Mathf.FloorToInt(tileee.transform.position.x), Mathf.FloorToInt(tileee.transform.position.y), Mathf.FloorToInt(tileee.transform.position.z));


                                if (gridPositionn == poss && tileee.IsBuildTower)
                                {
                                    canBuild = false;
                                    return;
                                }
                            }
                            
                        }

                        // GridObject gridObject = grid.GetGridObject(x, z);
                        if (canBuild)
                        {
                            SpawnTower(hit.transform, gridPositionList); //, placedObjcetWorldPosition);

                        }
                    }



                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        dir = TowerTemplate.GetNextDir(dir);
                    }

                }

                else if (onPanel)
                {
                    if (Input.GetMouseButtonDown(0) && hit.transform.CompareTag("Tower"))
                    {
                        towerDataViewer.OnPanel(hit.transform);
                    }
                }

                if (Input.GetMouseButtonDown(1) && reMove && (hit.collider.gameObject.layer == LayerMask.NameToLayer("Tower")))
                {                   
                    hit.collider.gameObject.GetComponent<Tower>().Sell();                   
                }


            }


        }
        /*
       
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layermask))
            {
                SpawnTower(hit.transform);
               
            }
        }
       */
    }




    // Start is called before the first frame update
    public void SpawnTower(Transform towerposition, List<Vector3Int> gridPositionList) //, Vector3 placedObjcetWorldPosition)
    {
        // towerTem = towerTemplate[towerType];
        if (isOnTowerButton == false)
        {
            onPanel = true;
            return;
        }



        Ground tile = towerposition.GetComponent<Ground>();

        //선택한 타일의 위치에 타워 건설

        if (tile.IsBuildTower == true)
        {
            onPanel = true;
            return;
        }

        isOnTowerButton = false;

        tile.IsBuildTower = true;

        // Vector3 position = towerposition.position;
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab,
                           placedObjcetWorldPosition,
                           Quaternion.Euler(0, towerTemplate[towerType].GetRotationAngle(dir), 0));
        //  GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, pos, Quaternion.identity);
        // clone.GetComponent<Tower>().Setup(tile);

        foreach (Vector3Int gridPosition in gridPositionList)
        {

            foreach (Ground tilee in tiles)
            {
                Vector3 pos = new Vector3(Mathf.FloorToInt(tilee.transform.position.x), Mathf.FloorToInt(tilee.transform.position.y), Mathf.FloorToInt(tilee.transform.position.z));


                if (gridPosition == pos)
                {
                    // Debug.Log(gridPosition);
                    // Debug.Log(pos);
                    tilee.IsBuildTower = true;
                }
            }
        }
        clone.GetComponentInChildren<Tower>().Setup(tiles, gridPositionList);
        Destroy(follwTowerClone);
        StopCoroutine("OnTowerCancel");

       // gridRine.SetActive(false);
        onPanel = true;

    }

    private IEnumerator OnTowerCancel()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isOnTowerButton = false;
               // gridRine.SetActive(false);
                Destroy(follwTowerClone);

                onPanel = true;
                break;
            }

            yield return null;
        }
    }


    public void ReadyToSpawnTower(int type)
    {
        int num = type;
        onPanel = false;

        //gridRine.SetActive(true);
        // 버튼을 중복해서 누르는 것을 방지하기 위해 필요
        if (isOnTowerButton == true)
        {

            Destroy(follwTowerClone);

            if (towerType != num)
            {
                follwTowerClone = Instantiate(towerTemplate[num].followTowerPrefab, Vector3.zero, Quaternion.Euler(0, towerTemplate[num].GetRotationAngle(dir), 0));

                towerType = num;

                return;
            }
            isOnTowerButton = false;
            towerType = num;

           // gridRine.SetActive(false);
            StopCoroutine(OnTowerCancel());
            return;
        }



        towerType = num;
        // 타워 건설 버튼을 눌렀다고 설정
        isOnTowerButton = true;

        //마우스를 따라다니는 임시 타워 생성
        follwTowerClone = Instantiate(towerTemplate[towerType].followTowerPrefab, Vector3.zero, Quaternion.Euler(0, towerTemplate[towerType].GetRotationAngle(dir), 0));

        StartCoroutine(OnTowerCancel());
    }

    public void RemoveTower()
    {

        reMove = !reMove;
        removePress.SetActive(reMove);
    }
}

