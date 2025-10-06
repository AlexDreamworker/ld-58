using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class IntroTexts : MonoBehaviour
{
    [System.Serializable]
    public class IntroTextSequence
    {
        [TextArea(3, 10)]
        public string text;
        public float typingSpeed = 10f;
        public float delayAfterTyping = 2f;
        public float delayBeforeNext = 1f;
    }

    [SerializeField] private List<IntroTextSequence> textSequences = new List<IntroTextSequence>();
    [SerializeField] private UnityEvent onAllSequencesCompleted;
    
    // НОВОЕ: Добавляем поле для звукового клика и AudioSource
    [SerializeField] private AudioClip typingSound;
    [SerializeField] private AudioSource audioSource;

    private TMP_Text tmpTextComponent;
    private Coroutine currentSequenceCoroutine;

    private void Awake()
    {
        tmpTextComponent = GetComponent<TMP_Text>();
        // НОВОЕ: Если AudioSource не назначен в инспекторе, попробуем получить его с этого же GameObject
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        if (textSequences.Count > 0)
        {
            StartTextSequence();
        }
    }

    public void StartTextSequence()
    {
        if (currentSequenceCoroutine != null)
        {
            StopCoroutine(currentSequenceCoroutine);
        }
        currentSequenceCoroutine = StartCoroutine(TextSequenceRoutine());
    }

    private IEnumerator TextSequenceRoutine()
    {
        foreach (IntroTextSequence sequence in textSequences)
        {
            yield return StartCoroutine(TypeTextRoutine(sequence.text, sequence.typingSpeed));
            yield return new WaitForSeconds(sequence.delayAfterTyping);
            tmpTextComponent.text = string.Empty;
            yield return new WaitForSeconds(sequence.delayBeforeNext);
        }
        onAllSequencesCompleted?.Invoke();
    }

    private IEnumerator TypeTextRoutine(string textToType, float typingSpeed)
    {
        tmpTextComponent.text = string.Empty;
        float delayBetweenChars = 1f / typingSpeed;

        for (int i = 0; i < textToType.Length; i++)
        {
            tmpTextComponent.text += textToType[i];
            
            // НОВОЕ: Проигрываем звук на каждом шаге цикла
            PlayTypingSound();
            
            yield return new WaitForSeconds(delayBetweenChars);
        }
    }

    // НОВОЕ: Метод для воспроизведения звука
    private void PlayTypingSound()
    {
        // Проверяем, что у нас есть звук и источник звука
        if (typingSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(typingSound);
        }
    }

    public void StopTextSequence()
    {
        if (currentSequenceCoroutine != null)
        {
            StopCoroutine(currentSequenceCoroutine);
            currentSequenceCoroutine = null;
        }
        tmpTextComponent.text = string.Empty;
    }
}
