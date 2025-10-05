using UnityEngine;
using TMPro;

public class GameClock : MonoBehaviour
{
    public TMP_Text timeText; // Ссылка на компонент текста
    private float timer = 0f; // Таймер для отслеживания времени
    private int currentHour = 11; // Текущий час
    private int currentMinute = 58; // Текущая минута

    void Start()
    {
        // Устанавливаем начальное время 11:58
        UpdateTimeDisplay();
    }

    void Update()
    {
        // Увеличиваем таймер на время, прошедшее с последнего кадра
        timer += Time.deltaTime;
        
        // Проверяем, прошла ли минута (60 секунд)
        if (timer >= 60f)
        {
            timer = 0f; // Сбрасываем таймер
            AddMinute(); // Добавляем минуту
        }
    }

    void AddMinute()
    {
        currentMinute++; // Увеличиваем минуту на 1
        
        // Если минуты достигли 60, сбрасываем и увеличиваем час
        if (currentMinute >= 60)
        {
            currentMinute = 0;
            currentHour++;
            
            // Если часы достигли 24, сбрасываем в 0
            if (currentHour >= 24)
            {
                currentHour = 0;
            }
        }
        
        UpdateTimeDisplay(); // Обновляем отображение времени
    }

    void UpdateTimeDisplay()
    {
        // Форматируем время в формате HH:mm
        timeText.text = $"{currentHour:D2}:{currentMinute:D2}";
    }
}
