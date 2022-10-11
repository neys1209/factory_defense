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
        
    }
}
