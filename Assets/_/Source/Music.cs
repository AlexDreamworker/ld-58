using UnityEngine;
using DG.Tweening;

public class Music : MonoBehaviour
{
    [Header("Fade Out Settings")]
    [SerializeField] private float fadeOutDuration = 2f;
    [SerializeField] private Ease fadeOutEase = Ease.Linear;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void FadeOut()
    {
        if (audioSource == null)
        {
            Debug.LogWarning("AudioSource component not found!");
            return;
        }

        // Создаем твин для плавного уменьшения громкости
        audioSource.DOFade(0f, fadeOutDuration)
            .SetEase(fadeOutEase)
            .OnComplete(() => {
                // Опционально: выключаем источник после завершения
                audioSource.Stop();
                Debug.Log("FadeOut completed");
            });
    }

    // Дополнительный метод с кастомными параметрами
    public void FadeOut(float duration, Ease easeType = Ease.Linear)
    {
        if (audioSource == null) return;

        fadeOutDuration = duration;
        fadeOutEase = easeType;
        FadeOut();
    }
}
