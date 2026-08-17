using UnityEngine;

/// <summary>
/// Rotates this GameObject around a target.
/// </summary>
public class RotateAround : MonoBehaviour
{
    [Header("Rotation Settings")]

    [Tooltip("The object to rotate around.")]
    [SerializeField] private Transform target;

    [Tooltip("Rotation speed in degrees per second.")]
    [SerializeField] private float rotationSpeed = 30f;

    private void Start()
    {
        if (target == null)
        {
            target = transform;
            Debug.Log("RotateAround target not assigned. Using this GameObject.");
        }
    }

    private void Update()
    {
        transform.RotateAround(
            target.position,
            target.up,
            rotationSpeed * Time.deltaTime);
    }
}