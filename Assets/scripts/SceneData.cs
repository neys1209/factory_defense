using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneData : MonoBehaviour
{

    public GameObject player;
    public Camera camera;
    public UnitManager unitManager;
    public List<GameObject> CurrentUnit = new List<GameObject>();

    public static SceneData instance;
    private void Awake()
    {
        instance = this;
    }
}
