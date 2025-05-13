using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickableUiActivator : MonoBehaviour
{
    [Header("UI affichée lors du clic")]
    public GameObject uiOnClick;

    [Header("UI affichée lors du survol")]
    public GameObject uiOnHover;

    private bool uiClickActive = false;
    private bool isHovered = false;

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool rayHit = Physics.Raycast(ray, out hit);
        bool hoveringThis = rayHit && hit.collider.gameObject == gameObject;
        bool clicked = Input.GetMouseButtonDown(0);

        // --- Gestion du survol ---
        if (hoveringThis)
        {
            if (!isHovered)
            {
                isHovered = true;
                if (uiOnHover != null)
                {
                    uiOnHover.SetActive(true);
                    Debug.Log("Survol de l'objet : " + gameObject.name);
                }
            }

            // Clic sur l’objet
            if (clicked)
            {
                if (uiOnClick != null)
                {
                    uiOnClick.SetActive(true);
                    uiClickActive = true;
                    Debug.Log("UI clic activée : " + gameObject.name);
                }
            }
        }
        else
        {
            if (isHovered)
            {
                isHovered = false;
                if (uiOnHover != null)
                {
                    uiOnHover.SetActive(false);
                    Debug.Log("Fin du survol de l'objet : " + gameObject.name);
                }
            }

            // Si on clique ailleurs → désactiver l'UI de clic
            if (clicked && uiClickActive)
            {
                if (uiOnClick != null)
                {
                    uiOnClick.SetActive(false);
                    uiClickActive = false;
                    Debug.Log("UI clic désactivée (clic hors de l'objet).");
                }
            }
        }
    }
}
