using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private const string IS_RUNNING = "IsRunning";
    private const string IS_JUMPING = "IsJumping";
    private const string IS_MOVING_LEFT= "IsMovingLeft";
    private const string IS_MOVING_RIGHT= "IsMovingRight";

    [SerializeField] private Player player;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
        animator.SetBool(IS_RUNNING, player.IsRunning());
        animator.SetBool(IS_JUMPING, player.IsJumping());
        animator.SetBool(IS_MOVING_LEFT, player.IsMovingLeft());
        animator.SetBool(IS_MOVING_RIGHT, player.IsMovingRight()); 
    }
}
