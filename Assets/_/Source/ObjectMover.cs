using UnityEngine;
using DG.Tweening;

public class ObjectMover : MonoBehaviour
{
    [Header("Точки перемещения")]
    public Vector3 pointA;
    public Vector3 pointB;
    
    [Header("Настройки анимации")]
    public float duration = 2f;
    public Ease easeType = Ease.InOutQuad;

    void Start()
    {
        // Устанавливаем объект в точку А
        transform.position = pointA;
        
        // Запускаем перемещение в точку Б
        MoveFromAToB();
    }

    public void MoveFromAToB()
    {
        // Плавное перемещение из текущей позиции (точка А) в точку Б
        transform.DOMove(pointB, duration)
            .SetEase(easeType)
            .OnComplete(() => Debug.Log("Перемещение завершено!"));
    }
}