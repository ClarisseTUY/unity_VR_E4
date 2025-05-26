using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer.Internal;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Playables;
using Unity.VisualScripting;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;


    public InputActionReference dropItemAction;

    private bool isHovered = false;
    //public PlayerMovement playerMovement;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        //playerMovement = FindObjectOfType<PlayerMovement>();

    }

    private void OnEnable()
    {
        AddInputListener();
    }

    private void OnDisable()
    {
        RemoveInputListener();
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

        //GameObject item = Instantiate(Resources.Load<GameObject>(cleanName + "_Model"));

        InventoryItem invItem = tempItemReference.GetComponent<InventoryItem>();
        if (invItem != null && invItem.worldPrefab != null)
        {
            GameObject item = Instantiate(invItem.worldPrefab);

            Vector3 dropSpawnPosition = Camera.main.transform.position;
            Vector3 forwardDirection = Camera.main.transform.forward;

            float playerMidHeight = Camera.main.transform.position.y;


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



            tempItemReference.transform.SetParent(null);
            Destroy(tempItemReference);
            StartCoroutine(DelayedRecalculate());


            StartCoroutine(MonitorItemPosition(item, groundHeight));
        }
        else
        {
            Debug.LogError("World prefab manquant !");
        }


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

    private IEnumerator DelayedRecalculate()
    {
        yield return null; // attendre fin de frame
        InventorySystem.Instance.ReCalculateList();
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    // Simulation du début de drag en VR : déclenche le comportement de DragDrop
    //    var dragDrop = GetComponent<DragDrop>();
    //    if (dragDrop != null)
    //    {
    //        dragDrop.OnBeginDrag(eventData);
    //    }
    //}


    //private void OnEnable()
    //{
    //    dropItemAction.action.performed += OnDropAction;
    //    dropItemAction.action.Enable();
    //}

    //private void OnDisable()
    //{
    //    dropItemAction.action.performed -= OnDropAction;
    //    dropItemAction.action.Disable();
    //}

    //private void OnDropAction(InputAction.CallbackContext ctx)
    //{
    //    if (itemBeingDragged != null)
    //    {
    //        DropItemIntoTheWorld(itemBeingDragged);
    //    }
    //}


    private void AddInputListener()
    {
        if (dropItemAction != null)
        {
            dropItemAction.action.performed += OnDropInputPerformed;
            dropItemAction.action.Enable();
        }
    }

    private void RemoveInputListener()
    {
        if (dropItemAction != null)
        {
            dropItemAction.action.performed -= OnDropInputPerformed;
            dropItemAction.action.Disable();
        }
    }
    private void OnDropInputPerformed(InputAction.CallbackContext context)
    {
        if (isHovered)
        {
            DropItemIntoTheWorld(gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovered = false;
    }

    //public void ForceDropFromVR()
    //{
    //    DropItemIntoTheWorld(gameObject);
    //    //Destroy(gameObject);
    //    //InventorySystem.Instance.ReCalculateList();
    //}
}

