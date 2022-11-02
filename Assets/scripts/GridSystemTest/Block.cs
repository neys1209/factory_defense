using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace FDBlock
{

    [SelectionBase]
    [System.Serializable]
    public class Block : MonoBehaviour
    {
        #region 변수
        public int TeamCode = 0;
        public enum Type { None, Turret, Factory, Wall, Conveyor, Storage, Drill,EnemyBase }
        public Vector2 CellSize = new Vector2(1, 1);
        public Type blockType = Type.None;
        //public GameObject prefab;
        public int InventoryCount { get; private set; }

        static protected (int x, int y)[] _rotations = { (0, -1), (-1, 0), (0, 1), (1, 0) };
        static public (int x, int y)[] rotations { get => _rotations; }

        protected int BlockAngleIndex = 0;
        public (int x, int y) Rotation { get => _rotations[BlockAngleIndex]; }

        bool _rotatable = false;
        public bool rotatable { get => _rotatable; protected set => _rotatable = value; }

        [HideInInspector] public List<Resource> Inventory = new List<Resource>();
        [HideInInspector] public List<Cell> OnCell = new List<Cell>();

        [HideInInspector] public bool Activate = false;
        public bool Unbreakable = false;
        #endregion

        private void Start()
        {
            init();
        }
        public void init()
        {
            StartCoroutine(SetRigidBody());
            InventoryCount = 0;
        }

        protected IEnumerator SetRigidBody()
        {
            yield return new WaitForSeconds(2);
            GetComponent<Rigidbody>().isKinematic = true;
            Activate = true;
            EffectMenager.Inst.EffectPlay(EffectMenager.EffectType.Put, transform.position);
        }

        public void BlockRotate(int angle, bool real=true)
        {
            BlockAngleIndex += angle % rotations.Length;
            if (BlockAngleIndex < 0)
                BlockAngleIndex = rotations.Length - 1 - BlockAngleIndex;
            if (BlockAngleIndex >= rotations.Length - 1)
                BlockAngleIndex %= rotations.Length;
            if (angle != 0 && real)
            {
                SetRotation();
            }
        }
        public void SetRotation()
        {
            transform.rotation = Quaternion.identity;
            transform.Rotate(Vector3.up * 90.0f * BlockAngleIndex);
        }

        public void SetRotation(int index)
        {
            BlockAngleIndex = index;
            SetRotation();
        }

        public int GetRotationIndex()
        {
            return BlockAngleIndex;
        }

        public Vector2 VectorRotation()
        {
            return new Vector2(Rotation.x, Rotation.y);
        }
        public void Processing() //��ӹ��� Ŭ�������� ������ ��
        { }

        public void DestoryMyself()
        {
            foreach (var i in Inventory)
            {
                if (i.gameObject != null)
                    DestroyImmediate(i.gameObject);
            }
        }

        [ContextMenu("ȸ��")]
        public void OneRotate(bool real = true)
        {
            BlockRotate(1,real);
        }

        public void InputInventory(Resource res)
        {
            Resource find = Inventory.Find(x => x.type == res.type);
            if (find != null)
            {
                find.count++;
                Destroy(res.gameObject);
            }
            else
            {
                Inventory.Add(res);
            }
            if (blockType == Type.Conveyor)
            {
                foreach (var item in Inventory)
                {
                    item.transform.position = transform.position + Vector3.up * 0.1f;
                    if (item == null)
                    {
                        Inventory.Remove(item);
                        InventoryCount--;
                    }
                }
            }
            InventoryCount++;
        }

        public Resource OutputInventory(int index)
        {
            if (index < 0 || Inventory.Count < index - 1) return null;
            if (Inventory[index] == null)
            {
                Inventory.RemoveAt(index);
                InventoryCount--;
                return null;
            }
            InventoryCount--;
            if (Inventory[index].count == 1)
            {
                Resource ret = Inventory[index];
                Inventory.RemoveAt(index);
                return ret;

            }
            else
            {
                Inventory[index].count--;
                GameObject obj = Instantiate(Inventory[index].gameObject, transform.position, Quaternion.identity);
                Resource ret = obj.GetComponent<Resource>();
                ret.count = 1;
                return ret;
            }
        }
        public Resource OutputInventory(Resource.Type restype)
        {
            int index = Inventory.FindIndex(x => x.type == restype);
            if (index < 0) return null;
            InventoryCount--;
            if (Inventory[index].count == 1)
            {
                Resource ret = Inventory[index];
                Inventory.RemoveAt(index);
                return ret;

            }
            else
            {
                Inventory[index].count--;
                GameObject obj = Instantiate(Inventory[index].gameObject, transform.position, Quaternion.identity, ResourceList.Inst.transform);
                Resource ret = obj.GetComponent<Resource>();
                ret.count = 1;
                return ret;
            }
        }

    }
}
