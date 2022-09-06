using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Speed = 5.0f;
    bool isFire = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isFire) transform.Translate(Vector3.forward * Speed * Time.deltaTime);
    }

    public void OnFire()
    {
        isFire = true;
        //transform.parent  = null;
        transform.SetParent(null);
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
