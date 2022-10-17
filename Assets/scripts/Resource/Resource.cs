using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public enum Type { Air,Iron, Sand, Coal, Clicon } //Air 타입은 아무런 특징이 없는 기본 아이템. 실험용으로만 사용 가능
    Type _type = Type.Air;
    public Type type
    {
        get => _type;
        set => _type = value;
    }
    int _count = 1;
    public int count
    {
        get => _count;
        set => _count = value;
    }

    public void MoveCell(Vector2 dir, float speed, List<Resource> list)
    {
        StopAllCoroutines();
        StartCoroutine(MoveingCell(dir, speed, list));
    }

    IEnumerator MoveingCell(Vector2 dir,float speed,List<Resource> list)
    {
        float dist = 1;
        Vector3 movedir = new Vector3(dir.x,0, dir.y);
        movedir.Normalize();
        while (dist > 0)
        {
            float delta = speed * Time.deltaTime;
            if (delta > dist) delta = dist;
            dist -= delta;
            transform.Translate(movedir*delta);
            yield return null;
        }
        if (list != null)
            list.Add(this);
        else
            Destroy(gameObject);
    }
}
