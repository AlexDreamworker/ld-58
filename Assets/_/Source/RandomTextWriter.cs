using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomTextWriter : MonoBehaviour
{
    [Header("Text Phrases")]
    [TextArea(3, 5)]
    [SerializeField] private string[] textPhrases; // Массив фраз для случайного выбора

    [Header("Timing Settings")]
    [SerializeField] private float minDelayBetweenCycles = 5f; // Минимальная задержка между циклами
    [SerializeField] private float maxDelayBetweenCycles = 10f; // Максимальная задержка между циклами
    [SerializeField] private float minDisplayTime = 2f; // Минимальное время отображения полного текста
    [SerializeField] private float maxDisplayTime = 4f; // Максимальное время отображения полного текста

    [Header("Typewriter Settings")]
    [SerializeField] private float charsPerSecond = 50f; // Скорость печати (символов в секунду)

    private TMP_Text _tmpText;
    private Coroutine _textCycleCoroutine;
    private int _lastPhraseIndex = -1; // Индекс последней использованной фразы

    private void Awake()
    {
        _tmpText = GetComponent<TMP_Text>();
        if (_tmpText == null)
        {
            Debug.LogError("RandomTextWriter: На объекте не найден компонент TMP_Text!");
        }
    }

    private void Start()
    {
        // Начинаем цикл при старте
        StartTextCycle();
    }

    /// <summary>
    /// Запускает основной цикл показа текста
    /// </summary>
    public void StartTextCycle()
    {
        if (_textCycleCoroutine != null)
        {
            StopCoroutine(_textCycleCoroutine);
        }
        _textCycleCoroutine = StartCoroutine(TextCycleRoutine());
    }

    /// <summary>
    /// Останавливает цикл показа текста
    /// </summary>
    public void StopTextCycle()
    {
        if (_textCycleCoroutine != null)
        {
            StopCoroutine(_textCycleCoroutine);
            _textCycleCoroutine = null;
        }
        HideAllVisibleChars();
    }

    private IEnumerator TextCycleRoutine()
    {
        while (true)
        {
            // Ждем случайное время перед показом следующей фразы
            float delay = Random.Range(minDelayBetweenCycles, maxDelayBetweenCycles);
            yield return new WaitForSeconds(delay);

            // Выбираем случайную фразу, которая не совпадает с предыдущей
            string selectedPhrase = GetRandomUniquePhrase();
            if (selectedPhrase != null)
            {
                // Устанавливаем полный текст и запускаем типайп-эффект
                _tmpText.text = selectedPhrase;
                _tmpText.ForceMeshUpdate(); // Обновляем информацию о тексте
                yield return StartCoroutine(TypeTextRoutine());

                // Ждем случайное время перед скрытием текста
                float displayTime = Random.Range(minDisplayTime, maxDisplayTime);
                yield return new WaitForSeconds(displayTime);

                // Скрываем текст
                _tmpText.maxVisibleCharacters = 0;
                //yield return StartCoroutine(HideTextRoutine());
            }
        }
    }

    private IEnumerator TypeTextRoutine()
    {
        int totalCharacters = _tmpText.textInfo.characterCount;
        _tmpText.maxVisibleCharacters = 0;

        for (int visibleCount = 0; visibleCount <= totalCharacters; visibleCount++)
        {
            _tmpText.maxVisibleCharacters = visibleCount;
            yield return new WaitForSeconds(1f / charsPerSecond);
        }
    }

    private IEnumerator HideTextRoutine()
    {
        int totalCharacters = _tmpText.textInfo.characterCount;

        for (int visibleCount = totalCharacters; visibleCount >= 0; visibleCount--)
        {
            _tmpText.maxVisibleCharacters = visibleCount;
            yield return null;
        }
    }

    /// <summary>
    /// Выбирает случайную фразу, которая не совпадает с предыдущей
    /// </summary>
    private string GetRandomUniquePhrase()
    {
        if (textPhrases == null || textPhrases.Length == 0)
        {
            Debug.LogWarning("RandomTextWriter: Массив textPhrases пуст!");
            return null;
        }

        // Если фраза всего одна, просто возвращаем ее
        if (textPhrases.Length == 1)
        {
            _lastPhraseIndex = 0;
            return textPhrases[0];
        }

        // Выбираем случайный индекс, отличный от предыдущего
        int randomIndex;
        do
        {
            randomIndex = Random.Range(0, textPhrases.Length);
        } while (randomIndex == _lastPhraseIndex);

        _lastPhraseIndex = randomIndex;
        return textPhrases[randomIndex];
    }

    private void HideAllVisibleChars()
    {
        if (_tmpText != null)
        {
            _tmpText.maxVisibleCharacters = 0;
        }
    }

    private void OnDisable()
    {
        // Останавливаем корутины при выключении объекта
        StopTextCycle();
    }
}
