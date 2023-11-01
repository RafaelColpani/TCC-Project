using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    [Header("Player Tag")]
    [SerializeField] string tagDoJogador = "Player";

    [Header("Vm Cam Obj")]
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    [Space(5)]

    [Header("Camera Shake Configuration")]
    [SerializeField] float shakeDuration = 0.3f;
    [SerializeField] float shakeAmplitude = 1.0f;
    [SerializeField] float shakeFrequency = 2.0f;

    private bool isShaking = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tagDoJogador) && !isShaking)
        {
            isShaking = true;
            StartCoroutine(ShakeCamera());
        }
    }

    private IEnumerator ShakeCamera()
    {
        var noiseSettings = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        noiseSettings.m_AmplitudeGain = shakeAmplitude;
        noiseSettings.m_FrequencyGain = shakeFrequency;

        yield return new WaitForSeconds(shakeDuration);

        noiseSettings.m_AmplitudeGain = 0;
        noiseSettings.m_FrequencyGain = 0;
        isShaking = false;
    }
}
