using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCell : MonoBehaviour
{

    private int PosX;
    private int PosY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPosition(int x, int y)
    {
        int PosX = x;
        int PosY = y;
    }
    public Vector2Int GetPosition()
    {
        return new Vector2Int(PosX, PosY);
    }
}
