using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryFramework
{

    [CreateAssetMenu(menuName = "Factory Framework/Data/Item")]
    [System.Serializable]
    public class Item : SerializeableScriptableObject
    {
        public Sprite icon;
        public GameObject prefab;
        public ItemData itemData;
        public Color DebugColor;
    }

    [System.Serializable]
    public struct ItemData
    {
        public string description;
        public int maxStack;
    }

    [System.Serializable]
    public struct ItemStack
    {
        [SerializeField]
        private Item _item;
        public Item item
        {
            get
            {
                return _item;
            }
            set 
            {
                _item = value;
                itemGUID = value.Guid;
            } 
        }
        public string itemGUID;
        public int amount;
        public bool IsFull { get { return item != null && amount >= item.itemData.maxStack; } }
    }
    [System.Serializable]
    public struct ItemOnBelt
    {
        public Item item;
        public float position;
        public Transform model;
        public float EndPos { get { return position - ConveyorLogisticsUtils.settings.BELT_SPACING; } }
    }


}