using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public Transform target; 
    public LayerMask obstacleMask; 
    public float rotationSpeed = 2f; 
    public float distance = 1f; 
    public float cameraHeight = 0.3f; 
    public float moveSpeed = 2f; 


    void LateUpdate()
    {
        float mouseX = Input.GetAxisRaw("Mouse X");
        float mouseY = Input.GetAxisRaw("Mouse Y");

        transform.RotateAround(target.position, Vector3.up, mouseX * rotationSpeed);
        transform.RotateAround(target.position, transform.right, -mouseY * rotationSpeed);

        transform.LookAt(target);

        Vector3 cameraPosition = target.position + new Vector3(0f, cameraHeight, 0f);

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

        transform.position = new Vector3(transform.position.x, cameraPosition.y, transform.position.z);
 
        if (Input.GetKey(KeyCode.W))
        {
            Vector3 moveDirection = transform.forward;
            moveDirection.y = 0f; 
            target.Translate(moveDirection.normalized * moveSpeed * Time.deltaTime, Space.World);
        }
    }
}
