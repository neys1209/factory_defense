using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EffectMenager : MonoBehaviour
{
    public enum EffectType { Smoke, Drill,Put }
    Dictionary<EffectType,GameObject> Effects = new Dictionary<EffectType, GameObject>();

    [SerializeField] List<EffectType> EffectTagList = new List<EffectType>();
    [SerializeField] List<GameObject> EffectGOList = new List<GameObject>();

    public static EffectMenager Inst;

    public void Awake()
    {
        Inst = this;
        for (int i = 0; i < EffectTagList.Count; i++)
        {
            Effects[EffectTagList[i]] = EffectGOList[i];
        }
    }

    public void EffectPlay(EffectType t,Vector3 pos)
    {
        Instantiate(Effects[t],pos,Quaternion.identity,transform);
    }

}
