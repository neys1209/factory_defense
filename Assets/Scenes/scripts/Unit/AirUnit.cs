using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirUnit : Unit
{


    
    
    Coroutine coMove = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMoveToTarget(Vector3 target)
    {
        if (coMove != null) StopCoroutine(coMove);
        coMove =  StartCoroutine(MoveToTarget(target));
    }

    new IEnumerator MoveToTarget(Vector3 target)
    {
        target.y = transform.position.y;
        Vector3 dir = target - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();

        Vector3 smoothMove = Vector3.zero;

        while (dist > 0)
        {
            float delta = Time.deltaTime * stat.MoveSpeed;
            if (dist < delta) delta = dist;
            smoothMove = Vector3.Lerp(smoothMove, dir * delta,Time.deltaTime*10);
            transform.Translate(smoothMove);
            dist -= smoothMove.magnitude;
            yield return null;
        }
        
    }

}
