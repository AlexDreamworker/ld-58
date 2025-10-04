using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WaitingTimer : MonoBehaviour
{
    public UnityEvent OnTimerEnd;

    private Coroutine _currentTimerCoroutine;
    
    public void StartTimer(float seconds)
    {
        _currentTimerCoroutine = StartCoroutine(TimerCoroutine(seconds));
    }

    public void StopTimer()
    {
        if (_currentTimerCoroutine != null)
        {
            StopCoroutine(_currentTimerCoroutine);
            _currentTimerCoroutine = null;
        }
    }

    private IEnumerator TimerCoroutine(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        
        OnTimerEnd?.Invoke();
        
        _currentTimerCoroutine = null;
    }
}
