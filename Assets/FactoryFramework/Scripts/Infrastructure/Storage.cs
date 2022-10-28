using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FactoryFramework
{
    public class Storage : Building, IInput, IOutput
    {
        public StorageData data;

        private void Awake()
        {
            data.storage = new ItemStack[data.capacity];
        }

        public bool CanTakeInput(Item item)
        {
            if (item == null) return false;
            foreach (ItemStack stack in data.storage)
            {
                if (stack.item == item && stack.amount < item.itemData.maxStack) return true;
                if (stack.item == null) return true;
            }
            return false;
        }
        public void TakeInput(Item item)
        {
            for (int s = 0; s < data.storage.Length; s++)
            {
                ItemStack stack = data.storage[s];
                if (stack.item == item && stack.amount < item.itemData.maxStack)
                {
                    stack.amount += 1;
                    data.storage[s] = stack;
                    return;
                }
                if (stack.item == null)
                {
                    stack.item = item;
                    stack.amount = 1;
                    data.storage[s] = stack;
                    return;
                }
            }
        }

        public bool CanGiveOutput(Item filter = null)
        {
            foreach (ItemStack stack in data.storage)
            {
                if ((filter != null && stack.item == filter || stack.item != null) && stack.amount > 0) return true;
            }
            return false;
        }
        public Item OutputType()
        {
            foreach (ItemStack stack in data.storage)
            {
                if (stack.item == null && stack.amount > 0) return stack.item;
            }
            return null;
        }
        public Item GiveOutput(Item filter = null)
        {
            for (int s = 0; s < data.storage.Length; s++)
            {
                ItemStack stack = data.storage[s];
                if ((filter != null && stack.item == filter || stack.item != null) && stack.amount > 0)
                {
                    stack.amount -= 1;
                    Item item = stack.item;
                    if (stack.amount == 0)
                        stack.item = null;
                    data.storage[s] = stack;
                    return item;
                }
            }
            return null;
        }
    }
    [System.Serializable]
    public struct StorageData
    {
        public string guid;
        public int capacity;
        public ItemStack[] storage;
    }
}