using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamHolder : MonoBehaviour
{
    public Transform leg;
    public float speedH = 2.0f;
    public float speedV = 2.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            yaw += speedH * Input.GetAxis("Mouse X");
            pitch -= speedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(0, 0, yaw);
            if (pitch < -90) pitch = -90;
            if (pitch > 0) pitch = 0;
            leg.localEulerAngles = new Vector3(pitch, 0, 0);
        }
    }
}
