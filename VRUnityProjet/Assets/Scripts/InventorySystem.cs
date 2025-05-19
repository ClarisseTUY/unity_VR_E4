using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.EventSystems;

public class InventorySystem : MonoBehaviour
{

    public static InventorySystem Instance { get; set; }
    public GameObject Canvas;
    public GameObject inventoryScreenUI;
    public GameObject ItemInfoUi;





    public List<GameObject> slotList = new List<GameObject>();

    public List<string> itemList = new List<string>();

    private GameObject itemToAdd;

    private GameObject whatSlotToEquip;

    private Coroutine closePopupCoroutine;

    public bool isOpen;

    //public bool isFull;

    //Pickup Popup
    public GameObject pickupAlert;
    public TMP_Text pickupName;
    public Image pickupImage;




    public InputActionReference toggleInventoryAction;
    public GameObject rightRayInteractor;
    public GameObject leftRayInteractor;

    private void OnEnable()
    {
        toggleInventoryAction.action.performed += ToggleInventory;
        toggleInventoryAction.action.Enable();
    }

    private void OnDisable()
    {
        toggleInventoryAction.action.performed -= ToggleInventory;
        toggleInventoryAction.action.Disable();
    }
    private void ToggleInventory(InputAction.CallbackContext context)
    {
        InventorySystem inventory = InventorySystem.Instance;

        if (inventory == null) return;

        if (!inventory.isOpen)
        {
            inventory.Canvas.SetActive(true);
            inventory.inventoryScreenUI.SetActive(true);
            inventory.isOpen = true;
            rightRayInteractor.SetActive(true);
            leftRayInteractor.SetActive(true);
        }
        else
        {
            inventory.Canvas.SetActive(false);
            inventory.inventoryScreenUI.SetActive(false);
            inventory.isOpen = false;
            rightRayInteractor.SetActive(false);
            leftRayInteractor.SetActive(false);
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    void Start()
    {
        isOpen = false;
        //isFull = false;

        PopulateSlotList();
        Cursor.visible = false;
    }

    private void PopulateSlotList()
    {
        foreach(Transform child in inventoryScreenUI.transform)
        {
            if (child.CompareTag("Slot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    }


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !isOpen)
        {
            //Debug.Log("i is pressed");

            Debug.Log("i is pressed");
            Canvas.SetActive(true);
            inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
            isOpen = true;

        }
        else if (Input.GetKeyDown(KeyCode.I) && isOpen)
        {
            Canvas.SetActive(false);
            inventoryScreenUI.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            isOpen = false;
        }


    }

    public void AddToInventory(string itemName)
    {

        whatSlotToEquip = FindNextEmptySlot();

        itemToAdd = Instantiate(Resources.Load<GameObject>(itemName),whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        RectTransform rt = itemToAdd.GetComponent<RectTransform>();
        if (rt != null)
        {
            
            rt.localScale = Vector3.one; // Reset l��chelle, au cas o�
        }



        itemList.Add(itemName);

        TriggerPickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);

        ReCalculateList();

    }


    void TriggerPickupPopUp(string itemName, Sprite itemSprite)
    {
        // Si une coroutine est d�j� en cours, on l'arr�te pour r�initialiser le popup
        if (closePopupCoroutine != null)
        {
            StopCoroutine(closePopupCoroutine);
        }

        // Afficher le popup avec le bon objet
        pickupAlert.SetActive(true);
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;

        // D�marre la coroutine pour fermer le popup apr�s un d�lai sp�cifi�
        closePopupCoroutine = StartCoroutine(ClosePickupPopUpAfterDelay(1f)); // Utilisez le d�lai souhait�
    }

    IEnumerator ClosePickupPopUpAfterDelay(float delay)
    {
        // Attend pendant le d�lai sp�cifi�
        yield return new WaitForSeconds(delay);

        // D�sactive le popup apr�s le d�lai
        pickupAlert.SetActive(false);
    }

    private GameObject FindNextEmptySlot()
    {
        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount == 0)
            {
                return slot;
            }
            
            
        }

        return new GameObject();
    }

    public bool CheckIfFull()
    {
        int counter = 0;

        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount > 0)
            {
                counter++;
            }

        }
        if (counter == 8)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public bool CheckIfEmpty()
    {
        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount > 0)
            {
                return false;
            }

        }
        return true;
    }

    public void RemoveItem(string nameToRemove)
    {
        for(var i=slotList.Count-1; i>=0; i--)
        {
            if (slotList[i].transform.childCount>0)
            {
                if (slotList[i].transform.GetChild(0).name == nameToRemove + "Clone")
                {
                    Destroy(slotList[i].transform.GetChild(0).gameObject) ;
                }
            }
        }
        ReCalculateList() ;
    }

    public void ReCalculateList()
    {
        itemList.Clear();

        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount>0)
            {
                string name = slot.transform.GetChild(0).name;
                string str2 = "(Clone)";
                string result = name.Replace(str2, "");


                itemList.Add(result);
            }
        }
    }




    /*
    public IEnumerator calculate()
    {
        yield return new WaitForSeconds(1f);

        InventorySystem.Instance.ReCalculateList();
        //RefreshNeededItems();
    }

    StartCoroutine(calculate());
    */
}