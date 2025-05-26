using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectHoverVR : MonoBehaviour
{
    public GameObject uiPanel;                  // UI à afficher
    public Light targetLight;                   // Lumière à faire tourner
    public InputActionReference MoveLight;      // Action input pour activer le suivi

    private bool isHovered = false;
    private bool playerInZone = false;
    private bool followRotation = false;

    void OnEnable()
    {
        if (MoveLight != null)
            MoveLight.action.Enable();
    }

    void OnDisable()
    {
        if (MoveLight != null)
            MoveLight.action.Disable();
    }

    void Update()
    {
        if (!playerInZone)
            return;

        // Bouton pressé cette frame ?
        bool clicked = MoveLight != null && MoveLight.action.WasPerformedThisFrame();

        if (clicked)
        {
            followRotation = !followRotation;
            Debug.Log(followRotation ? "Suivi activé." : "Suivi désactivé.");
        }

        if (followRotation && targetLight != null)
        {
            Vector3 cameraEuler = Camera.main.transform.eulerAngles;
            targetLight.transform.rotation = Quaternion.Euler(cameraEuler.x, cameraEuler.y, 0f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Ou autre tag pour le joueur
        {
            playerInZone = true;
            if (uiPanel != null)
                uiPanel.SetActive(true);
            Debug.Log("Entrée dans la zone de contrôle.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            followRotation = false;

            if (uiPanel != null)
                uiPanel.SetActive(false);

            Debug.Log("Sortie de la zone de contrôle.");
        }
    }
}
