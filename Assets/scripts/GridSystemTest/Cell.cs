using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FDBlock;

[System.Serializable]
public class Cell
{
    // Start is called before the first frame update

    public Vector2 GridSpacePostion = new Vector2();
    public bool isEmpty = true;
    GameObject data;
    Block blockData;

    public Resource.Type OnResourcetype = Resource.Type.Air;

    public void SetData(GameObject obj)
    {
        data = obj;
        blockData = obj.GetComponent<Block>();
        isEmpty = false;
        GridSystem.ActivatedCell.Add(((int)GridSpacePostion.x, (int)GridSpacePostion.y));
    }

    public GameObject GetData()
    {
        return data;
    }
    public Block GetBlockData()
    {
        return blockData;
    }
    
    public void Init()
    {
        if (OnResourcetype != Resource.Type.Air)
        {
            GameObject obj = GameObject.Instantiate(ResourceList.Inst.dictionary[OnResourcetype]);
            obj.transform.position = GridSystem.Inst.MapPosition2WorldPostion(GridSpacePostion) + GridSystem.Inst.Offset;
            obj.transform.localScale = obj.transform.localScale * 1.3f;
            obj.transform.Translate(Vector3.down * 0.5f);
            obj.transform.parent = GridSystem.Inst.transform;
        }
    }

    public void Reload()
    {
        if (data == null)
        {
            blockData = null;
            isEmpty = true;
            GridSystem.ActivatedCell.Remove(((int)GridSpacePostion.x, (int)GridSpacePostion.y));
        }
    }
}
