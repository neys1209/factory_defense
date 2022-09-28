using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour
{

    public static Blocks Inst;
    [SerializeField]
    public List<GameObject> BlockList = new List<GameObject>();
    private void Awake()
    {
        Inst = this;
    }
}
