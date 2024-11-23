using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;

    public PlayerMovement playerMovement;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        playerMovement = FindObjectOfType<PlayerMovement>();

    }


    public void OnBeginDrag(PointerEventData eventData)
    {

        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = .6f;
        //So the ray cast will ignore the item itself.
        canvasGroup.blocksRaycasts = false;
        startPosition = transform.position;
        startParent = transform.parent;
        transform.SetParent(transform.root);
        itemBeingDragged = gameObject;

    }

    public void OnDrag(PointerEventData eventData)
    {
        //So the item will move with our mouse (at same speed)  and so it will be consistant if the canvas has a different scale (other then 1);
        rectTransform.anchoredPosition += eventData.delta;

    }



    public void OnEndDrag(PointerEventData eventData)
    {
        var tempItemReference = itemBeingDragged;

        itemBeingDragged = null;

        if (transform.parent == startParent || transform.parent == transform.root)
        {

            DropItemIntoTheWorld(tempItemReference);

            /**
            transform.position = startPosition;
            transform.SetParent(startParent);
            **/

        }
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }
        
    }
    private void DropItemIntoTheWorld(GameObject tempItemReference)
    {
        // Nom propre de l'objet
        string cleanName = tempItemReference.name.Split(new string[] { "(Clone)" }, System.StringSplitOptions.None)[0];

        // Charger et instancier l'objet
        GameObject item = Instantiate(Resources.Load<GameObject>(cleanName + "_Model"));

        // R�cup�rer la position et direction du joueur
        Vector3 dropSpawnPosition = playerMovement.GetPlayerPosition();
        Vector3 forwardDirection = playerMovement.GetPlayerForwardDirection();

        // R�cup�rer le CharacterController pour calculer la hauteur du milieu
        CharacterController playerController = playerMovement.GetComponent<CharacterController>();
        float playerMidHeight = 0f;

        if (playerController != null)
        {
            // Calculer la hauteur au milieu du CharacterController
            playerMidHeight = playerController.transform.position.y + playerController.center.y;
        }
        else
        {
            Debug.LogError("Le joueur n'a pas de CharacterController attach� !");
            return;
        }

        // Calculer la position initiale
        float dropDistance = 2.0f; // Distance devant le joueur
        Vector3 initialDropPosition = dropSpawnPosition + forwardDirection * dropDistance;
        initialDropPosition.y = playerMidHeight; // Fixer la hauteur initiale

        // V�rifier la hauteur du sol
        RaycastHit hit;
        float groundHeight = 0f;

        if (Physics.Raycast(initialDropPosition, Vector3.down, out hit, Mathf.Infinity))
        {
            groundHeight = hit.point.y;
        }
        else
        {
            Debug.LogError("Impossible de d�tecter le sol !");
            return;
        }

        // Calculer la position finale (au-dessus du sol)
        float heightAboveGround = 0.1f; // L�g�rement au-dessus du sol pour �viter les interf�rences
        Vector3 finalDropPosition = new Vector3(initialDropPosition.x, groundHeight + heightAboveGround, initialDropPosition.z);

        // Positionner l'objet
        item.transform.position = finalDropPosition;

        // Ajouter un Rigidbody et configurer les collisions
        Rigidbody rb = item.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        // Configurer la d�tection continue des collisions
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        // Limiter la vitesse de chute pour �viter les travers�es
        rb.maxDepenetrationVelocity = 10f; // Limite de vitesse d'interaction avec le sol

        // Supprimer la r�f�rence temporaire de l'objet d'origine
        Destroy(tempItemReference); // Utilisez Destroy au lieu de DestroyImmediate si ce n'est pas dans l'�diteur

        // Recalculer l'inventaire
        InventorySystem.Instance.ReCalculateList();

        // V�rifier constamment si l'objet traverse le sol
        StartCoroutine(MonitorItemPosition(item, groundHeight));
    }

    private IEnumerator MonitorItemPosition(GameObject item, float groundHeight)
    {
        Rigidbody rb = item.GetComponent<Rigidbody>();

        while (item != null)
        {
            // V�rifier si l'objet est sous le sol
            if (item.transform.position.y < groundHeight)
            {
                // Repositionner l'objet au-dessus du sol
                Vector3 correctedPosition = item.transform.position;
                correctedPosition.y = groundHeight + 0.1f; // Ajustement au-dessus du sol
                item.transform.position = correctedPosition;

                // R�initialiser la vitesse pour �viter de continuer � traverser
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                }
            }

            yield return null; // Attendre le prochain frame
        }
    }

}

