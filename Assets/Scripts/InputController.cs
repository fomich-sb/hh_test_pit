using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Zenject;

public class InputController : MonoBehaviour
{
    private InputSystem_Actions _inputActions;
    private Camera _mainCamera;

    [Inject] private BagController bagController;

    private void Awake()
    {
        _inputActions = new InputSystem_Actions();
        _mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        _inputActions.Main.Enable();
        _inputActions.Main.Click.performed += OnClick;
    }
    private void OnDisable()
    {
        _inputActions.Main.Click.performed -= OnClick;
        _inputActions.Main.Disable();
    }

    private void OnClick(InputAction.CallbackContext context)
    {
        Vector2 screenPosition = _inputActions.Main.ClickPosition.ReadValue<Vector2>();
        Vector2 clickPosition = _mainCamera.ScreenToWorldPoint(screenPosition);
        RaycastHit2D hit = Physics2D.Raycast(clickPosition, Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Figures"));
        if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out FigureTemplate figureTemplate))
        {
            if (figureTemplate.figure.Status == Figure.FigureStatus.Spawned)
                bagController.Add(figureTemplate.figure);
        }
    }
}
