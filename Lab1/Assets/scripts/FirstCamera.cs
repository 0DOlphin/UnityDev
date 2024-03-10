using UnityEngine;

public class FirstCamera : MonoBehaviour
{
    public Transform target; 
    public LayerMask obstacleMask; 
    public float rotationSpeed = 2f; 
    public float distance = 1f; 
    public float cameraHeight = 1f; 
    public float moveSpeed = 2f; 
    public float minVerticalAngle = -30f; // Мінімальний кут обертання камери вгору
    public float maxVerticalAngle = 30f; // Максимальний кут обертання камери вниз

    private float verticalRotation = 0f;

    void LateUpdate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        // Обмежуємо вертикальне обертання камери
        verticalRotation -= mouseY * rotationSpeed;
        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);

        transform.RotateAround(target.position, Vector3.up, mouseX * rotationSpeed);
        transform.RotateAround(target.position, transform.right, -mouseY * rotationSpeed);

        Vector3 offset = -transform.forward * distance;
        RaycastHit hit;
        if (Physics.Raycast(target.position, offset.normalized, out hit, distance, obstacleMask))
        {
            transform.position = hit.point + offset.normalized * hit.distance;
        }
        else
        {
            transform.position = target.position + offset;
        }

    }
}

