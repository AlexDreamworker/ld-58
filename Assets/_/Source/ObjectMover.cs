using System;
using UnityEngine;
using DG.Tweening;

public class ObjectMover : MonoBehaviour
{
    public Vector3 pointA;
    public Vector3 pointB;
    
    public float duration = 2f;
    public Ease easeType = Ease.InOutQuad;
    
    private Tween _tween;

    void Start()
    {
        transform.position = pointA;
        
        MoveFromAToB();
    }

    public void MoveFromAToB()
    {
        _tween = transform.DOMove(pointB, duration)
            .SetEase(easeType)
            .OnComplete(() => Debug.Log("Перемещение завершено!"));
    }

    private void OnDisable()
    {
        _tween?.Kill();
    }
}