using System;
using UnityEngine;
using DG.Tweening;

public class CameraShaker : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeStrength = 1f;
    public int vibrato = 10;
    public float randomness = 90f;
    public bool fadeOut = true;
    
    private Camera _camera;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    [ContextMenu("ShakeCamera")]
    public void ShakeCamera()
    {
        _camera.DOShakePosition(shakeDuration, shakeStrength, vibrato, randomness, fadeOut);
    }
}
