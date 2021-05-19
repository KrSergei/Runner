using System;
using System.Collections;
using UnityEngine;

public class WorldController : MonoBehaviour
{

    public float SpeedPlatform = 0f;

    public WorldBuilder worldBuilder;

    public delegate void TryToDeleteAndAddPlatform();
    public event TryToDeleteAndAddPlatform OnPlatformMovement;
    
    //public event ChangeSpeedPlatform changeSpeedPlatform;

    public static WorldController instance;

    private bool isWork = true;
    /// <summary>
    /// Проверка на наличие объекта WorldController. Если есть, то объект уничтожается и выход из функции.
    /// Если объекта нет, текущий объект становиться WorldController
    /// </summary>
    private void Awake()
    {
        if (WorldController.instance != null)
        {
            Destroy(gameObject);
            return;
        }
        WorldController.instance = this;
    }

    public void SetSpeedPlatform(float speed)
    {
        SpeedPlatform = speed;
    }

    private void OnDestroy()
    {
        WorldController.instance = null;
    }

    void Update()
    {
        //задание двжение платформе
        transform.position -= Vector3.forward * SpeedPlatform * Time.deltaTime;
    }

    /// <summary>
    /// Запуск корутины при начале игры/
    /// Запускается по нажатию кнопки старт. Настраивается в компонене button, как реакция на нажатие кнопки
    /// </summary>
    public void StartCoroutineAddPlatform()
    {
        StartCoroutine(OnPlatformMovementCoroutine());
    }

    /// <summary>
    /// Остановка корутины при выходе из игры
    /// </summary>
    public void StopCoroutine()
    {
        isWork = false;
        StopCoroutine(OnPlatformMovementCoroutine());
    }

    IEnumerator OnPlatformMovementCoroutine()
    {
        while (isWork)
        {
            //Паузка перед вызовом 24 сек
            yield return new WaitForSecondsRealtime(25f);
            //Проверка на null, если OnPlatFormMovement не равно null, вызывается OnPlatFormMovement
            OnPlatformMovement?.Invoke();
        }
    }   
}
