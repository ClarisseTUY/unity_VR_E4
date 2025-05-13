using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectHover : MonoBehaviour
{
    public GameObject uiPanel;     // UI à afficher
    public Light targetLight;      // Lumière à faire tourner
    private bool isHovered = false;
    private bool followRotation = false;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        bool rayHit = Physics.Raycast(ray, out hit);
        bool clicked = Input.GetMouseButtonDown(0);

        // --- Gestion du survol ---
        if (rayHit && hit.collider.gameObject == gameObject)
        {
            if (!isHovered)
            {
                isHovered = true;
                uiPanel.SetActive(true);
                Debug.Log("Survol de l'objet.");
            }

            if (clicked)
            {
                followRotation = !followRotation;
                Debug.Log(followRotation ? "Suivi ACTIVÉ." : "Suivi DÉSACTIVÉ.");
            }
        }
        else
        {
            if (isHovered)
            {
                isHovered = false;
                uiPanel.SetActive(false);
                Debug.Log("Sortie du survol.");
            }

            // Si clic ailleurs que sur l'objet → désactive le suivi
            if (clicked && followRotation)
            {
                followRotation = false;
                Debug.Log("Clic hors de l'objet : suivi DÉSACTIVÉ.");
            }
        }

        // --- Appliquer rotation de la lumière ---
        if (followRotation && targetLight != null)
        {
            Vector3 cameraEuler = Camera.main.transform.eulerAngles;
            targetLight.transform.rotation = Quaternion.Euler(cameraEuler.x, cameraEuler.y, 0f);
            Debug.Log("Rotation de la lumière synchronisée (X, Y).");
        }
    }
}
