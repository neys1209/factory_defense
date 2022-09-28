using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell
{
    // Start is called before the first frame update

    public Vector2 GridSpacePostion = new Vector2();
    public bool isEmpty = true;
    GameObject data;
    public void SetData(GameObject obj)
    {
        data = obj;
        isEmpty = false;
    }

    public GameObject GetData()
    {
        return data;
    }

    public void Reload()
    {
        if (data == null)
        {
            isEmpty = true;
        }
        
    }
}
