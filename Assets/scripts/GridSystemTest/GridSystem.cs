using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridSystem : MonoBehaviour
{
    #region ����
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

    #region ��ǥ�躯ȯ �Լ���
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

    #region �� ���� ���� �Լ���
    public bool PlaceBlock(GameObject prefab,Vector3 position,Vector2 objCellSize)
    {
        //�׸���� ���� ���� �Լ��Դϴ�
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
        //�� ����
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
        //���� ���� �� ����ĭ�� �����ϴ� ���� ��ġ�� ����� �����ϴ� �Լ��Դϴ�
        return MapPosition2WorldPostion(MapPos) + new Vector3(CellSize.x * 0.5f * (objCellSize.x), 0.0f, CellSize.y * 0.5f * (objCellSize.y));
    }
    private void SetPreview(GameObject obj)
    {
        //��ġ�� ���� �����並 �����ϴ� �Լ��Դϴ�.
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

    #region ��Ÿ ���/���� �Լ�
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
        //���� �����ϰ� ���� �ִ� �ڸ��� ���� ���ε� ������ �ʱ�ȭ�մϴ�. 
        //������ ���¸� ����� �α��� ������ ��� ���ε�˴ϴ�.
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
        //���� ���� �ڸ��� �̹� ���𰡰� �ִ��� Ȯ���ϴ� �Լ��Դϴ�
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
        //���� ������ ��ġ�� �̸� �����ִ� �Լ�
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