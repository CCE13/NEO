using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }


    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private float intensityValue;
    private float shakeTimer;

    private void Awake()
    {
        Instance = this;
    }

    public void CameraShakeControl(float intensity, float time)
    {
        //gets the current virtural camera that is active
        cinemachineVirtualCamera = (CinemachineVirtualCamera)FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
        //gets the cinemachine camerashake component
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        //sets the camera shake intensity as the value given
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
        shakeTimer = time;
        intensityValue = intensity;
    }

    private void Update()
    {

        if (shakeTimer > 0)
        {
            //A timer to decide when the camera shake ends
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0f)
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(intensityValue, 0f, 1000 * Time.deltaTime);
            }

        }
    }
    public void StopShake()
    {
        cinemachineVirtualCamera = (CinemachineVirtualCamera)FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
        //gets the cinemachine camerashake component
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        //sets the camera shake intensity as the value given
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
        intensityValue = 0f;
    }
}
