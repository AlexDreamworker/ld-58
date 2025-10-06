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

    private TMP_Text tmpTextComponent;
    private Coroutine currentSequenceCoroutine;

    private void Awake()
    {
        tmpTextComponent = GetComponent<TMP_Text>();
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
            // Печатаем текущий текст
            yield return StartCoroutine(TypeTextRoutine(sequence.text, sequence.typingSpeed));
            
            // Ждем указанное время после печати
            yield return new WaitForSeconds(sequence.delayAfterTyping);
            
            // Стираем текст
            tmpTextComponent.text = string.Empty;
            
            // Ждем перед переходом к следующему тексту
            yield return new WaitForSeconds(sequence.delayBeforeNext);
        }

        // Все последовательности завершены
        onAllSequencesCompleted?.Invoke();
    }

    private IEnumerator TypeTextRoutine(string textToType, float typingSpeed)
    {
        tmpTextComponent.text = string.Empty;
        float delayBetweenChars = 1f / typingSpeed;

        for (int i = 0; i < textToType.Length; i++)
        {
            tmpTextComponent.text += textToType[i];
            yield return new WaitForSeconds(delayBetweenChars);
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
