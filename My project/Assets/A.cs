using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class A : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
        }
    }

}
