using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B : MonoBehaviour
{
    private void Start()
    {
        CreateA();
    }

    void CreateA()
    {
        GameObject obj = Instantiate(Resources.Load("Prefabs/A")) as GameObject;
        obj.transform.position = Vector3.zero;
        
    }
}
