using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public delegate void Action(Resource res);

    public enum Type { Air, Iron, Sand, Coal, Silicon,CarbonAlloy } //Air Ÿ���� �ƹ��� Ư¡�� ���� �⺻ ������. ��������θ� ��� ����

    [SerializeField]
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

    public void MoveCell(Vector2 dir, float speed, Action act)
    {
        StopAllCoroutines();
        StartCoroutine(MoveingCell(dir, speed, act));
    }

    IEnumerator MoveingCell(Vector2 dir,float speed,Action act)
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
        act?.Invoke(this);
    }
}
