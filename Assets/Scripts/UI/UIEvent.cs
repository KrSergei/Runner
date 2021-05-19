using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIEvent : MonoBehaviour
{
    /// <summary>
    /// Массив canvas. 
    /// Canvas[0] -  Canvas start menu
    /// Canvas[1] -  Canvas setting menu
    /// Canvas[2] -  Player game menu 
    /// Canvas[3] -  Pause menu
    /// Canvas[4] -  Game over menu
    /// Canvas[5] -  Record menu
    /// </summary>
    public Canvas[] canvas;

    public Text RecordCurrentValue;                 //Значение рекорда за все игры, которое выводится на стартовый экран
    private int currentRecord;                      //Текущее значение рекорда последней игры
    private float currentVolume;                    //Текущий уровень громкости
    private bool toggleCurrentState;                //Текущие состояние звука(вкл/выкл)

    [SerializeField]
    private GameObject toggleSound;
    [SerializeField]
    private GameObject sliderSound;
    [SerializeField]
    private GameObject audioManager;

    private SaveAndLoadData DataSaveAndLoad;        //Загрузчик данных в файл

    private string soundToggleCurrrentStateOn = "Sound is On",      //Выводимый текст на экран при включенном звуке
                   soundToggleCurrrentStateOff = "Sound is Off";    //Выводимый текст на экран при выключенном звуке

    private void Start()
    {
        //Инициализация загрузчика данных
        DataSaveAndLoad = GetComponent<SaveAndLoadData>();
        InitStartMenu();
    }

    /// <summary>
    /// Инициализация стартового меню
    /// </summary>
    private void InitStartMenu()
    {
        //активация стартового меню
        canvas[0].gameObject.SetActive(true);

        //Деактивация остальных меню
        for (int i = 1; i < canvas.Length; i++)
        {
            canvas[i].gameObject.SetActive(false);
        }
        //Выов метода для загрузки данных из файла сохранения
        DataSaveAndLoad.LoadData();
        //Вывод значения рекорда на экран из сохранненого файла
        RecordCurrentValue.text = DataSaveAndLoad.GetLoadedRecord().ToString();
        //Инициализация значения текущего стостояния звука(вкл/выкл)
        toggleCurrentState = DataSaveAndLoad.GetLoadedStateSound();
    }

    public void ShowMenuGameOver()
    {
        //Остановка времени
        StopTimer();
        //Активация меню окончания игры
        canvas[4].gameObject.SetActive(true);
        //Вызов метод сравнения текущего значения с рекордом
        CompareRecordAndValueLastGame();
    }

    #region главное меню
    /// <summary>
    /// Реакция на нажатие кнопки старт.
    /// Деактивация стартового меню, активация игрового меню.
    /// Запуск игры
    /// Настройка реакций на кнопку в компоненте button:
    /// Вызов метода StartInit в Worldbulder, для запуска генерации платформ.
    /// Вызов метода StopTime в PlayTime для обнуления времени игры.
    /// Вызов метода StartTime в PlayTime для начала отчета времени игры.
    /// </summary>
    public void ButtonStartAction()
    {
        //Деактивации стартового меню
        ChangeCurrentActive(canvas[0].gameObject);
        //активация меню игры
        ChangeCurrentActive(canvas[2].gameObject);
        //вывов метода для установки переключателя звука (вкл/выкл)
        ToggleChangeSound(toggleCurrentState);
    }

    /// <summary>
    /// Реакция на нажитие кнопки настройки. Переход в меню SETTINGS. Деактивация текущего меню
    /// </summary>
    public void ButtonSettingsAction()
    {
        //Вызвов метода для изменения текущего стостояния стартового меню
        ChangeCurrentActive(canvas[0].gameObject);
        //Вызвов метода для изменения текущего стостояния меню настроек
        ChangeCurrentActive(canvas[1].gameObject);
        //вызов метода вкл/выкл звука в соответсвии с toggleCurrentState
        ToggleChangeSound(toggleCurrentState);
        //Установка громкости в соответствии с загруженным значением из файла
        //if (toggleCurrentState)
        //    SliderAction(currentVolume);

    }

    /// <summary>
    /// Реакция на нажитие кнопки выход. Выход из игры
    /// </summary>
    public void ButtonExitAction()
    {
        Application.Quit();
        Debug.Log("Exit");
    }
    #endregion

    #region меню настроек

    /// <summary>
    /// Реакция на нажитие кнопки Back to main menu. Возврат из текущего меню, с деактивацией меню настройки
    /// </summary>
    public void ButtonActionBackToMainMenu()
    {
        //Вызвов метода для изменения текущего стостояния стартового меню
        ChangeCurrentActive(canvas[0].gameObject);
        //Вызвов метода для изменения текущего стостояния меню настроек
        ChangeCurrentActive(canvas[1].gameObject);
    }

    /// <summary>
    /// Реакция на нажитие кнопки Apply Change. Сохранение текущих настроек в конфигурационный файл
    /// </summary>
    public void ButtonActionSaveChange()
    {
        GetVolume();
        DataSaveAndLoad.SaveGameData(currentRecord, currentVolume, toggleCurrentState);
    }

    /// <summary>
    /// Метод загрузки данных из файла сохранения
    /// </summary>
    public void ButtonActionLoadData()
    {
        //вызво метода загрузки данных из файла
        DataSaveAndLoad.LoadData();
        //Инициализация рекорда данными из файла   
        RecordCurrentValue.text = DataSaveAndLoad.GetLoadedRecord().ToString();
        //Инициализация значения громкости из файла
        currentVolume = DataSaveAndLoad.GetLoadedVolume();
        //Инициализация состояния звука(вкл/выкл) данными из файла
        toggleCurrentState = DataSaveAndLoad.GetLoadedStateSound();
        //Включение/отключение toggle
        ToggleChangeSound(toggleCurrentState);
    }

    /// <summary>
    /// Обработка включения/выключения звука и вывод сообщения об состоянии звука 
    /// </summary>
    /// <param name="value"></param>
    public void ToggleChangeSound(bool value)
    {
        //Установка флага toggleCurrentState в соответсвие со значением переключателя
        toggleSound.GetComponentInChildren<Toggle>().isOn = value;

        //Вызвов метода для изменения текущего стостояния slider в соответсвии с значением value
        sliderSound.SetActive(value);

        //замена текста в компоненте label в поле text в зависимости от значения value
        toggleSound.GetComponentInChildren<Text>().text = (value)
            ? toggleSound.GetComponentInChildren<Text>().text = soundToggleCurrrentStateOn
            : toggleSound.GetComponentInChildren<Text>().text = soundToggleCurrrentStateOff;

        //Установка toggleCurrentState в значение value 
        SetToggleState(value);

        //установка уровня громкости slider, если value = true
        if (value)
            SetVolume();
    }

    /// <summary>
    /// Вызво метода для отключения/включения звука в зависимости от переданного параметра value
    /// </summary>
    /// <param name="value">true - вкл/false -  выкл</param>
    /// <returns></returns>
    private void SetToggleState(bool value)
    {
        //изменение toggleCurrentState при изменении состояния toggle
        toggleCurrentState = value;
        audioManager.GetComponent<ChangeVolumeOnMenu>().OnOffSound(value);
    }

    /// <summary>
    ///  Уровень громкости от Slider
    /// </summary>
    /// <returns></returns>
    public float GetVolume()
    {
        currentVolume = sliderSound.GetComponent<Slider>().value;
        return currentVolume;
    }

    /// <summary>
    /// Установка громкости
    /// </summary>
    private void SetVolume()
    {
        sliderSound.GetComponent<Slider>().value = currentVolume;
    }

    /// <summary>
    /// Состояние переключателя вкл/выкл звука
    /// </summary>
    /// <returns></returns>
    public bool GetCurrentStateSound()
    {
        return toggleCurrentState;
    }

    /// <summary>
    /// установка громкости
    /// </summary>
    /// <param name="value"></param>
    public void SliderAction(float value)
    {
        audioManager.GetComponent<ChangeVolumeOnMenu>().ChangeVolume(value);
    }
    #endregion

    #region меню игры

    /// <summary>
    /// Вызов метода пауза 
    /// </summary>
    public void StopTimer()
    {
        canvas[2].GetComponent<PlayTime>().PauseTime();
    }

    #endregion

    #region меню паузы

    /// <summary>
    /// Пауза в игре
    /// </summary>
    public void Pause()
    {
        //Проверяем, находится ли игрок в главном меню или в меню настройки, если да, то установка IsPaused = false и выход
        if (canvas[0].gameObject.activeInHierarchy || canvas[1].gameObject.activeInHierarchy)
        {
            return;
        }
        else
        {   
            //Активация меню паузы
            canvas[3].gameObject.SetActive(true);
            //Установка масштаба времени 0
            Time.timeScale = 0;

            PlayerController.IsPaused = true;
        }
    }

    public void Resume()
    {
        //Деактивация меню паузы
        //ChangeCurrentActive(canvas[3].gameObject);
        canvas[3].gameObject.SetActive(false);
        //Установка масштаба времени 1
        Time.timeScale = 1;
        Debug.Log("Resume to game");
        PlayerController.IsPaused = false;
    }

    public void BackMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
        PlayerController.IsPaused = false;
    }
    #endregion

    #region меню Game Over

    /// <summary>
    /// Меню перезапуска игры после окончания игры 
    /// </summary>
    public void ButtonActionRestartGame()
    {
        //Деактивации меню game over
        ChangeCurrentActive(canvas[4].gameObject);
        //активация меню игры
        ChangeCurrentActive(canvas[2].gameObject);
    }

    /// <summary>
    /// Сравнение результата игры с рекордом, если больше, то показ меню поздравления
    /// </summary>
    private void CompareRecordAndValueLastGame()
    {
        int number;
        bool success = int.TryParse(RecordCurrentValue.text, out number);

        if (success)
        {
            if(number < currentRecord)
                ChangeCurrentActive(canvas[5].gameObject);
        } 
    }

    #endregion
    public void SaveCurrentRecord()
    {
        //Получение значения текущего рекорда
        currentRecord = (int)GetComponentInChildren<CalculateResultGame>().GetResultAfterEndedPlay();
        //Сохранение текущах данных
        DataSaveAndLoad.SaveGameData(currentRecord, currentVolume, toggleCurrentState);
    }

    /// <summary>
    /// Изменения текущей активности переданного gameObject на противоположное от текущего значения state
    /// </summary>
    /// <param name="gameObject">принимаемый объект</param>
    /// <param name="state">состояние на которое необходимо изменить активность объекта</param>
    private void ChangeCurrentActive(GameObject gameObject)
    {
        gameObject.SetActive(!gameObject.activeInHierarchy);
    }
}
