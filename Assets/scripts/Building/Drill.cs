using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace FDBlock
{
    public class Drill : Block
    {

        float timer = 0.0f;
        public float deley = 0.0f;
        DrillRotate myDrill;
        private void Awake()
        {
            myDrill = GetComponentInChildren<DrillRotate>();
            blockType = Type.Drill;
            rotatable = false;
            StartCoroutine(rotateDrill());
        }
        // Start is called before the first frame update
        void Start()
        {
            init();
        }

        // Update is called once per frame
        void Update()
        {
            if (Activate)
            {
                Processing();
                timer -= Time.deltaTime;
            }

        }

        new public void Processing()
        {
            if (InventoryCount < 10 && timer <= 0.0f)
            {
                foreach (var cell in OnCell)
                {
                    if (cell.OnResourcetype != Resource.Type.Air)
                    {
                        timer = deley;
                        GameObject obj = Instantiate(ResourceList.Inst.dictionary[cell.OnResourcetype], transform.position, Quaternion.identity, ResourceList.Inst.transform);
                        obj.SetActive(false);
                        InputInventory(obj.GetComponent<Resource>());
                    }
                    if (timer == deley)
                        myDrill.Dig();
                }
            }
        }

        IEnumerator rotateDrill()
        {
            yield return new WaitUntil(() => Activate);
            foreach (Cell cell in OnCell)
            {
                if (cell.OnResourcetype != Resource.Type.Air)
                {
                    myDrill.OnRotate(720.0f);
                    yield break;
                }
            }
            
        }
    }
}