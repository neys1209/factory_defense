using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceList : MonoBehaviour
{
    static public ResourceList Inst;
    public List<Resource> list = new List<Resource>();

    private void Awake()
    {
        Inst = this;
    }
}
