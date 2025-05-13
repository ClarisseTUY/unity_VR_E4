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
    [SerializeField] private InteractableObject requiredItem;
    [SerializeField] public bool itemRequired = false;
    public bool isLocked;


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
        isLocked = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    public void OpenDoor()
    {
        isLocked = false;
        doorAnimator.SetTrigger(doorAnimatorName);
        door.UpdateShowCommand(false);

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
                isLocked = false;
                return true;

            }
        }
        return false;
    }
    public void DoorLocked()
    {

    }

}
