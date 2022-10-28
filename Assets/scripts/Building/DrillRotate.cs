using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrillRotate : MonoBehaviour
{

    bool isRotate = false;
    public float Speed = 720.0f;
    public float CurSpeed = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isRotate)
        {
            CurSpeed = Mathf.Lerp(CurSpeed, Speed, Time.deltaTime * 0.5f);
            transform.Rotate(Vector3.up * CurSpeed * Time.deltaTime);
        }
    }

    public void OnRotate(float speed=180.0f)
    {
        Speed = speed;
        CurSpeed = 0.0f;
        isRotate = true;
    }

    public void StopRotate()
    {
        isRotate = false;
    }

    public void Dig()
    {
        StopAllCoroutines();
        StartCoroutine(Digging());
    }

    IEnumerator Digging()
    {
        transform.localPosition = Vector3.zero;
        transform.Translate(Vector3.down * 0.3f);
        while (transform.localPosition != Vector3.zero)
        {
            transform.localPosition = Vector3.Slerp(transform.localPosition, Vector3.zero,Time.deltaTime * 3.0f);
            yield return null;
        }
        transform.localPosition = Vector3.zero;
    }
    
}
