using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BlockData", menuName = "오브젝트 데이터/블럭 데이터", order = -1)]
public class BlockData : ScriptableObject
{
    [SerializeField] Resource.Type[] _costResource;
    [SerializeField] int[] _costCount;
    [SerializeField] GameObject _prefab;
    [SerializeField] Sprite _Image;

    public Resource.Type[] costResource { get; }
    public int[] costCount { get; }
    public GameObject prefab { get; }
    public Sprite Image { get; }
}
