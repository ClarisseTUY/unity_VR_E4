using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.EditorTools;
using UnityEngine;

public class DoorManager : MonoBehaviour
{

    [Header("Door Animation")]
    [SerializeField] private Animator doorAnimator;
    [SerializeField] private string doorAnimatorName;
    [SerializeField] private InteractableObject door;

    [Header("Item Requirements")]
    [SerializeField] private InteractableObject requiredItem;
    [SerializeField] public bool itemRequired = false;

    [Header("Empty Inventory")]
    public bool needEmptyInventory;
    [SerializeField] public InventorySystem inventorySystem;
    [SerializeField] public EquipSystem equipSystem;


    [Header("UI")]
    [SerializeField] private TMP_Text messageText;
    public string infoText;


    // Start is called before the first frame update
    void Start()
    {
        if (messageText != null)
        {
            messageText.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    public void OpenDoor()
    {
        doorAnimator.SetTrigger(doorAnimatorName);
        //door.UpdateShowCommand(false);

    }

    public void ShowMessage(string message, float duration = 1.5f)
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
    public bool CheckIfCanOpen()
    {
        if (itemRequired)
        {
            string equippedItem = EquipSystem.Instance.GetEquippedItemName();

            if (equippedItem == requiredItem.GetItemName())
            {
                return true;

            }
        }
        else if(needEmptyInventory)
        {
            return inventorySystem.CheckIfEmpty() && equipSystem.CheckIfEmpty();
        }
        return false;
    }
    public void DoorLocked()
    {

    }

}
