using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirUnit : Unit
{    
    Coroutine coMove = null;
    Coroutine coRot = null;

    public UnitStat myInfo;
    
    


    // Start is called before the first frame update
    void Start()
    {
        UnitManager.instance.UnitList.Add(gameObject);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartMoveToTarget(Vector3 target)
    {
        if (coMove != null)
        {
            StopCoroutine(coMove);
        }
        if (coRot != null)
        {
            StopCoroutine(coRot);
        }
        coMove =  StartCoroutine(MoveToPosition(target));
        coRot = StartCoroutine(RotateToTarget(target));
    }

    IEnumerator MoveToPosition(Vector3 target)
    {
        
        target.y = transform.position.y;
        Vector3 dir = target - transform.position;
        float dist = dir.magnitude;
        dir.Normalize();

        while (dist > 0)
        {
            float delta = Time.deltaTime * myInfo.MoveSpeed;
            if (dist < delta) delta = dist;
            transform.Translate(dir * delta, Space.World);
            dist -= delta;
            yield return null;
        }
    }

    IEnumerator RotateToTarget(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        dir.Normalize();
        float angle = Mathf.Acos(Vector3.Dot(dir,Vector3.forward)) * Mathf.Deg2Rad;
        float rotDir = Vector3.Dot(dir, transform.right) < 0.0f ? -1.0f : 1.0f;

        while (angle > Mathf.Epsilon)
        {
            float delta = Time.deltaTime * myInfo.RotateSpeed;
            if (angle < delta) delta = angle;
            transform.Rotate(Vector3.up * delta * rotDir,Space.World);
            angle -= delta;
            yield return null;
        }
    }

    
    

}
