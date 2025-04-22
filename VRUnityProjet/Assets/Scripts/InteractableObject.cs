using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public bool isPickable;
    public bool showCommand;

    public string itemName;
    public string itemCommand;

    [Header("Door Animation")]
    [SerializeField] private Animator doorAnimator;

    [Header("UI")]
    [SerializeField]
    private TMP_Text messageText;

    public bool isUnlockable;              
    public string requiredItem;
    public string infoText;

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
    void Start()
    {
        if (messageText != null)
        {
            messageText.text = "";
        }
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
                string equippedItem = EquipSystem.Instance.GetEquippedItemName();

                if (equippedItem == requiredItem)
                {
                    InteractWithObject();
                }
                else
                {
                    if (showCommand)
                    {
                        ShowMessage(infoText);
                    }

                }
            }
        }
    }

    private void InteractWithObject()
    {
        if (doorAnimator != null)
        {
            doorAnimator.SetTrigger("OpenDoor2");
            Debug.Log("Porte en train de s'ouvrir...");
            UpdateShowCommand(false);
        }
        else
        {
            Debug.LogWarning("Aucun Animator assigné à la porte !");
        }
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

    private void ShowMessage(string message, float duration = 1.5f)
    {
        if (messageText == null) return;
        if (!messageText.gameObject.activeSelf)
        {
            messageText.gameObject.SetActive(true);
        }
        messageText.text = message;
        CancelInvoke(nameof(ClearMessage));
        Invoke(nameof(ClearMessage), duration);
    }

    private void ClearMessage()
    {
        if (messageText != null)
            messageText.text = "";
    }

}
