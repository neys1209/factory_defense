using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridProcessor : MonoBehaviour
{
    

    public void Processing() //¾È¾¸
    {
        foreach (var Index in GridSystem.ActivatedCell)
        {
            Cell cell = GridSystem.Inst.getCellData(Index.x, Index.y);
            if (cell == null) continue;

            switch (cell.GetBlockData().blockType)
            {
                case Block.Type.Turret:
                    break;
                case Block.Type.Factory:   
                    break;
                case Block.Type.Wall:
                    break;
                case Block.Type.Conveyor:
                    break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
