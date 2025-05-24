using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmptyInventoryDoor : MonoBehaviour
{
    [SerializeField] private DoorManager doorManager;
    [SerializeField] public InventorySystem inventorySystem;
    private bool playerInRange = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (inventorySystem.CheckIfEmpty() && playerInRange)
        {
            doorManager.OpenDoor();
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
}
