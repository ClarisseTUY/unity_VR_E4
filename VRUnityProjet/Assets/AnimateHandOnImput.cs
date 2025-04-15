using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimateHandOnImput : MonoBehaviour
{
    public InputActionProperty pinchAnimationAction;
    public InputActionProperty gripAnimationAction;
    public Animator handAnimator;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float triggerValue = pinchAnimationAction.action.ReadValue<float>(); //float pour value, bool pour activate ou non

        handAnimator.SetFloat("Trigger", triggerValue); // animation index et pousse se ferme quand on appuit sur le bouton

        Debug.Log(triggerValue); // remove après avoir testé


        //fermer le poing quand on clique sur le bouton
        //necessite de changer la réference
        float gripValue = gripAnimationAction.action.ReadValue<float>();
        handAnimator.SetFloat("Grip", gripValue);
    }
}
