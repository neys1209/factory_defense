using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Moving(transform.forward, 100));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Moving(Vector3 Dir, float Dist)

    {

        while (Dist > Mathf.Epsilon)

        {

            float delta = Time.deltaTime * 3.0f;

            if (Dist < delta)

            {

                delta = Dist;

            }

            Dist -= delta;

            this.transform.Translate(Dir * delta, Space.World);

            yield return null;

        }

    }

    IEnumerator Rotate2Target(Vector3 Target)
    {
        Vector3 Dir = (Target - this.transform.position).normalized;

        float d = Vector3.Dot(Dir, this.transform.forward);

        float angle = Mathf.Acos(d) * 180.0f / Mathf.PI;

        float rotDir = 1.0f;
        if (Vector3.Dot(this.transform.right, Dir) < 0.0f)

        {
            rotDir = -1.0f;
        }

        yield return null;
    }
}
