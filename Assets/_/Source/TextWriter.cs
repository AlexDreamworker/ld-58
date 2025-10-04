using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextWriter : MonoBehaviour
{
    public float charsPerSecond = 50f;
    
    [Space]
    public UnityEvent onTypeCompleted;

    private TMP_Text _text;
    
    private Coroutine _textCoroutine;
    
    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        HideAllVisibleChars();
    }

    [ContextMenu("_Show")]
    public void Show()
    {
        _textCoroutine = StartCoroutine(TypeText());
    }
    
    [ContextMenu("_Hide")]
    public void Hide()
    {
        if (_textCoroutine != null)
        {
            StopCoroutine(_textCoroutine);
            _textCoroutine = null;
            HideAllVisibleChars();
        }
    }

    IEnumerator TypeText()
    {
        HideAllVisibleChars();
        
        int totalCharacters = _text.textInfo.characterCount;

        for (int visibleCount = 0; visibleCount <= totalCharacters; visibleCount++)
        {
            _text.maxVisibleCharacters = visibleCount;
            yield return new WaitForSeconds(1f / charsPerSecond);
        }
        
        onTypeCompleted?.Invoke();
    }

    private void HideAllVisibleChars()
    {
        _text.maxVisibleCharacters = 0;
    }
}
