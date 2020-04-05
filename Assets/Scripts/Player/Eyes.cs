using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eyes : MonoBehaviour
{
    public float mouseSensitivity = 2f;

    public Transform playerBody;

    float xRotation = 0f;

    float mouseX;
    float mouseY;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    public void Update()
    {        
        mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.fixedDeltaTime;
        mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.fixedDeltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localEulerAngles = new Vector3(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);

    }

    private void FixedUpdate()
    {


    }
}
