using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPos : MonoBehaviour
{   
   
    [SerializeField]
    private LayerMask layerMask;
    public bool towerBuild;

    [SerializeField]
    private GameObject towerObject { get; }

    private void Awake()
    {
       // transform.position = Vector3.zero;
    }

   

    private void OnTriggerEnter(Collider other)
    {
      
          towerBuild = false;
            Debug.Log("towerEnter");
        
    }

    private void OnTriggerExit(Collider other)
    {
        towerBuild = true;
        Debug.Log("towerExit");
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.R))
        {
          //   Vector2Int rotationOffset = towerTemplate.GetRotationOffset(dir);

          //  transform.rotation = Quaternion.Euler(0, towerTemplate.GetRotationAngle(dir), 0);
            //transform.Rotate(0, 90, 0);
          //  dir = TowerTemplate.GetNextDir(dir);
        }
             /*   

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
          
           // mathf.lerp 선형보간을 사용해서 그리드 이동시 부드럽게 이동
            transform.position = new Vector3(Mathf.Lerp(transform.position.x , Mathf.FloorToInt(hit.point.x) , 0.5f) , 
                hit.point.y, Mathf.Lerp(transform.position.z, Mathf.FloorToInt(hit.point.z) , 0.5f) );
              
        }
             */
        
    }
   
}
