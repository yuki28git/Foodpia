using UnityEngine;
using UnityEngine.InputSystem;

public class CameraFollowSpawnedCharacter : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform spawnRoot;
    [SerializeField] private float targetHeight = 1.4f;

    [Header("Mouse Look")]
    [SerializeField] private float mouseSensitivityX = 180f;
    [SerializeField] private float mouseSensitivityY = 140f;
    [SerializeField] private float minPitch = -20f;
    [SerializeField] private float maxPitch = 60f;

    [Header("Distance")]
    [SerializeField] private float distance = 5.5f;
    [SerializeField] private float minDistance = 2.5f;
    [SerializeField] private float maxDistance = 8.0f;
    [SerializeField] private float zoomSpeed = 2.0f;

    [Header("Smoothing")]
    [SerializeField] private float positionSmooth = 12f;
    [SerializeField] private float rotationSmooth = 14f;

    private Transform target;
    private float yaw;
    private float pitch;

    private void Start()
    {
        Vector3 e = transform.eulerAngles;
        yaw = e.y;
        pitch = NormalizePitch(e.x);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void LateUpdate()
    {
        if (spawnRoot == null) return;

        if (target == null)
        {
            if (spawnRoot.childCount > 0) target = spawnRoot.GetChild(0);
            else return;
        }

        UpdateMouseLook();
        UpdateZoom();

        Vector3 focus = target.position + Vector3.up * targetHeight;
        Quaternion orbitRot = Quaternion.Euler(pitch, yaw, 0f);
        Vector3 desiredPos = focus - orbitRot * Vector3.forward * distance;

        transform.position = Vector3.Lerp(
            transform.position,
            desiredPos,
            positionSmooth * Time.deltaTime
        );

        Quaternion desiredRot = Quaternion.LookRotation(focus - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRot,
            rotationSmooth * Time.deltaTime
        );
    }

    private void UpdateMouseLook()
    {
        if (Mouse.current == null) return;
        if (Cursor.lockState != CursorLockMode.Locked) return;

        Vector2 delta = Mouse.current.delta.ReadValue();
        yaw += delta.x * mouseSensitivityX * Time.deltaTime;
        pitch -= delta.y * mouseSensitivityY * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void UpdateZoom()
    {
        if (Mouse.current == null) return;

        float scroll = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Abs(scroll) < 0.01f) return;

        distance -= scroll * 0.01f * zoomSpeed;
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
    }

    private float NormalizePitch(float xAngle)
    {
        if (xAngle > 180f) xAngle -= 360f;
        return xAngle;
    }
}