using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public LayerMask layerMask;
  //  public Player playerMouse;
    public bool towerBuild ;



   // private Ground ownerTile;  // 현재 타워가 배치되어 있는 타일
    private List<Vector3Int> gridPosList;
    
    
    private Ground[] tiles;


    public void Setup(Ground[] tiles , List<Vector3Int> gridPosList)
    {
        // this.ownerTile = ownerTile;
        this.gridPosList = gridPosList;
        this.tiles = tiles;
    }    

    public void Sell()
    {
        foreach (Vector3Int gridPosition in gridPosList)
        {

            foreach (Ground tilee in tiles)
            {
                Vector3 pos = new Vector3(Mathf.FloorToInt(tilee.transform.position.x), Mathf.FloorToInt(tilee.transform.position.y), Mathf.FloorToInt(tilee.transform.position.z));


                if (gridPosition == pos)
                {
                     Debug.Log(gridPosition);
                     Debug.Log(pos);
                    tilee.IsBuildTower = false;
                }
            }
        }
       // ownerTile.IsBuildTower = false;
         Destroy(transform.parent.gameObject); // Create Empty 로 만든거를 Empty까지 삭제하기
         // Destroy(gameObject);
    }
    private void Awake()
    {
      
        towerBuild = true;
    }
   
   
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Tower"))
        {
            towerBuild = false;
            Debug.Log(towerBuild);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Tower"))
        {
            towerBuild = true;
            Debug.Log(towerBuild);
        }

    }
    
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            collision.gameObject.GetComponent<Ground>().IsBuildTower = true;
           
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        
         collision.gameObject.GetComponent<Ground>().IsBuildTower = false;            
       
    }
    */
    // Update is called once per frame
    void Update()
    {
        
    }
}
