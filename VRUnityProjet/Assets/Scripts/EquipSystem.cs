using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class EquipSystem : MonoBehaviour
{
    public static EquipSystem Instance { get; set; }

    // -- UI -- //
    public GameObject quickSlotsPanel;

    public List<GameObject> quickSlotsList = new List<GameObject>();
    public List<string> itemList = new List<string>();
    public GameObject numbersHolder;
    public int selectedNumber = -1;
    public GameObject selectedItem;

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


    private void Start()
    {
        PopulateSlotList();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectQuickSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectQuickSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectQuickSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectQuickSlot(4);
        }
    }

    void SelectQuickSlot(int number)
    {
        if (checkIfSlotIsFull(number))
        {
            if (selectedNumber != number)
            {
                selectedNumber = number;

                // Désélectionner l'item précédent
                if (selectedItem != null)
                {
                    InventoryItem itemComponent = selectedItem.GetComponent<InventoryItem>();
                    if (itemComponent != null)
                    {
                        itemComponent.isSelected = false;
                    }
                }

                // Récupérer l'item sélectionné
                selectedItem = GetSelectedItem(number);
                if (selectedItem == null)
                {
                    Debug.LogError($"No item found in slot {number}.");
                    return;
                }

                InventoryItem selectedComponent = selectedItem.GetComponent<InventoryItem>();
                if (selectedComponent != null)
                {
                    selectedComponent.isSelected = true;
                }

                // Mettre à jour l'UI des numéros
                foreach (Transform child in numbersHolder.transform)
                {
                    Transform textTransform = child.Find("Text");
                    if (textTransform != null)
                    {
                        Text textComponent = textTransform.GetComponent<Text>();
                        if (textComponent != null)
                        {
                            textComponent.color = Color.black;
                        }
                    }
                }

                // Changer la couleur du numéro sélectionné
                Transform slotTransform = numbersHolder.transform.Find("number" + number);
                if (slotTransform != null)
                {
                    Transform textTransform = slotTransform.Find("Text");
                    if (textTransform != null)
                    {
                        Text toBeChanged = textTransform.GetComponent<Text>();
                        if (toBeChanged != null)
                        {
                            toBeChanged.color = Color.red;
                        }
                    }
                }
            }
            else
            {
                // Désélectionner si on clique sur le même numéro
                selectedNumber = -1;

                if (selectedItem != null)
                {
                    InventoryItem itemComponent = selectedItem.GetComponent<InventoryItem>();
                    if (itemComponent != null)
                    {
                        itemComponent.isSelected = false;
                    }
                }

                foreach (Transform child in numbersHolder.transform)
                {
                    Transform textTransform = child.Find("Text");
                    if (textTransform != null)
                    {
                        Text textComponent = textTransform.GetComponent<Text>();
                        if (textComponent != null)
                        {
                            textComponent.color = Color.black;
                        }
                    }
                }
            }
        }
    }

    GameObject GetSelectedItem(int slotNumber)
    {
        return quickSlotsList[slotNumber-1].transform.GetChild(0).gameObject;
    }


    bool checkIfSlotIsFull(int slotNumber)
    {
        if (quickSlotsList[slotNumber-1].transform.childCount > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void PopulateSlotList()
    {
        foreach (Transform child in quickSlotsPanel.transform)
        {
            if (child.CompareTag("QuickSlot"))
            {
                quickSlotsList.Add(child.gameObject);
            }
        }
    }

    public void AddToQuickSlots(GameObject itemToEquip)
    {
        // Find next free slot
        GameObject availableSlot = FindNextEmptySlot();
        // Set transform of our object
        itemToEquip.transform.SetParent(availableSlot.transform, false);
        // Getting clean name
        string cleanName = itemToEquip.name.Replace("(Clone)", "");
        // Adding item to list
        itemList.Add(cleanName);

        InventorySystem.Instance.ReCalculateList();

    }


    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject();
    }

    public bool CheckIfFull()
    {

        int counter = 0;

        foreach (GameObject slot in quickSlotsList)
        {
            if (slot.transform.childCount > 0)
            {
                counter += 1;
            }
        }

        if (counter == 4)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

