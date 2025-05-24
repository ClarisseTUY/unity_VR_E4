using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class VRInterectbleObject : MonoBehaviour
{
    public string itemName;
    public string itemCommand;

    private XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;

    [Header("Input Actions")]
    public InputActionReference addToInventoryAction; // Assignable dans l’inspecteur

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnGrab);
        grabInteractable.selectExited.RemoveListener(OnRelease);
        RemoveInputListener(); // Sécurité
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
        AddInputListener();
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        RemoveInputListener();
    }

    private void AddInputListener()
    {
        if (addToInventoryAction != null)
        {
            addToInventoryAction.action.performed -= OnAddToInventoryPressed; // Empêche double
            addToInventoryAction.action.performed += OnAddToInventoryPressed;
            addToInventoryAction.action.Enable();
        }
    }

    private void RemoveInputListener()
    {
        if (addToInventoryAction != null)
        {
            addToInventoryAction.action.performed -= OnAddToInventoryPressed;
            addToInventoryAction.action.Disable();
        }
    }

    private void OnAddToInventoryPressed(InputAction.CallbackContext context)
    {
        if (!isGrabbed)
            return;

        if (!InventorySystem.Instance.CheckIfFull())
        {
            InventorySystem.Instance.AddToInventory(itemName);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory is full");
        }
    }

    public string GetItemName() => itemName;
    public string GetItemCommand() => itemCommand;
}
