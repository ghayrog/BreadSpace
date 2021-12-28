using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ShakeCam : MonoBehaviour
{
    private CinemachineVirtualCamera cam;
    private float shakeTimer;

    //Singleton
    private static ShakeCam instance;
    public static ShakeCam Instance
    {
        get
        {
            if (instance == null) instance = GameObject.FindObjectOfType<ShakeCam>();
            return instance;
        }
    }

    private void Awake()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float timer)
    {
        CinemachineBasicMultiChannelPerlin camPerlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        camPerlin.m_AmplitudeGain += intensity;
        shakeTimer += timer;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                //Stop shaking
                CinemachineBasicMultiChannelPerlin camPerlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                camPerlin.m_AmplitudeGain = 0;
                shakeTimer = 0;
            }
        }
    }
}
