using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public List<GameObject> UnitList = new List<GameObject>();
    public static UnitManager instance;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
}
