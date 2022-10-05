using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Block : MonoBehaviour
{
    public int TeamCode = 0;
    public enum Type { None, Turret, Factory, Wall, Conveyor }
    public Vector2 CellSize = new Vector2(1, 1);
    public Type blockType = Type.None;
    public GameObject prefab;

    protected (int x, int y)[] Rotations = { (1, 0), (0, -1), (-1, 0), (0, 1) };
    public int BlockAngleIndex = 0;
    public (int x, int y) Rotation
    {
        get => Rotations[BlockAngleIndex];
    }

    private void Start()
    {
        StartCoroutine(SetRigidBody());
    }

    IEnumerator SetRigidBody()
    {
        yield return new WaitForSeconds(2);
        GetComponent<Rigidbody>().isKinematic = true;
    }

    public void BlockRotate(int angle)
    {
        BlockAngleIndex += angle % Rotations.Length;
        if (BlockAngleIndex < 0)
            BlockAngleIndex = Rotations.Length - 1 - BlockAngleIndex ;
        if (BlockAngleIndex >= Rotations.Length-1)
            BlockAngleIndex %= Rotations.Length;
    }

    
}
