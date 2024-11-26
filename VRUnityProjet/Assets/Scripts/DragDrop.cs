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
        string cleanName = tempItemReference.name.Split(new string[] { "(Clone)" }, System.StringSplitOptions.None)[0];

        GameObject item = Instantiate(Resources.Load<GameObject>(cleanName + "_Model"));

        Vector3 dropSpawnPosition = playerMovement.GetPlayerPosition();
        Vector3 forwardDirection = playerMovement.GetPlayerForwardDirection();

        CharacterController playerController = playerMovement.GetComponent<CharacterController>();
        float playerMidHeight = 0f;

        if (playerController != null)
        {
            playerMidHeight = playerController.transform.position.y + playerController.center.y;
        }
        else
        {
            Debug.LogError("Le joueur n'a pas de CharacterController attaché !");
            return;
        }

        float dropDistance = 1.5f; 
        Vector3 initialDropPosition = dropSpawnPosition + forwardDirection * dropDistance;
        initialDropPosition.y = playerMidHeight;

        RaycastHit hit;
        float groundHeight = 0f;

        if (Physics.Raycast(initialDropPosition, Vector3.down, out hit, Mathf.Infinity))
        {
            groundHeight = hit.point.y;
        }
        else
        {
            Debug.LogError("Impossible de détecter le sol !");
            return;
        }

        float heightAboveGround = 0.1f; 
        Vector3 finalDropPosition = new Vector3(initialDropPosition.x, groundHeight + heightAboveGround, initialDropPosition.z);

        item.transform.position = finalDropPosition;

        Rigidbody rb = item.AddComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;

        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;

        rb.maxDepenetrationVelocity = 10f; 

        Destroy(tempItemReference);

        InventorySystem.Instance.ReCalculateList();

        StartCoroutine(MonitorItemPosition(item, groundHeight));
    }

    private IEnumerator MonitorItemPosition(GameObject item, float groundHeight)
    {
        Rigidbody rb = item.GetComponent<Rigidbody>();

        while (item != null)
        {
            if (item.transform.position.y < groundHeight)
            {
                Vector3 correctedPosition = item.transform.position;
                correctedPosition.y = groundHeight + 0.1f; 
                item.transform.position = correctedPosition;

                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                }
            }

            yield return null; 
        }
    }

}

