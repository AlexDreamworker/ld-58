using UnityEngine;
using DG.Tweening;

public class CurtainController : MonoBehaviour
{
    [Header("Настройки анимации")]
    [SerializeField, Tooltip("Длительность одной фазы (появления или исчезновения) в секундах")]
    private float fadeDuration = 1f;
    
    private SpriteRenderer spriteRenderer;
    private Tween currentTween;

    void Start()
    {
        // Получаем компонент SpriteRenderer и устанавливаем начальную прозрачность
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetAlpha(0f);
    }

    /// <summary>
    /// Запускает анимацию затемнения и осветления
    /// </summary>
    [ContextMenu("Show Curtain")]
    public void ShowCurtain()
    {
        // Останавливаем текущую анимацию, если она есть
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }

        // Получаем текущее значение альфа-канала
        Color currentColor = spriteRenderer.color;
        float startAlpha = currentColor.a;

        // Создаем последовательность анимаций
        Sequence fadeSequence = DOTween.Sequence();
        
        // Фаза затемнения (0 → 1)
        fadeSequence.Append(spriteRenderer.DOFade(1f, fadeDuration * (1f - startAlpha)));
        // Фаза осветления (1 → 0)
        fadeSequence.Append(spriteRenderer.DOFade(0f, fadeDuration));
        
        // Сохраняем ссылку на текущую анимацию
        currentTween = fadeSequence;
    }

    /// <summary>
    /// Устанавливает значение альфа-канала для SpriteRenderer
    /// </summary>
    /// <param name="alpha">Значение прозрачности (0-1)</param>
    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    void OnDestroy()
    {
        // Уничтожаем твины при удалении объекта для избежания ошибок
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
    }
}
