using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : Block
{
    private void Awake()
    {
        blockType = Type.Conveyor;
        rotatable = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        init();
    }

    // Update is called once per frame
    void Update()
    {
        if (Activate)
        {
            
            Processing();
        }
    }



    new public void Processing()
    {
        if (InventoryCount != 0)
        {
            foreach (var cell in OnCell)
            {        
                GameObject adjacentCells = GridSystem.Inst.getCellData(cell.GridSpacePostion + VectorRotation())?.GetData();
                if (adjacentCells!=null && adjacentCells.GetComponent<Block>().Activate)
                {
                    Block cellblock = adjacentCells.GetComponent<Block>();
                    switch (cellblock.blockType)
                    {
                        case Type.Conveyor:
                            if (adjacentCells.GetComponent<Block>().InventoryCount == 0)
                            {
                                Inventory[0].MoveCell(VectorRotation(), 2f, cellblock.InputInventory);
                                OutputInventory(0);
                            }
                            break;
                        case Type.Factory:
                            if (adjacentCells.GetComponent<Factory>().CanInputResource(Inventory[0].type))
                            {
                                Inventory[0].MoveCell(VectorRotation(), 2f, 
                                    adjacentCells.GetComponent<Factory>().InputNeedInventory);
                                OutputInventory(0);
                            }
                            break;
                        case Type.Storage:
                            Inventory[0].MoveCell(VectorRotation(), 2f, cellblock.InputInventory);
                            OutputInventory(0);
                            break;

                    }
                }
            }
        }
        else
        {
            foreach (var cell in OnCell)
            {
                GameObject adjacentCells = GridSystem.Inst.getCellData(cell.GridSpacePostion - VectorRotation())?.GetData();
                if (adjacentCells != null && adjacentCells != gameObject && adjacentCells.GetComponent<Block>().Activate)
                {
                    Block cellblock = adjacentCells.GetComponent<Block>();
                    switch (cellblock.blockType)
                    {
                        case Type.Factory:
                            if (cellblock.InventoryCount != 0)
                            {
                                //if (cellblock.Inventory[0]/)
                                cellblock.Inventory[0].gameObject.SetActive(true);
                                cellblock.Inventory[0].MoveCell(VectorRotation(), 2f, InputInventory);
                                cellblock.OutputInventory(0);
                            }
                            break;
                    }
                }
            }
        }
    }

    

}
