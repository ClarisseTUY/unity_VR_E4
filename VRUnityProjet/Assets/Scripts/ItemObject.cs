using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public InventoryItemData referenceItem;

    public void OnHandlePickupItem()
    {
        InventorySystem_0.current.Add(referenceItem);
        Destroy(gameObject);
    }
}
