using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ResourceList : MonoBehaviour
{
    static public ResourceList Inst;
    public List<Resource> list = new List<Resource>();
    public Dictionary<Resource.Type,GameObject> dictionary = new Dictionary<Resource.Type,GameObject>();

    private void Awake()
    {
        Inst = this;

        foreach (var res in list)
        {
            dictionary[res.type] = res.gameObject;
        }
    }
}
