using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    // Start is called before the first frame update

    public const int MapWidth = 16;
    public const int MapHeight = 16;
    
    public static Cell[,] MapData = new Cell[MapWidth,MapHeight];

    public static Vector2 CellSize = new Vector2(1.0f,1.0f);

    public Block block;
    GameObject preview;
    public Material previewMaterial;

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
        CellSize = new Vector2(transform.lossyScale.x / MapWidth, transform.lossyScale.z / MapHeight);
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
                if (getCellData(hitPos.x, hitPos.y) != null && getCellData(hitPos.x, hitPos.y).isEmpty)
                {
                    if (Input.GetMouseButton(0))
                    {
                        PlaceBlock(block.gameObject, hit.point, block.CellSize);
                    }

                    preview.transform.position = GetBlockPosition(WorldPostion2MapPosition(hit.point), block.CellSize);
                    preview.transform.Translate(Vector3.up * 0.2f);
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


    }

    public Vector2 WorldPostion2MapPosition(Vector3 Wp)
    {
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
        return MapPosition2WorldPostion(MapPos) + new Vector3(CellSize.x * 0.5f * (objCellSize.x), 0.0f, CellSize.y * 0.5f * (objCellSize.y));
    }
    private void SetPreview(GameObject obj)
    {
        if (preview != null) Destroy(preview);
        if (obj == null) return;
        preview = Instantiate(obj);
        Material[] materials = preview.GetComponent<MeshRenderer>().materials;
        materials[0] = previewMaterial;
        preview.GetComponent<MeshRenderer>().materials = materials;
    }

    public Cell getCellData(int x, int y)
    {
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
        if (0 <= x && x < MapWidth)
        {
            if (0 <= y && y < MapHeight)
                return MapData[(int)x, (int)y];            
        }
        return null;
    }

    public void DeltiteCellData(GameObject obj,Vector2 index, Vector2 size)
    {
        DestroyImmediate(obj); //즉시 삭제
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
        
}

