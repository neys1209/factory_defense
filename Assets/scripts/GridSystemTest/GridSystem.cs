using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
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
                        PlaceBlock(block.gameObject, hit.point, block.CellSize);
                    }

                    if (WorldPostion2MapPosition(preview.transform.position) != WorldPostion2MapPosition(hit.point))
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
                else if (getCellData(hitPos.x, hitPos.y) != null)
                {
                    if (Input.GetMouseButton(1))
                    {
                        
                        DeltiteCellData(getCellData(hitPos.x, hitPos.y).GetData(),
                            hitPos,
                            getCellData(hitPos.x, hitPos.y).GetData().GetComponent<Block>().CellSize);
                    }
                }
            }
        }
        else preview?.SetActive(false);
        
        if(Input.GetKeyDown(KeyCode.P))
        {
            string d = "";
            foreach ((int, int) x in ActivatedCell)
            {
                d += x.ToString();
            }
            Debug.Log(d);
        }

    }


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

    public void PlaceBlock(GameObject prefab,Vector3 position,Vector2 objCellSize)
    {
        //�׸���� ���� ���� �Լ��Դϴ�
        Vector2 pos = WorldPostion2MapPosition(position); 
        
        if (!CanPlaceBlock(pos,objCellSize))
        {
            return;
        }
        GameObject obj = Instantiate(prefab);
        obj.transform.position = GetBlockPosition(pos,objCellSize);
        obj.transform.parent = transform;
        
        for (int x = 0; x < objCellSize.x; x++)
        {
            for (int y = 0; y < objCellSize.y; y++)
            {
                getCellData((int)pos.x + x, (int)pos.y + y)?.SetData(obj);                
            }
        }
        obj.SetActive(true);
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
        Material[] materials = preview.GetComponent<MeshRenderer>().materials;
        materials[0] = previewMaterial;
        preview.GetComponent<MeshRenderer>().materials = materials;
        preview.GetComponent<Collider>().isTrigger = true;
        preview.GetComponent<Rigidbody>().isKinematic = true;
    }

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

    IEnumerator ViewGridCell(Vector2 pos, Vector2 objCellSize)
    {
        List<CellViewer> objlist = new List<CellViewer>();
        //���� ������ ��ġ�� �̸� �����ִ� �Լ�
        for (int x = -1; x <= objCellSize.x; x++)
        {
            for (int y = -1; y <= objCellSize.y; y++)
            {
                Cell celldata = getCellData((int)pos.x + x, (int)pos.y + y);
                if (celldata != null)
                {
                    GameObject obj = Instantiate(CellViewer);
                    obj.transform.position = MapPosition2WorldPostion(celldata.GridSpacePostion) + new Vector3(CellSize.x*0.5f, 0,CellSize.y*0.5f);
                    objlist.Add(obj.GetComponent<CellViewer>());
                }
            }
        }
        yield return new WaitUntil(()=>previewSetOffset);
        for (int x = 0; x < objlist.Count; x++)
        {
            objlist[x]?.StartDelite();
        }
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
        
}

