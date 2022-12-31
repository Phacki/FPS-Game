using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CamMovement : NetworkBehaviour
{
    public float currentSensitivity = 100f;
    public float unscopedSensitivity = 100f;
    public float scopeSensitivity = 100f;

    [SerializeField]
    private Transform yAxis;
    private float xAxis = 0;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (!IsOwner)
            return;
        mouseMovement();
    }

    private void mouseMovement()
    {
        float mouseX = Input.GetAxis("Mouse X") * currentSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * currentSensitivity * Time.deltaTime;

        xAxis -= mouseY;
        xAxis = Mathf.Clamp(xAxis, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xAxis, 0, 0);
        yAxis.Rotate(Vector3.up * mouseX);
    }
}
