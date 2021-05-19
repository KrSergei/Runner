using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayTime : MonoBehaviour
{

    public Text valueCurrentTime;       //Текстовое поле, для вывода текущего времени игры

    private float playTime = 0f;        //Время игры
    private bool isRun = false;          //Флаг, что игра запущена

    
    void Update()
    {
        if (isRun)
            playTime += Time.deltaTime;
        //Вывод текущего времени
        valueCurrentTime.text = GetTime().ToString();
    }

    /// <summary>
    /// Запуск таймера
    /// </summary>
    public void StartTime()
    {
        isRun = true;
    }

    /// <summary>
    /// Пауза таймера
    /// </summary>
    public void PauseTime()
    {
        isRun = false;
    }

    /// <summary>
    /// Остановка и сброс таймера
    /// </summary>
    public void StopTimer()
    {
        isRun = false;
        playTime = 0;
    }

    /// <summary>
    /// Возврат текущего значения таймера
    /// </summary>
    /// <returns></returns>
    public float GetTime()
    {
        return Mathf.Floor(playTime);
    }
}
