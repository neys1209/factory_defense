using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FDBlock;

public class GridProcessor : MonoBehaviour
{
    
    public void Processing() //�Ⱦ�
    {
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
    public void Processing()
    {
    void Update()
    {
        
    }
}
