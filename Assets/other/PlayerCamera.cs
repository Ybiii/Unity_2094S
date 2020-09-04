using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform targetFollow;
    public Transform cam;
    public Transform pivot;

    public float sensitiveX = 3f;
    public float sensitiveY = 3f;

    public float minX = -360;
    public float maxX = 360;
    public float minY = -60;
    public float maxY = 60;

    private Quaternion originalRot;

    private float rotX = 0;
    private float rotY = 0;

    private void Start()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb)
        {
            rb.freezeRotation = true;
        }

        originalRot = transform.localRotation;

        pivot = cam.parent;
    }

    private void Update()
    {
        rotX += Input.GetAxis("Mouse X") * sensitiveX;
        rotY += Input.GetAxis("Mouse Y") * sensitiveX;

        rotX = rotX % 360;
        rotY = rotY % 360;

        rotX = Mathf.Clamp(rotX, minX, maxX);
        rotY = Mathf.Clamp(rotY, minY, maxY);

        Quaternion quaternionX = Quaternion.AngleAxis(rotX, Vector3.up);
        Quaternion quaternionY = Quaternion.AngleAxis(rotY, Vector3.left);

        //pivot.transform.position = Vector3.Lerp(pivot.transform.position, targetFollow.position, Time.deltaTime * 4);

        transform.localRotation = originalRot * quaternionX * quaternionY;
    }
}
