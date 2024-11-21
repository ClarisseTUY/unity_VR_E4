using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem_0 : MonoBehaviour
{
    public static InventorySystem_0 current;
    private Dictionary<InventoryItemData, InventoryItem_0> m_itemDictionary;
    public List<InventoryItem_0> inventory { get; private set; }

    private void Awake()
    {
        current = this;
        inventory = new List<InventoryItem_0>();
        m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem_0>();
    }
    
    public void Add(InventoryItemData referenceData)
    {
        if(m_itemDictionary.TryGetValue(referenceData, out InventoryItem_0 value)) {
            value.AddToStack();
        }
        else
        {
            InventoryItem_0 newItem = new InventoryItem_0(referenceData);
            inventory.Add(newItem);
            m_itemDictionary.Add(referenceData, newItem);
        }
    }

    public void Remove(InventoryItemData referenceData)
    {
        if(m_itemDictionary.TryGetValue(referenceData, out InventoryItem_0 value))
        {
            value.RemoveFromStack();
            
            if(value.stackSize == 0)
            {
                inventory.Remove(value);
                m_itemDictionary.Remove(referenceData);
            }
        }
    }

    public InventoryItem_0 Get(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem_0 value))
        {
            return value;
        }
        return null;
    }
}
