using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource
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
}
