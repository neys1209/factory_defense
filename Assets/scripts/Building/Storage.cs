using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : Block
{
    private void Awake()
    {
        blockType = Type.Storage;
        rotatable = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        init();
    }
}
