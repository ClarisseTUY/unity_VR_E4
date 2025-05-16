using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SelectionManager : MonoBehaviour
{

    public static SelectionManager Instance { get; private set; }

    public bool onTarget;

    public GameObject selectedObject;



    public GameObject interaction_Info_name_UI;
    public GameObject interaction_Info_command_UI;
    public TMP_Text interaction_text_name;
    public TMP_Text interaction_text_command;

    //public XRController controller; // À assigner dans l'inspecteur (Main Hand Controller, ex: RightHand Controller)
    public Transform rayOrigin;     // Le point de départ du rayon (ex: un empty object enfant du contrôleur)

    public float rayDistance = 10f;
    public LayerMask interactableLayer;

    private void Start()
    {
        onTarget = false;
        interaction_text_name = interaction_Info_name_UI.GetComponent<TMP_Text>();
        interaction_text_command = interaction_Info_command_UI.GetComponent<TMP_Text>();
    }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        /*Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layerMask = ~(1 << LayerMask.NameToLayer("PlayerLayer"));
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            var selectionTransform = hit.transform;

            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();

            if (interactable && interactable.playerInRange && interactable.ShowCommand())
            {
                onTarget = true;    
                selectedObject = interactable.gameObject;

                interaction_text_name.text = interactable.GetItemName();
                interaction_text_command.text = interactable.GetItemCommand();
                EnableSelection();
            }
            else
            {
                onTarget = false;
                DisableSelection();
            }

        }
        else
        {

            onTarget= false;
            DisableSelection();
        }*/

        Ray ray;

#if UNITY_ANDROID && !UNITY_EDITOR
        // --- MODE VR (Meta Quest) ---
        ray = new Ray(rayOrigin.position, rayOrigin.forward);
#else
        // --- MODE PC ---
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
#endif

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayDistance, interactableLayer))
        {
            var selectionTransform = hit.transform;
            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();

            if (interactable && interactable.playerInRange)
            {
                onTarget = true;
                selectedObject = interactable.gameObject;

                interaction_text_name.text = interactable.GetItemName();
                interaction_text_command.text = interactable.GetItemCommand();
                EnableSelection();
            }
            else
            {
                onTarget = false;
                DisableSelection();
            }
        }
        else
        {
            onTarget = false;
            DisableSelection();
        }
    }

    public void DisableSelection()
    {
        interaction_Info_name_UI.SetActive(false);
        interaction_Info_command_UI.SetActive(false);
        selectedObject = null;
    }
    public void EnableSelection()
    {
        interaction_Info_name_UI.SetActive(true);
        interaction_Info_command_UI.SetActive(true);
    }
}