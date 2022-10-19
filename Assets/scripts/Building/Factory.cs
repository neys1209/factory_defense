using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : Block
{

    [SerializeField] FactoryData myData;
    int[] needInventory;

    float timer = 0.0f;

    private void Awake()
    {
        blockType = Type.Factory;
        rotatable = false;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        init();
        needInventory = new int[myData.NeedResource.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (Activate)
        {
            if (timer > 0)
                timer -= Time.deltaTime;
            else if (timer < 0) timer = 0.0f;
            Processing();
        }
    }

    public bool CanInputResource(Resource.Type res)
    {
        int index = Array.IndexOf(myData.NeedResource, res);
        if (index < 0)
        {
            return false;
        }
        if (myData.NeedResourceCount[index]*5 <= needInventory[index])
        {
            return false;
        }
        return true;
    }

    public void InputNeedInventory(Resource res)
    {
        int index = Array.IndexOf(myData.NeedResource, res.type);
        if (index >= 0)
        {
            needInventory[index] += res.count;
            Destroy(res.gameObject);
        }
        
    }

    new public void Processing()
    {
        if (timer == 0)
        {
            bool cheak = true;
            for (int i = 0; i < needInventory.Length; i++)
            {
                if (myData.NeedResourceCount[i] > needInventory[i]) cheak = false;
            }
            if (cheak && InventoryCount < 10)
            {
                for (int i = 0; i < needInventory.Length; i++)
                {
                    needInventory[i] -= myData.NeedResourceCount[i];
                }
                GameObject obj = Instantiate(myData.ReturnResource, transform.position, Quaternion.identity, ResourceList.Inst.transform) as GameObject;
                obj.SetActive(false);
                InputInventory(obj.GetComponent<Resource>()); 
            }
            timer = myData.Deley;
        }
        

    }
}
