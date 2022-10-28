using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FDBlock
{
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

        private void Update()
        {
            foreach (var item in Inventory)
            {
                item.gameObject.SetActive(false);
            }
        }
    }
}