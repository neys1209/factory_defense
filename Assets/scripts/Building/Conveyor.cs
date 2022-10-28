using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FDBlock
{

    public class Conveyor : Block
    {
        public enum Mod {Normal, Divider, filter}
        public Mod myMod = Mod.Normal;
        public Resource.Type filterType = Resource.Type.Air;

        Vector2 orgVector;
        


        private void Awake()
        {
            blockType = Type.Conveyor;
            rotatable = true;
        }
        // Start is called before the first frame update
        void Start()
        {
            init();
            orgVector = VectorRotation();
        }

        // Update is called once per frame
        void Update()
        {
            if (Activate)
            {

                Processing();
            }
        }

        void ModProcessing()
        {
            switch (myMod)
            { 
                case Mod.Normal:
                    break;
                case Mod.Divider:                    
                    do
                    {
                        OneRotate(false);
                    }
                    while (VectorRotation() == -orgVector);
                    break;
                case Mod.filter:
                    if (filterType != Resource.Type.Air && InventoryCount > 0 && Inventory[0].type != filterType)
                    {
                        do
                        {
                            OneRotate(false);
                        }
                        while (VectorRotation() == -orgVector || VectorRotation() == orgVector);
                    }
                    else
                    {
                        while (VectorRotation() != orgVector)
                        {
                            OneRotate(false);
                        }
                    }
                    break;
            }

        }


        new public void Processing()
        {

            if (InventoryCount != 0)
            {
                ModProcessing();
                foreach (var cell in OnCell)
                {
                    GameObject adjacentCells = GridSystem.Inst.getCellData(cell.GridSpacePostion + VectorRotation())?.GetData();
                    if (adjacentCells != null && adjacentCells.GetComponent<Block>().Activate)
                    {
                        Block cellblock = adjacentCells.GetComponent<Block>();
                        switch (cellblock.blockType)
                        {
                            case Type.Conveyor:
                                if (adjacentCells.GetComponent<Block>().InventoryCount == 0)
                                {
                                    OutputInventory(0)?.MoveCell(VectorRotation(), 2f, cellblock.InputInventory);
                                }
                                break;
                            case Type.Factory:
                                if (adjacentCells.GetComponent<Factory>().CanInputResource(Inventory[0].type))
                                {
                                    OutputInventory(0)?.MoveCell(VectorRotation(), 2f,
                                        adjacentCells.GetComponent<Factory>().InputNeedInventory);
                                }
                                break;
                            case Type.Storage:
                                OutputInventory(0)?.MoveCell(VectorRotation(), 2f, cellblock.InputInventory);
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
                    if (adjacentCells != null && adjacentCells != gameObject && adjacentCells.GetComponent<Block>().Activate && VectorRotation() == orgVector)
                    {
                        ModProcessing();
                        Block cellblock = adjacentCells.GetComponent<Block>();
                        switch (cellblock.blockType)
                        {
                            case Type.Factory:
                                if (cellblock.InventoryCount != 0)
                                {
                                    Resource res = cellblock.OutputInventory(0);
                                    res?.gameObject.SetActive(true);
                                    res?.MoveCell(VectorRotation(), 2f, InputInventory);

                                }
                                break;
                        }
                        switch (cellblock.blockType)
                        {
                            case Type.Drill:
                                if (cellblock.InventoryCount != 0)
                                {
                                    Resource res = cellblock.OutputInventory(0);
                                    res?.gameObject.SetActive(true);
                                    res?.MoveCell(VectorRotation(), 2f, InputInventory);
                                }
                                break;
                        }
                    }
                }
            }
        }
    }

}
