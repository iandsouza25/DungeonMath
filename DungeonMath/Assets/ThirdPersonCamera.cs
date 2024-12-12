using UnityEngine;

public class ThirdPersonCameraWithPlayerControl : MonoBehaviour
{
    public Transform target; // The model the camera will follow
    public float distance = 2.5f; // Distance from the target
    public float heightOffset = 1.0f; // Height above the player
    public float sensitivityX = 3.0f; // Mouse X sensitivity
    public float sensitivityY = 2.0f; // Mouse Y sensitivity
    public float minY = -20f; // Minimum vertical angle
    public float maxY = 80f; // Maximum vertical angle
    public float rotationSmoothTime = 0.1f; // Smoothing for player rotation

    private float currentX = 0f; // Horizontal rotation angle
    private float currentY = 15f; // Start with a slight vertical angle
    private Vector3 rotationVelocity; // For smoothing player rotation

    void Update()
    {
        // Get mouse input
        currentX += Input.GetAxis("Mouse X") * sensitivityX;
        currentY -= Input.GetAxis("Mouse Y") * sensitivityY;

        // Clamp the vertical angle
        currentY = Mathf.Clamp(currentY, minY, maxY);
    }

void LateUpdate()
{
    if (target != null)
    {
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 desiredPosition = target.position + rotation * new Vector3(0, heightOffset, -distance);

        // Check for obstacles
        RaycastHit hit;
        if (Physics.Linecast(target.position + Vector3.up * heightOffset, desiredPosition, out hit))
        {
            transform.position = hit.point;
        }
        else
        {
            transform.position = desiredPosition;
        }

        transform.LookAt(target.position + Vector3.up * heightOffset);

        Vector3 targetForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(targetForward);
        target.rotation = Quaternion.Slerp(target.rotation, targetRotation, rotationSmoothTime);
    }
}
}