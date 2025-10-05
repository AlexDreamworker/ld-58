using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class TimedEvent
{
    public float triggerTime;
    public UnityEvent onTimeReached;
    [HideInInspector] public bool hasBeenTriggered = false;
}

public class GameTimeEvents : MonoBehaviour
{
    [SerializeField] private List<TimedEvent> timedEvents = new List<TimedEvent>();
    private float gameStartTime;

    void Start()
    {
        gameStartTime = Time.time;
    }

    void Update()
    {
        float currentTime = Time.time - gameStartTime;
        
        foreach (var timedEvent in timedEvents)
        {
            if (!timedEvent.hasBeenTriggered && currentTime >= timedEvent.triggerTime)
            {
                timedEvent.onTimeReached?.Invoke();
                timedEvent.hasBeenTriggered = true;
            }
        }
    }

    // Методы для управления списком из инспектора
    public void AddNewTimedEvent()
    {
        timedEvents.Add(new TimedEvent());
    }

    public void RemoveTimedEvent(int index)
    {
        if (index >= 0 && index < timedEvents.Count)
        {
            timedEvents.RemoveAt(index);
        }
    }
}