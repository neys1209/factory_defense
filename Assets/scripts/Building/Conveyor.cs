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
        StartCoroutine(SetRigidBody());
    }

    // Update is called once per frame
    void Update()
    {
        if (Activate)
        {
            foreach (var item in Inventory)
            {
                item.transform.position = transform.position + Vector3.up * 0.1f;
                if (item == null) Inventory.Remove(item);
            }
            Processing();
        }
    }

    new public void Processing()
    {
        if (Inventory.Count != 0)
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
                            if (adjacentCells.GetComponent<Block>().Inventory.Count == 0)
                            {
                                Inventory[0].MoveCell(VectorRotation(), 2f, (res) => cellblock.Inventory.Add(res));
                                Inventory.RemoveAt(0);
                            }
                            break;
                        case Type.Factory:
                            if (adjacentCells.GetComponent<Factory>().CanInputResource(Inventory[0].type))
                            {
                                Inventory[0].MoveCell(VectorRotation(), 2f, 
                                    adjacentCells.GetComponent<Factory>().InputNeedInventory);
                                Inventory.RemoveAt(0);
                            }
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
                            if (cellblock.Inventory.Count != 0)
                            {
                                cellblock.Inventory[0].gameObject.SetActive(true);
                                cellblock.Inventory[0].MoveCell(VectorRotation(), 2f, (res) => Inventory.Add(res));
                                cellblock.Inventory.RemoveAt(0);
                            }
                            break;
                    }
                }
            }
        }
    }

    

}
