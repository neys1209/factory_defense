using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource
{
    public enum Type { Air,Iron, Sand, Coal, Clicon } //Air Ÿ���� �ƹ��� Ư¡�� ���� �⺻ ������. ��������θ� ��� ����
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
}
