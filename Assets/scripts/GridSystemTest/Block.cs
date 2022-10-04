using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Block : MonoBehaviour
{
    public int TeamCode = 0;
    public enum Type {None, Tower, Factory, Wall}
    public Vector2 CellSize = new Vector2(1, 1);
    public Type blockType = Type.None;
    public GameObject prefab;

    private void Start()
    {
        StartCoroutine(SetRigidBody());
    }

    IEnumerator SetRigidBody()
    {
        yield return new WaitForSeconds(2);
        GetComponent<Rigidbody>().isKinematic = true;
    }
}
