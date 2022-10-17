using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : Block
{
    Block myBlock;
    private void Awake()
    {
        myBlock = GetComponent<Block>();
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
        foreach (var item in Inventory)
        {
            item.transform.position = transform.position + Vector3.up * 0.1f;
        }
        Processing();
    }

    new public void Processing()
    {
        if (Inventory.Count != 0)
        {
            foreach (var cell in OnCell)
            {        
                GameObject adjacentCells = GridSystem.Inst.getCellData(cell.GridSpacePostion + VectorRotation()).GetData();
                if (adjacentCells!=null)
                {
                    switch (adjacentCells.GetComponent<Block>().blockType)
                    {
                        case Type.Conveyor:
                            if (adjacentCells.GetComponent<Block>().Inventory.Count == 0)
                            {
                                Inventory[0].MoveCell(VectorRotation(), 2f, adjacentCells.GetComponent<Block>().Inventory);
                                Inventory.RemoveAt(0);
                            }
                            break;
                        case Type.Factory:
                            break;
                    }
                }
            }
               
        }
    }

    

}
