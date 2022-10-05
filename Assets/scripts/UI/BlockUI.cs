using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockUI : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject uiPrefab;
    void Start()
    {
        for (int n = 0; n < Blocks.Inst.BlockList.Count; n++)
        {
            GameObject ui = Instantiate(uiPrefab);
            ui.transform.parent = transform;
            ui.GetComponent<RectTransform>().localPosition = new Vector3(n*110, 0, 0);
            ui.GetComponent<setCurrentBlockUI>().Number = n;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
