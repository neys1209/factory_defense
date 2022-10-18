using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="FactoryData",menuName = "오브젝트 데이터/공장 데이터", order = -1)]
public class FactoryData : ScriptableObject
{
    [SerializeField] GameObject _ReturnResource;
    [SerializeField] int _ReturnCount;
    [SerializeField] float _Deley;
    [Space(10)]
    [SerializeField] Resource.Type[] _NeedResource;
    [SerializeField] int[] _NeedResourceCount;

    public GameObject ReturnResource { get => ReturnResource; }
    public int ReturnCount { get => ReturnCount; }
    public float Deley { get => Deley; }

    public Resource.Type[] NeedResource { get => NeedResource; }
    public int[] NeedResourceCount { get => NeedResourceCount; }



}
