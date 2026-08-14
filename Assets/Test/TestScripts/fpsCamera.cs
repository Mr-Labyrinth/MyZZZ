using UnityEngine;

public class fpsCamera : MonoBehaviour
{
    private float mouseSensitivity = 200f;
    [SerializeField]
    private Transform playerBody;
    float xRotation = 0;
    [SerializeField]
    private bool flag = false;
    [SerializeField]
    private float speed = 5;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * (mouseSensitivity * Time.deltaTime);
        xRotation -= mouseY;
        if (flag)
        {
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        }
        transform.localRotation = Quaternion.Euler(xRotation,0f, 0f);
        playerBody.Rotate(Vector3.up*mouseX);
        if (Input.GetKeyDown(KeyCode.Escape)){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 dir = playerBody.transform.forward * vertical + playerBody.transform.right * horizontal;
        dir.Normalize();
        playerBody.transform.Translate(dir * speed * Time.deltaTime, Space.World);
    }
}
