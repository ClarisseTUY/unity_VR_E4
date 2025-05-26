using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEditor.EditorTools;

using UnityEngine.InputSystem;


using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class KeyDoor : MonoBehaviour
{

    public bool showCommand;


    public string itemName;
    public string itemCommand;
    private bool isGrabbed = false;

    [SerializeField] private DoorManager doorManager;

    private XRGrabInteractable grabInteractable;

    [Header("Input Actions")]
    public InputActionReference OpenDoorAction;


    //void Start()
    //{
    //    showCommand = true;
    //}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {
            Debug.Log("Clé a touché la porte !");
            doorManager.OpenDoor();
            Destroy(collision.gameObject);
            //Destroy(gameObject); //a décommenter quand on aura une serrure

        }
    }
    /*
    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        if (grabInteractable.CompareTag("Key") && OpenDoorAction && SerrureInRange())
        {


            if (doorManager.CheckIfCanOpen())
            {
                doorManager.OpenDoor();
                Destroy(gameObject);
            }
            else if (showCommand)
            {
                doorManager.ShowMessage(doorManager.infoText);
            }
        }

        grabInteractable = GetComponent<XRGrabInteractable>();

        grabInteractable.selectEntered.AddListener(OnGrab);

    }

    bool SerrureInRange()
    {
        //if
        return false;
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        isGrabbed = true;
    }
    private void OnRelease(SelectExitEventArgs args)
    {
        isGrabbed = false;

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
    }

    public string GetItemName()
    {
        return itemName;
    }

    public string GetItemCommand()
    {
        return itemCommand;
    }*/

}
