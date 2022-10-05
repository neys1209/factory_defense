using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : Block
{
    Block myBlock;
    private void Awake()
    {
        myBlock = GetComponent<Block>();
        myBlock.blockType = Type.Conveyor;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
