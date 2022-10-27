using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beam : MonoBehaviour
{
    public GameObject target;
    public LayerMask layerMask;

       
    // Start is called before the first frame update
    void Start()
    {
       
    }
    private void OnCollisionEnter(Collision collision)
    {
        
    }
    
    public void Target(GameObject transform)
    {
        target = transform;
    }

    void MoveTarget()
    {
        if (target != null)
        {
            Vector3 dir = (target.transform.position + new Vector3(0, 1, 0)) - transform.position;
            transform.forward = dir;
            transform.position += dir.normalized * Time.deltaTime * 50f;
            transform.localRotation =
                    Quaternion.Slerp(transform.localRotation, Quaternion.LookRotation(dir), 5 * Time.deltaTime);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((layerMask & 1 << other.gameObject.layer) != 0)
        {          
            Destroy(gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {
        MoveTarget();
    }

    
}
