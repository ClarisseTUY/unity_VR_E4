using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    private float runSpeed = 1f;
    [SerializeField] private GameInput gameInput;

    [SerializeField] private Transform cameraTransform;

    [SerializeField] private float time;

    [SerializeField] private LayerMask WhatIsGround;

    [SerializeField] private AnimationCurve animCurve;


    private bool isWalking;
    private bool isRunning;
    private bool isJumping;
    private bool onPickUp;


    private void Update()
    {
        SurfaceAlignment();
        OnRun();
        Movement();
        OnJump();
        OnPickup();
    }



    private void SurfaceAlignment()
    {
        Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit info = new RaycastHit();
        Quaternion RotationRef = Quaternion.Euler(0, 0, 0);

        if(Physics.Raycast(ray, out info, WhatIsGround)) {
            RotationRef = Quaternion.Lerp(transform.rotation, Quaternion.FromToRotation(Vector3.up, info.normal), animCurve.Evaluate(time));
            transform.rotation = Quaternion.Euler(RotationRef.eulerAngles.x, transform.eulerAngles.y, RotationRef.eulerAngles.z);
        }
    }
    private void Movement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);
        moveDir = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * moveDir;
        moveDir.Normalize();

        transform.position += moveDir * moveSpeed * runSpeed * Time.deltaTime;

        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);


    }

    private void OnRun()
    {
        float runState = gameInput.GetRunningState();
        isRunning = runState > 0;

        if (isRunning)
        {
            runSpeed = 1.5f;
        }
        else
        {
            runSpeed = 1f;
        }

    }
    private void OnJump()
    {
        float jumpState = gameInput.GetJumpingState();
        isJumping = jumpState > 0;
    }


    public bool IsWalking()
    {
        return isWalking;
    } 

    public bool IsRunning()
    {
        return isRunning;
    }
    public bool IsJumping()
    {
        return isJumping;
    }

    private void OnPickup()
    {
        float pickUpState = gameInput.GetPickUpState();
        onPickUp = pickUpState > 0;

        if (onPickUp)
        {
           
        }
    }



}
