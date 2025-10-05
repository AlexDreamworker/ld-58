using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtomicBombsController : MonoBehaviour
{
    [Header("Objects to Control")]
    public List<GameObject> targetObjects = new List<GameObject>();
    
    [Header("Timer Settings")]
    [SerializeField] private float minRandomDelay = 3f;
    [SerializeField] private float maxRandomDelay = 8f;
    [SerializeField] private float objectsActiveTime = 2f;
    
    private Coroutine cycleCoroutine;
    
    void Start()
    {
        // Изначально выключаем все объекты
        SetAllObjectsState(false);
        
        // Запускаем цикл
        StartCycle();
    }
    
    void OnEnable()
    {
        StartCycle();
    }
    
    void OnDisable()
    {
        StopCycle();
    }
    
    public void StartCycle()
    {
        if (cycleCoroutine != null)
            StopCoroutine(cycleCoroutine);
            
        cycleCoroutine = StartCoroutine(CycleRoutine());
    }
    
    public void StopCycle()
    {
        if (cycleCoroutine != null)
        {
            StopCoroutine(cycleCoroutine);
            cycleCoroutine = null;
        }
    }
    
    private IEnumerator CycleRoutine()
    {
        while (true)
        {
            // Ждем случайное время перед включением объектов
            float randomDelay = Random.Range(minRandomDelay, maxRandomDelay);
            yield return new WaitForSeconds(randomDelay);
            
            // Включаем все объекты
            SetAllObjectsState(true);
            
            // Ждем фиксированное время активности
            yield return new WaitForSeconds(objectsActiveTime);
            
            // Выключаем все объекты
            SetAllObjectsState(false);
        }
    }
    
    private void SetAllObjectsState(bool state)
    {
        foreach (GameObject obj in targetObjects)
        {
            if (obj != null)
                obj.SetActive(state);
        }
    }
    
    // Методы для управления из других скриптов
    public void SetRandomDelayRange(float min, float max)
    {
        minRandomDelay = min;
        maxRandomDelay = max;
    }
    
    public void SetActiveTime(float time)
    {
        objectsActiveTime = time;
    }
    
    // Контекстное меню для тестирования
    [ContextMenu("Force Activate Objects")]
    private void ForceActivateObjects()
    {
        SetAllObjectsState(true);
    }
    
    [ContextMenu("Force Deactivate Objects")]
    private void ForceDeactivateObjects()
    {
        SetAllObjectsState(false);
    }
    
    [ContextMenu("Restart Cycle")]
    private void RestartCycle()
    {
        StopCycle();
        StartCycle();
    }
}