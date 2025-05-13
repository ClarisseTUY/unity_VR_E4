using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.EditorTools;


public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public bool isPickable;
    public bool isUnlockable;
    public bool showCommand;

    public string itemName;
    public string itemCommand;

    [SerializeField] private DoorManager doorManager;


    void Start()
    {
        showCommand = true;
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
        {
            if (isPickable)
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

            else if (isUnlockable)
            {
                if (doorManager.CheckIfCanOpen())
                {
                    doorManager.OpenDoor();
                }
                else if(showCommand)
                {
                    doorManager.ShowMessage(doorManager.infoText);
                }
            }

        }
    }


    public string GetItemName()
    {
        return itemName;
    }

    public string GetItemCommand()
    {
        return itemCommand;
    }

    public bool ShowCommand()
    {
        return showCommand;
    }

    public void UpdateShowCommand(bool value)
    {
        this.showCommand = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
