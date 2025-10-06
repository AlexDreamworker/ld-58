using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ColorController : MonoBehaviour
{
    public List<SpriteRenderer> _sprites;
    public List<SpriteRenderer> _backgrounds;
    public Color targetColor = Color.red;
    public Color colorForBackground = Color.red;
    public float duration = 1f;
    
    [ContextMenu("SetColorToAllSprites")]
    public void SetColorToAllSprites()
    {
        foreach (var sprite in _sprites)
        {
            sprite.DOColor(targetColor, duration);
        }
        
        foreach (var sprite in _backgrounds)
        {
            sprite.DOColor(colorForBackground, duration);
        }
    }
}
