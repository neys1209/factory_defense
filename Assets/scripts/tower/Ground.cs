using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{

    // public bool IsBuildTower { get; set; }

    public bool IsBuildTower;
    private void Awake()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
       
         //   IsBuildTower = true;
            Debug.Log("Ground true");
        
    }

    private void OnCollisionStay(Collision collision)
    {
        
    }

    private void OnCollisionExit(Collision collision)
    {

      
           // IsBuildTower = false;
            Debug.Log("Ground false");
        

    }

    // Start is called before the first frame update
    void Start()
    {
        IsBuildTower = false;
        
    }
  
}
