using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Makes the camera look at a selected target while maintaining
/// a fixed distance from it.
/// Left Click  = Select Target
/// Right Click = Reset to Default Target
/// </summary>
public class TargetLook : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private Transform defaultTarget;
    [SerializeField] private Transform currentTarget;

    [Header("Camera Settings")]
    [Tooltip("Distance from the target.")]
    [SerializeField] private float distance = 5f;

    private Camera mainCamera;

    // Direction from target to camera
    private Vector3 offsetDirection;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
        if (defaultTarget == null)
            defaultTarget = transform;

        if (currentTarget == null)
            currentTarget = defaultTarget;

        // Save the camera's current direction from the target.
        offsetDirection = (transform.position - currentTarget.position).normalized;
    }

    private void Update()
    {
        HandleInput();
        UpdateCamera();
    }

    private void HandleInput()
    {
        if (mainCamera == null || Mouse.current == null)
            return;

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                currentTarget = hit.transform;

                // Save the current viewing direction when changing targets.
                offsetDirection = (transform.position - currentTarget.position).normalized;
            }
        }

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            currentTarget = defaultTarget;

            offsetDirection = (transform.position - currentTarget.position).normalized;
        }
    }

    private void UpdateCamera()
    {
        if (currentTarget == null)
            return;

        // Keep the camera at the desired distance.
        transform.position = currentTarget.position + offsetDirection * distance;

        // Always look at the target.
        transform.LookAt(currentTarget);
    }
}