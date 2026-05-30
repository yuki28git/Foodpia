using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CharacterDragRotate : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 0.2f;

    private bool isDragging;
    private Vector2 lastPointerPosition;

    void Update()
    {
        if (Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            isDragging = true;
            lastPointerPosition = Mouse.current.position.ReadValue();
        }

        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 currentPointerPosition = Mouse.current.position.ReadValue();
            Vector2 delta = currentPointerPosition - lastPointerPosition;

            transform.Rotate(0f, -delta.x * rotationSpeed, 0f, Space.World);

            lastPointerPosition = currentPointerPosition;
        }
    }
}