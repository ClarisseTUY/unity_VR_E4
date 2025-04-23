using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public bool handInRange;

    public string itemName;
    public string itemCommand;

    private UnityEngine.XR.InputDevice rightHandDevice;


    void Start()
    {
        var rightHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);
        if (rightHandDevices.Count > 0)
        {
            rightHandDevice = rightHandDevices[0];
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

    void Update()
    {
        bool buttonAPressed = false;
        if (rightHandDevice.isValid)
        {
            InputHelpers.IsPressed(rightHandDevice, InputHelpers.Button.PrimaryButton, out buttonAPressed);
        }

        if ((buttonAPressed && handInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject) ||
            ((Input.GetKeyDown(KeyCode.E) ) && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject))
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
            handInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            handInRange = false;
        }
    }
}