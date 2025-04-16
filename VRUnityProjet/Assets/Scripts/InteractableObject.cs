using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public bool isPickable;

    public string itemName;
    public string itemCommand;

    public string GetItemName()
    {
        return itemName;
    }    
    public string GetItemCommand()
    {
        return itemCommand;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && isPickable && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
        {
            if (!InventorySystem.Instance.CheckIfFull())
            {
                InventorySystem.Instance.AddToInventory(itemName);

                Destroy(gameObject);
            }
            else
            {
                Debug.Log("inventory is full");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}