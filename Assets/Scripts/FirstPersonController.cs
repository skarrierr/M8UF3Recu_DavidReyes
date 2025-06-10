using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("Movement")]
    public Rigidbody rb;
    public float speed_forward;
    public float speed_sideways;
    public float speed_backwards;
    [Header("Camera")]
    public Camera cam;
    public float sensitivity_x;
    public float sensitivity_y;
    public float maxAngle;
    float currentAngle;
    private void Start()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        if (cam == null)
            cam = GetComponentInChildren<Camera>();
        currentAngle = cam.transform.localEulerAngles.x;
    }
    public void Move(Vector2 rawinput, float deltaTime)
    {
        float magnitude = rawinput.magnitude;
        if(magnitude > 1)
        {
            rawinput /= magnitude;
        }
        rawinput.x *= speed_sideways;
        if (rawinput.y > 0)
        {
            rawinput.y *= speed_forward;
        }
        else
        {
            rawinput.y *= speed_backwards;
        }
        transform.position += transform.forward * rawinput.y * deltaTime;
        transform.position += transform.right * rawinput.x * deltaTime;
    }
    public void Look(Vector2 rawinput)
    {
        rawinput.y *= sensitivity_y;
        rawinput.x *= sensitivity_x;
        transform.Rotate(Vector3.up, rawinput.x);
        currentAngle = Mathf.Clamp(currentAngle-rawinput.y, -maxAngle, maxAngle);
        cam.transform.localEulerAngles = new Vector3(currentAngle, 0);
    }
}
