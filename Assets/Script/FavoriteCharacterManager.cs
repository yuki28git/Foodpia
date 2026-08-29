using UnityEngine;
using UnityEngine.InputSystem;

public class FavoriteCharacterManager : MonoBehaviour
{
    [Header("Spawn")]
    [SerializeField] private Transform spawnRoot;
    [SerializeField] private float modelYOffset = 0f;

    [Header("Move")]
    [SerializeField] private float moveSpeed = 4f;

    [Header("Jump / Gravity")]
    [SerializeField] private float jumpHeight = 1.2f;
    [SerializeField] private float gravity = -20f;
    [SerializeField] private float groundedStickForce = -2f;

    [Header("Camera Follow (TPS)")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float cameraHeight = 2.2f;
    [SerializeField] private float cameraDistance = 6.5f;
    [SerializeField] private float cameraSideOffset = 0.35f;
    [SerializeField] private float cameraPositionSmooth = 14f;
    [SerializeField] private float cameraRotationSmooth = 18f;

    private GameObject currentModel;
    private CharacterController characterController;
    private float verticalVelocity;

    // 視点固定用（開始時のカメラ向きを保持）
    private float fixedYaw = 0f;
    private float fixedPitch = 10f;

    private void Start()
    {
        if (cameraTransform == null && Camera.main != null)
            cameraTransform = Camera.main.transform;

        SpawnFavoriteCharacter();

        if (cameraTransform != null)
        {
            Vector3 camEuler = cameraTransform.eulerAngles;
            fixedYaw = camEuler.y;
            fixedPitch = NormalizePitch(camEuler.x);
        }
    }

    private void Update()
    {
        if (currentModel == null || characterController == null)
            return;

        MoveWithGravityAndJump();
    }

    private void LateUpdate()
    {
        UpdateCameraFollow();
    }

    private void SpawnFavoriteCharacter()
    {
        string favoriteId = FavoriteCharacterService.GetFavorite();
        if (string.IsNullOrEmpty(favoriteId))
        {
            Debug.Log("お気に入りキャラが未設定です");
            return;
        }

        if (spawnRoot == null)
        {
            Debug.LogWarning("spawnRoot が未設定です");
            return;
        }

        if (currentModel != null)
            Destroy(currentModel);

        string path = $"Charactors/3D/{favoriteId}";
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab == null)
        {
            Debug.LogWarning($"Prefabが見つかりません: {path}");
            return;
        }

        currentModel = Instantiate(prefab, spawnRoot, false);

        Vector3 p = currentModel.transform.localPosition;
        p.y += modelYOffset;
        currentModel.transform.localPosition = p;

        CharacterAnimationHelper.PlayIdle(currentModel);
        SetupCharacterController();
    }

    private void SetupCharacterController()
    {
        characterController = currentModel.GetComponent<CharacterController>();
        if (characterController == null)
            characterController = currentModel.AddComponent<CharacterController>();

        var renderers = currentModel.GetComponentsInChildren<Renderer>();
        if (renderers.Length > 0)
        {
            Bounds b = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++) b.Encapsulate(renderers[i].bounds);

            float height = Mathf.Max(0.5f, b.size.y);
            float radius = Mathf.Max(0.2f, Mathf.Max(b.extents.x, b.extents.z) * 0.35f);

            characterController.height = height;
            characterController.radius = radius;
            characterController.center = currentModel.transform.InverseTransformPoint(b.center);
        }

        characterController.minMoveDistance = 0f;
        characterController.stepOffset = 0.3f;
        characterController.skinWidth = 0.08f;
        characterController.slopeLimit = 45f;
    }

    private void MoveWithGravityAndJump()
    {
        if (Keyboard.current == null) return;

        float x = 0f;
        float z = 0f;

        if (Keyboard.current.wKey.isPressed) z += 1f;
        if (Keyboard.current.sKey.isPressed) z -= 1f;
        if (Keyboard.current.dKey.isPressed) x += 1f;
        if (Keyboard.current.aKey.isPressed) x -= 1f;

        Vector2 input = new Vector2(x, z);
        if (input.sqrMagnitude > 1f) input.Normalize();

        // 視点固定の向き基準で移動方向を作る
        Quaternion yawRot = Quaternion.Euler(0f, fixedYaw, 0f);
        Vector3 camForward = yawRot * Vector3.forward;
        Vector3 camRight = yawRot * Vector3.right;

        Vector3 moveDir = (camForward * input.y) + (camRight * input.x);

        // 入力があれば、その方向を向く（A=左向き、D=右向き、S=後ろ向き）
        if (moveDir.sqrMagnitude > 0.0001f)
        {
            currentModel.transform.rotation = Quaternion.LookRotation(moveDir, Vector3.up);
        }

        Vector3 horizontalMove = moveDir.normalized * (moveSpeed * input.magnitude);

        if (characterController.isGrounded && verticalVelocity < 0f)
            verticalVelocity = groundedStickForce;

        if (Keyboard.current.spaceKey.wasPressedThisFrame && characterController.isGrounded)
            verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = horizontalMove;
        velocity.y = verticalVelocity;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void UpdateCameraFollow()
    {
        if (cameraTransform == null || currentModel == null)
            return;

        Vector3 focus = currentModel.transform.position + Vector3.up * cameraHeight;

        // 視点角は固定（マウスで変更しない）
        Quaternion orbitRot = Quaternion.Euler(fixedPitch, fixedYaw, 0f);

        Vector3 backward = orbitRot * Vector3.back;
        Vector3 side = orbitRot * Vector3.right * cameraSideOffset;
        Vector3 desiredPos = focus + backward * cameraDistance + side;

        cameraTransform.position = Vector3.Lerp(
            cameraTransform.position,
            desiredPos,
            cameraPositionSmooth * Time.deltaTime
        );

        Quaternion desiredRot = Quaternion.LookRotation(focus - cameraTransform.position, Vector3.up);
        cameraTransform.rotation = Quaternion.Slerp(
            cameraTransform.rotation,
            desiredRot,
            cameraRotationSmooth * Time.deltaTime
        );
    }

    private float NormalizePitch(float xAngle)
    {
        if (xAngle > 180f) xAngle -= 360f;
        return xAngle;
    }

    private void OnDestroy()
    {
        if (currentModel != null) Destroy(currentModel);
    }
}