using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{

    public static SelectionManager Instance { get; private set; }

    public bool onTarget;

    public GameObject selectedObject;



    public GameObject interaction_Info_name_UI;
    public GameObject interaction_Info_command_UI;
    public TMP_Text interaction_text_name;
    public TMP_Text interaction_text_command;

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
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