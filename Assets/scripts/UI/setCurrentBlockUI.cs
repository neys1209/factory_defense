using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class setCurrentBlockUI: MonoBehaviour
{

    public int Number;
    
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnButtonClick);
        GameObject obj = Instantiate(Blocks.Inst.BlockList[Number]);
        obj.GetComponent<Rigidbody>().isKinematic = true;
        obj.transform.parent = transform;
        obj.transform.position = Vector3.zero;
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = obj.transform.localScale * 6;
        obj.layer = LayerMask.NameToLayer("UI");
        if (obj.transform.childCount > 0)
            obj.transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("UI");
        obj.GetComponent<Block>().StopAllCoroutines();
        obj.GetComponent<Block>().Activate = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnButtonClick()
    {
        GridSystem.Inst.SetCurrentBlock(Blocks.Inst.BlockList[Number]);
    }
}

