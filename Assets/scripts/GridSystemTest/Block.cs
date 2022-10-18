using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
[System.Serializable]
public class Block : MonoBehaviour
{
    #region ����
    public int TeamCode = 0;
    public enum Type { None, Turret, Factory, Wall, Conveyor }
    public Vector2 CellSize = new Vector2(1, 1);
    public Type blockType = Type.None;
    public GameObject prefab;

    static protected (int x, int y)[] _rotations = {(0, -1), (-1, 0), (0, 1), (1, 0) };
    static public (int x, int y)[] rotations { get => _rotations;}

    protected int BlockAngleIndex = 0;
    public (int x, int y) Rotation { get => _rotations[BlockAngleIndex]; }

    bool _rotatable = false;
    public bool rotatable { get => _rotatable; protected set => _rotatable = value; }

    public List<Resource> Inventory = new List<Resource>();
    [HideInInspector] public List<Cell> OnCell = new List<Cell>();

    public bool Activate = false;

    #endregion

    private void Start()
    {
        StartCoroutine(SetRigidBody());
    }

    protected IEnumerator SetRigidBody()
    {
        yield return new WaitForSeconds(2);
        GetComponent<Rigidbody>().isKinematic = true;
        Activate = true;
    }

    public void BlockRotate(int angle)
    {
        BlockAngleIndex += angle % rotations.Length;
        if (BlockAngleIndex < 0)
            BlockAngleIndex = rotations.Length - 1 - BlockAngleIndex;
        if (BlockAngleIndex >= rotations.Length-1)
            BlockAngleIndex %= rotations.Length;
        if (angle != 0)
        {
            SetRotation();
        }
    }
    public void SetRotation()
    {
        transform.rotation = Quaternion.identity;
        transform.Rotate(Vector3.up * 90.0f * BlockAngleIndex);            
    }

    public Vector2 VectorRotation()
    {
        return new Vector2(Rotation.x, Rotation.y);
    }

    public void Processing() //��ӹ��� Ŭ�������� ������ ��
    { }

    private void OnDestroy()
    {
        foreach (var i in Inventory)
        {
            Destroy(i.gameObject);
        }
    }

    [ContextMenu("ȸ��")]
    public void OneRotate()
    {
        BlockRotate(1);
    }
}
