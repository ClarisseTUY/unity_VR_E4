using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OpenHelpMenu : MonoBehaviour
{
    public GameObject Canvas;
    public InputActionReference OpenHelpMenuAction;
    private bool isMenuOpen = false;

    private void OnEnable()
    {
        OpenHelpMenuAction.action.performed += OnToggleMenu;
        OpenHelpMenuAction.action.Enable();
    }

    private void OnDisable()
    {
        OpenHelpMenuAction.action.performed -= OnToggleMenu;
        OpenHelpMenuAction.action.Disable();
    }

    private void OnToggleMenu(InputAction.CallbackContext context)
    {
        isMenuOpen = !isMenuOpen;
        Canvas.SetActive(isMenuOpen);
    }
}
