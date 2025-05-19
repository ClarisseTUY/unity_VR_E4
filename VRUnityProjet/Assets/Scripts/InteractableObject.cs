using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.EditorTools;

using UnityEngine.InputSystem;

//using UnityEngine.InputSystem;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public bool isPickable;
    public bool isUnlockable;
    public bool showCommand;
    public bool handInRange;

    public string itemName;
    public string itemCommand;

    [SerializeField] private DoorManager doorManager;
    //private UnityEngine.XR.InputDevice rightHandDevice;

    private XRGrabInteractable grabInteractable;
    private bool isGrabbed = false;

    [Header("Input Actions")]
    public InputActionReference addToInventoryAction;


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
    


        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);

        /*var rightHandDevices = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);
        if (rightHandDevices.Count > 0)
        {
            rightHandDevice = rightHandDevices[0];
        }*/
    }
    private void OnEnable()
    {
        if (addToInventoryAction != null)
        {
            addToInventoryAction.action.performed += OnAddToInventoryPressed;
            addToInventoryAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (addToInventoryAction != null)
        {
            addToInventoryAction.action.performed -= OnAddToInventoryPressed;
            addToInventoryAction.action.Disable();
        }
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        /*isGrabbed = true;
        Debug.Log("Grabbed " + gameObject.name);

        if (addToInventoryAction != null)
        {
            addToInventoryAction.action.performed += OnAddToInventoryPressed;
        }*/

        isGrabbed = true;
        Debug.Log("Grabbed " + gameObject.name);

        if (addToInventoryAction != null)
        {
            // ATTENTION : pour �tre propre, on se d�sabonne d'abord avant de r�abonner
            //addToInventoryAction.action.performed -= OnAddToInventoryPressed;
            addToInventoryAction.action.performed += OnAddToInventoryPressed;
        }
    }

    private void OnDestroy()
    {
        // Petit filet de s�curit� : au cas o� l'objet est d�truit sans release propre
        if (addToInventoryAction != null)
        {
            addToInventoryAction.action.performed -= OnAddToInventoryPressed;
        }
    }

    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;
        Debug.Log("Released " + gameObject.name);
        if (addToInventoryAction != null)
        {
            addToInventoryAction.action.performed -= OnAddToInventoryPressed;
        }
    }

    private void OnAddToInventoryPressed(InputAction.CallbackContext context)
    {
        if (!isGrabbed)
        {
            Debug.Log("Tried to add to inventory but object not grabbed.");
            return;
        }

        if (!InventorySystem.Instance.CheckIfFull())
        {
            InventorySystem.Instance.AddToInventory(itemName);
            Debug.Log("Item added to inventory: " + itemName);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory is full");
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

    /*public string GetItemName()
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

    public void UpdateShowCommand(bool value)
    {
        this.showCommand = value;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
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
    }*/
}
