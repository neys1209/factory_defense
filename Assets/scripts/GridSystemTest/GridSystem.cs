using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridSystem : MonoBehaviour
{
    #region 변수
    public const int MapWidth = 30;
    public const int MapHeight = 30;
    
    public static Cell[,] MapData = new Cell[MapWidth,MapHeight];
    public static List<(int x, int y)> ActivatedCell = new List<(int x, int y)>();

    public static Vector2 CellSize = new Vector2(1.0f,1.0f);

    public Block block;
    GameObject preview;
    public Material previewMaterial;
    
    public GameObject CellViewer;
    bool previewSetOffset = false;
    public static GridSystem Inst;

    Vector2 _lastPlaceOffset = Vector2.zero;
    public Vector2 lastPlaceOffset { get => _lastPlaceOffset; }
    #endregion

    private void Awake()
    {
        Inst = this;
        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                MapData[x, y] = new Cell();
                MapData[x, y].GridSpacePostion = new Vector2(x, y);
            }
        }
        CellSize = new Vector2(transform.localScale.x / MapWidth, transform.localScale.z / MapHeight);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (block != null)
        {
            if (preview == null)
            {
                SetPreview(block.gameObject);
            }
            preview.SetActive(false);
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Ground")))
            {
                Vector2 hitPos = WorldPostion2MapPosition(hit.point);
                if (getCellData(hitPos.x, hitPos.y) != null)
                {
                    if (Input.GetMouseButton(0))
                    {
                        if (PlaceBlock(block.gameObject, hit.point, block.CellSize))
                            _lastPlaceOffset = WorldPostion2MapPosition(hit.point);
                    }
                    else if (Input.GetMouseButton(1) && !getCellData(hitPos.x, hitPos.y).isEmpty)
                    {
                        GameObject deliteCellData = getCellData(hitPos.x, hitPos.y).GetData();
                        DeltiteCellData(deliteCellData,hitPos,deliteCellData.GetComponent<Block>().CellSize);
                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        block = null;
                        previewSetOffset = true;
                        return;
                    }

                    if ((int)Vector3.Distance(Vector3.zero, preview.transform.position) != (int)Vector3.Distance(Vector3.zero,GetBlockPosition(WorldPostion2MapPosition(hit.point), block.CellSize)))
                    {
                        preview.transform.position = GetBlockPosition(WorldPostion2MapPosition(hit.point), block.CellSize);
                        preview.transform.Translate(Vector3.up * 0.1f);
                        previewSetOffset = true;
                    }
                    else
                    {
                        if (previewSetOffset)
                        {
                            previewSetOffset = false;
                            StartCoroutine(ViewGridCell(WorldPostion2MapPosition(hit.point), block.CellSize));
                        }
                    }
                    preview.SetActive(true);

                }
            }
        }
        else preview?.SetActive(false);
    }

    #region 좌표계변환 함수들
    public Vector2 WorldPostion2MapPosition(Vector3 Wp)
    {
        //월드의 좌표를 그리드상의 좌표로 변환합니다
        float x = Wp.x;
        float y = Wp.z;

        x += transform.lossyScale.x * 0.5f;
        y += transform.lossyScale.z * 0.5f;
        x -= transform.position.x;
        y -= transform.position.z;
        x /= CellSize.x;
        y /= CellSize.y;
        return new Vector2((int)x, (int)y);
    }
    public Vector3 MapPosition2WorldPostion(Vector2 Mp)
    {
        //그리드상의 좌표를 월드 좌표계로 변환합니다. 다만 y값은 고정되어 있습니다
        float x = Mp.x;
        float y = Mp.y;
        x -= transform.lossyScale.x * 0.5f;
        y -= transform.lossyScale.z * 0.5f;
        x += transform.position.x;
        y += transform.position.z;
        x *= CellSize.x;
        y *= CellSize.y;

        return new Vector3(x, 1, y);
    }
    #endregion

    #region 블럭 놓기 관련 함수들
    public bool PlaceBlock(GameObject prefab,Vector3 position,Vector2 objCellSize)
    {
        //그리드상에 블럭을 놓는 함수입니다
        Vector2 pos = WorldPostion2MapPosition(position); 
        
        if (!CanPlaceBlock(pos,objCellSize))
        {
            return false;
        }
        GameObject obj = Instantiate(prefab);
        obj.transform.position = GetBlockPosition(pos,objCellSize);
        obj.transform.parent = transform;
        if (obj.GetComponent<Block>().rotatable)
        {
            Vector2 dir = pos - lastPlaceOffset;
            obj.GetComponent<Block>().BlockRotate(Array.IndexOf(Block.rotations, ((int)dir.x, (int)dir.y)));
        }
        //셀 설정
        for (int x = 0; x < objCellSize.x; x++)
        {
            for (int y = 0; y < objCellSize.y; y++)
            {
                getCellData((int)pos.x + x, (int)pos.y + y)?.SetData(obj);
                obj.GetComponent<Block>().OnCell.Add(getCellData((int)pos.x + x, (int)pos.y + y));
            }
        }
        
        obj.SetActive(true);
        return true;
    }
    public Vector3 GetBlockPosition(Vector2 MapPos,Vector2 objCellSize)
    {
        //블럭을 놓을 때 여러칸을 차지하는 블럭의 위치를 가운데로 조정하는 함수입니다
        return MapPosition2WorldPostion(MapPos) + new Vector3(CellSize.x * 0.5f * (objCellSize.x), 0.0f, CellSize.y * 0.5f * (objCellSize.y));
    }
    private void SetPreview(GameObject obj)
    {
        //설치할 블럭의 프리뷰를 설정하는 함수입니다.
        if (preview != null) Destroy(preview);
        if (obj == null) return;
        preview = Instantiate(obj);
        Material[] materials = preview.GetComponentInChildren<MeshRenderer>().materials;
        materials[0] = previewMaterial;
        preview.GetComponentInChildren<MeshRenderer>().materials = materials;
        preview.GetComponentInChildren<Collider>().isTrigger = true;
        preview.GetComponentInChildren<Rigidbody>().isKinematic = true;
    }

    #endregion

    #region 기타 계산/편의 함수
    public Cell getCellData(int x, int y)
    {
        //인덱스(그리드 좌표)를 통해 그 좌표의 셀 데이터를 얻어옵니다
        //맵 데이터 배열에 직접 접근하는 경우 인덱스 에러가 날 수 있으니 본 함수를 사용하기를 권장합니다
        //인덱스를 초과하는 경우 null을 리턴합니다
        if (0 <= x && x < MapWidth)
        {
            if (0 <= y && y < MapHeight)
            {
                return MapData[x, y];
            }
        }    
        return null;
    }
    public Cell getCellData(float x, float y)
    {
        //getCellData(int,int)의 오버로드
        if (0 <= x && x < MapWidth)
        {
            if (0 <= y && y < MapHeight)
                return MapData[(int)x, (int)y];            
        }
        return null;
    }
    public Cell getCellData(Vector2 pos)
    {
        //getCellData(int,int)의 오버로드2
        if (0 <= pos.x && pos.x < MapWidth)
        {
            if (0 <= pos.y && pos.y < MapHeight)
                return MapData[(int)pos.x, (int)pos.y];
        }
        return null;
    }
    public Cell getCellData((int x,int y) pos)
    {
        //getCellData(int,int)의 오버로드3
        if (0 <= pos.x && pos.x < MapWidth)
        {
            if (0 <= pos.y && pos.y < MapHeight)
                return MapData[(int)pos.x, (int)pos.y];
        }
        return null;
    }

    public void DeltiteCellData(GameObject obj,Vector2 index, Vector2 size)
    {
        //블럭을 제거하고 블럭이 있던 자리의 셀을 리로드 함으로 초기화합니다. 
        //만일의 사태를 대비해 인근의 셀들이 모두 리로드됩니다.
        DestroyImmediate(obj); //즉시 삭제함수
        for (int x = (int)(index.x - size.x + 1); x < index.x + size.x; x++)
        {
            for (int y = (int)(index.y - size.y + 1); y < index.y + size.y; y++)
            {
                getCellData(x, y)?.Reload();
            }
        }
    }
    public bool CanPlaceBlock(Vector2 pos, Vector2 objCellSize)
    {
        //블럭을 놓을 자리에 이미 무언가가 있는지 확인하는 함수입니다
        for (int x = 0; x < objCellSize.x; x++)
        {
            for (int y = 0; y < objCellSize.y; y++)
            {
                if (getCellData((int)pos.x + x, (int)pos.y + y) == null || !getCellData((int)pos.x + x, (int)pos.y + y).isEmpty)
                {
                    return false;
                }
                    
            }
        }
        return true;
    }
    public void SetCurrentBlock(GameObject obj)
    {
        Block blockobj = obj.GetComponent<Block>();
        if (blockobj != null)
        {
            block = blockobj;
        }
        SetPreview(obj);
    }
    public bool Vector3EqualVector3(Vector3 A,Vector3 B)
    {
        if (!Mathf.Approximately(A.x, B.x)) return false;
        if (!Mathf.Approximately(A.y, B.y)) return false;
        if (!Mathf.Approximately(A.z, B.z)) return false;
        return true;

    }
    IEnumerator ViewGridCell(Vector2 pos, Vector2 objCellSize)
    {

        List<CellViewer> objlist = new List<CellViewer>();
        //블럭이 놓아질 위치를 미리 보여주는 함수
        for (int x = -1; x <= objCellSize.x; x++)
        {
            for (int y = -1; y <= objCellSize.y; y++)
            {
                Cell celldata = getCellData((int)pos.x + x, (int)pos.y + y);
                if (celldata != null && !ActivatedCell.Contains(((int)pos.x + x, (int)pos.y + y)))
                {
                    GameObject obj = Instantiate(CellViewer);
                    obj.transform.position = MapPosition2WorldPostion(celldata.GridSpacePostion) + new Vector3(CellSize.x * 0.5f, -0.4f, CellSize.y * 0.5f);
                    objlist.Add(obj.GetComponent<CellViewer>());
                }
            }
        }
        yield return new WaitUntil(() => previewSetOffset);
        for (int x = 0; x < objlist.Count; x++)
        {
            objlist[x]?.StartDelite();
        }
    }
    #endregion
}