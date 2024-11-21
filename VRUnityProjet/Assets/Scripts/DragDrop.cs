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

        float dropDistance = 15.0f;
        Vector3 initialDropPosition = dropSpawnPosition + forwardDirection * dropDistance;


        RaycastHit hit;
        float groundHeight = 0f;

        if (Physics.Raycast(initialDropPosition, Vector3.down, out hit))
        {
            groundHeight = hit.point.y;
        }

        float heightAboveGround = 10.0f; 
        Vector3 finalDropPosition = new Vector3(initialDropPosition.x, groundHeight + heightAboveGround, initialDropPosition.z);

        item.transform.position = finalDropPosition;

        Rigidbody rb = item.AddComponent<Rigidbody>();
        rb.useGravity = true;


        float fallSpeedMultiplier = 1200f; 
        rb.AddForce(Vector3.down * fallSpeedMultiplier, ForceMode.Acceleration);



        StartCoroutine(WaitForItemToFall(item, groundHeight));


        var parentObject = GameObject.Find("Objects");
        if (parentObject != null)
        {
            item.transform.SetParent(parentObject.transform);
        }
        else
        {
            Debug.LogError("Parent object 'Objects' not found!");
        }


        DestroyImmediate(tempItemReference.gameObject);
        InventorySystem.Instance.ReCalculateList();
    }

    private IEnumerator WaitForItemToFall(GameObject item, float groundHeight)
    {

        while (item.transform.position.y > groundHeight)
        {
            yield return null;
        }

        Debug.Log("L'objet a touché le sol.");
    }
}

