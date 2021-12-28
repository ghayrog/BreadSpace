using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeScript : MonoBehaviour
{
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 headPosition = GameObject.Find("Cat_head").transform.position;
        Vector3 headRotation = GameObject.Find("Cat_head").transform.rotation.eulerAngles;
        Vector3 mouseDirection = (PlayerController.Instance.transform.position - headPosition).normalized;
        double aimAngle = Math.Atan2(mouseDirection.y, mouseDirection.x) / Math.PI * 180;
        aimAngle -= headRotation.z;
        mouseDirection = new Vector3((float)Math.Cos(aimAngle/180*Math.PI)*1.2f, (float)Math.Sin(aimAngle / 180 * Math.PI) * 0.5f,0);
        transform.localPosition = startPosition + mouseDirection/20;
    }
}
