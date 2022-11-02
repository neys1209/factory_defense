using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using FDBlock;

public class GridSystem : MonoBehaviour
{
    #region 변수
    public const int MapWidth = 80;
    public const int MapHeight = 80;
    
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

    public Vector3 Offset = new Vector3(CellSize.x * 0.5f, 0, CellSize.y * 0.5f);

    public bool GameStart = false;
    
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
        InputResourceInMap(Resource.Type.Sand, -1, -1, 10, 10);
        InputResourceInMap(Resource.Type.Coal,-1,-1,6,2);
        InputResourceInMap(Resource.Type.Iron, -1, -1, 4, 5);
        
    }
    void Start()
    {
        transform.localScale = new Vector3(MapWidth, transform.lossyScale.y,MapHeight);
        for (int x = 0; x < MapWidth; x++)
        {
            for (int y = 0; y < MapHeight; y++)
            {
                MapData[x, y].Init();
            }
        }
        GameStart = true;
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

                    if (Input.GetKeyDown(KeyCode.R))
                    {
                        if (preview.GetComponent<Block>().rotatable)
                        {
                            preview.GetComponent<Block>().OneRotate();
                        }
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

            if (Input.GetMouseButtonUp(0)) _lastPlaceOffset = Vector2.zero;
        }
        else preview?.SetActive(false);
    }

    #region 좌표 변환
    public Vector2 WorldPostion2MapPosition(Vector3 Wp)
    {
        //������ ��ǥ�� �׸������ ��ǥ�� ��ȯ�մϴ�
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
        //�׸������ ��ǥ�� ���� ��ǥ��� ��ȯ�մϴ�. �ٸ� y���� �����Ǿ� �ֽ��ϴ�
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

    #region 블럭 놓기 관련 함수
    public bool PlaceBlock(GameObject prefab,Vector3 position,Vector2 objCellSize)
    {
        //�׸���� ������ ���� �Լ��Դϴ�
        Vector2 pos = WorldPostion2MapPosition(position); 
        
        if (!CanPlaceBlock(pos,objCellSize))
        {
            return false;
        }
        GameObject obj = Instantiate(prefab);
        Block objBlock = obj.GetComponent<Block>();
        obj.transform.position = GetBlockPosition(pos,objCellSize);
        obj.transform.parent = transform;
        if (objBlock.rotatable)
        {
            Vector2 dir = pos - lastPlaceOffset;
            if (dir.magnitude <= 1.1f)
            {
                objBlock.BlockRotate(Array.IndexOf(Block.rotations, ((int)dir.x, (int)dir.y)));
            }
            else
            {
                objBlock.SetRotation(preview.GetComponent<Block>().GetRotationIndex());
            }
        }
        //�� ����
        for (int x = 0; x < objCellSize.x; x++)
        {
            for (int y = 0; y < objCellSize.y; y++)
            {
                getCellData((int)pos.x + x, (int)pos.y + y)?.SetData(obj);
                objBlock.OnCell.Add(getCellData((int)pos.x + x, (int)pos.y + y));
            }
        }
        
        obj.SetActive(true);
        return true;
    }
    public Vector3 GetBlockPosition(Vector2 MapPos,Vector2 objCellSize)
    {
        //������ ���� �� ����ĭ�� �����ϴ� ������ ��ġ�� ����� �����ϴ� �Լ��Դϴ�
        return MapPosition2WorldPostion(MapPos) + new Vector3(CellSize.x * 0.5f * (objCellSize.x), 0.0f, CellSize.y * 0.5f * (objCellSize.y));
    }
    private void SetPreview(GameObject obj)
    {
        //��ġ�� ������ �����並 �����ϴ� �Լ��Դϴ�.
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

    #region 셀 데이터 가져오기, 기타 등등
    public Cell getCellData(int x, int y)
    {
        //�ε���(�׸��� ��ǥ)�� ���� �� ��ǥ�� �� �����͸� ���ɴϴ�
        //�� ������ �迭�� ���� �����ϴ� ��� �ε��� ������ �� �� ������ �� �Լ��� ����ϱ⸦ �����մϴ�
        //�ε����� �ʰ��ϴ� ��� null�� �����մϴ�
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
        //getCellData(int,int)�� �����ε�
        if (0 <= x && x < MapWidth)
        {
            if (0 <= y && y < MapHeight)
                return MapData[(int)x, (int)y];            
        }
        return null;
    }
    public Cell getCellData(Vector2 pos)
    {
        //getCellData(int,int)�� �����ε�2
        if (0 <= pos.x && pos.x < MapWidth)
        {
            if (0 <= pos.y && pos.y < MapHeight)
                return MapData[(int)pos.x, (int)pos.y];
        }
        return null;
    }
    public Cell getCellData((int x,int y) pos)
    {
        //getCellData(int,int)�� �����ε�3
        if (0 <= pos.x && pos.x < MapWidth)
        {
            if (0 <= pos.y && pos.y < MapHeight)
                return MapData[(int)pos.x, (int)pos.y];
        }
        return null;
    }

    public void DeltiteCellData(GameObject obj,Vector2 index, Vector2 size)
    {
        //������ �����ϰ� ������ �ִ� �ڸ��� ���� ���ε� ������ �ʱ�ȭ�մϴ�. 
        //������ ���¸� ����� �α��� ������ ��� ���ε�˴ϴ�.
        Block block = obj.GetComponent<Block>();
        if (block != null && block.Unbreakable) return;
        block.DestoryMyself();
        DestroyImmediate(obj); //��� �����Լ�
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
        //������ ���� �ڸ��� �̹� ���𰡰� �ִ��� Ȯ���ϴ� �Լ��Դϴ�
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
        //������ ������ ��ġ�� �̸� �����ִ� �Լ�
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

    void InputResourceInMap(Resource.Type res,int x = -1,int y = -1,int spread = 1,int frequency=1)
    {
        for (int i = 0; i < frequency; i++)
        {
            if (x < 0 || y < 0)
            {
                x = UnityEngine.Random.Range(0, MapWidth);
                y = UnityEngine.Random.Range(0, MapHeight);
            }
            if (getCellData(x, y) != null && getCellData(x, y).OnResourcetype != res)
            {
                getCellData(x, y).OnResourcetype = res;
                for (int j = 0; j < spread; j++)
                    InputResourceInMap(res, x + UnityEngine.Random.Range(-1, 1), y + UnityEngine.Random.Range(-1, 1), spread - 1);
            }
            x = -1;
            y = -1;
        }
    }
}
