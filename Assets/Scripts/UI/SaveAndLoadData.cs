using System;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveAndLoadData : MonoBehaviour
{
    int loadedRecord;               //Загруженный рекорд из файла
    float loadedVolume;             //Загруженное значение громкости из файла
    bool loadedStateSound;          //Загруженное значение вкл/выкл звука
    private int currentRecord;      //Значении текущего рекорда игры после проигрыша

    private string pathToSaveFile = "/SaveData.txt";
    [Serializable]
    class SaveData
    {
        public int savedRecord;         //поле для записи рекорда
        public float savedVolume;       //поле для записи уровня громкости
        public bool savedSoundOnOff;    //поле для записи вкл/выкл звука
    }

    public void SaveGameData(int currentRecord, float volume, bool currentStateSound)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + pathToSaveFile);
        SaveData data = new SaveData();
        //Вывзо метода для записи значения рекорда
        data.savedRecord = SaveRecord(currentRecord);
        data.savedVolume = volume;
        data.savedSoundOnOff = currentStateSound;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
    }

    public void LoadData()
    {
        //Проверка наличия файла
        if (File.Exists(Application.persistentDataPath + pathToSaveFile))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file =
              File.Open(Application.persistentDataPath + pathToSaveFile, FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            loadedRecord = data.savedRecord;
            loadedVolume = data.savedVolume;
            loadedStateSound = data.savedSoundOnOff;
        }
        else
            Debug.LogError("There is no save data!");
    }

    public int SaveRecord(int currentRecord)
    {
        int saveRecord;
        //Сравнение рекорда с загруженным
        saveRecord = (currentRecord > loadedRecord) ? saveRecord = currentRecord : saveRecord = loadedRecord;
        //возврат большего значения для записи в таблицу рекородов
        return saveRecord;
    }

    /// <summary>
    /// Метода для сброса значений в файле записи
    /// </summary>
    public void ResetSettings()
    {
        if (File.Exists(Application.persistentDataPath + pathToSaveFile))
        {
            File.Delete(Application.persistentDataPath + pathToSaveFile);
            loadedRecord = 0;
            loadedVolume = 0.0f;
            loadedStateSound = true;
            Debug.Log("Data reset complete!");
        }
        else
            Debug.LogError("No save data to delete.");
    }

    private void CloseStream()
    {

    }

    /// <summary>
    /// Возвращает значение рекорда, загруженное из файла
    /// </summary>
    /// <returns></returns>
    public int GetLoadedRecord()
    {
        return loadedRecord;
    }

    /// <summary>
    /// Возвращает значение громкости, загруженное из файла
    /// </summary>
    /// <returns></returns>
    public float GetLoadedVolume()
    {
        return loadedVolume;
    }

    /// <summary>
    /// Возвращает значение вкл/выкл звука, загруженное из файла
    /// </summary>
    /// <returns></returns>
    public bool GetLoadedStateSound()
    {
        return loadedStateSound;
    }
}
