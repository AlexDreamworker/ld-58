using UnityEngine;
using DG.Tweening;

public class CurtainController : MonoBehaviour
{
    public float fadeDuration = 1f;
    
    private SpriteRenderer spriteRenderer;
    private Tween currentTween;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetAlpha(0f);
    }
    
    [ContextMenu("Show Curtain")]
    public void ShowCurtain()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
        
        Color currentColor = spriteRenderer.color;
        float startAlpha = currentColor.a;
        
        Sequence fadeSequence = DOTween.Sequence();
        
        // (0 → 1)
        fadeSequence.Append(spriteRenderer.DOFade(1f, fadeDuration * (1f - startAlpha)));
        // (1 → 0)
        fadeSequence.Append(spriteRenderer.DOFade(0f, fadeDuration));
        
        currentTween = fadeSequence;
    }
    
    private void SetAlpha(float alpha)
    {
        Color color = spriteRenderer.color;
        color.a = alpha;
        spriteRenderer.color = color;
    }

    void OnDestroy()
    {
        if (currentTween != null && currentTween.IsActive())
        {
            currentTween.Kill();
        }
    }
}
