using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

    }
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }


    public float GetRunningState()
    {
        float run = playerInputActions.Player.Run.ReadValue<float>();

        return run;
    }


    public float GetJumpingState()
    {
        float jump = playerInputActions.Player.Jump.ReadValue<float>();

        return jump;
    }
    public float GetPickUpState()
    {
        float pickUp = playerInputActions.Player.PickUp.ReadValue<float>();

        return pickUp;
    }

}
