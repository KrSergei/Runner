using System;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveAndLoadData : MonoBehaviour
{
    int loadedRecord;               //����������� ������ �� �����
    float loadedVolume;             //����������� �������� ��������� �� �����
    bool loadedStateSound;          //����������� �������� ���/���� �����
    private int currentRecord;      //�������� �������� ������� ���� ����� ���������

    private string pathToSaveFile = "/SaveData.txt";
    [Serializable]
    class SaveData
    {
        public int savedRecord;         //���� ��� ������ �������
        public float savedVolume;       //���� ��� ������ ������ ���������
        public bool savedSoundOnOff;    //���� ��� ������ ���/���� �����
    }

    public void SaveGameData(int currentRecord, float volume, bool currentStateSound)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + pathToSaveFile);
        SaveData data = new SaveData();
        //����� ������ ��� ������ �������� �������
        data.savedRecord = SaveRecord(currentRecord);
        data.savedVolume = volume;
        data.savedSoundOnOff = currentStateSound;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game data saved!");
    }

    public void LoadData()
    {
        //�������� ������� �����
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
        //��������� ������� � �����������
        saveRecord = (currentRecord > loadedRecord) ? saveRecord = currentRecord : saveRecord = loadedRecord;
        //������� �������� �������� ��� ������ � ������� ���������
        return saveRecord;
    }

    /// <summary>
    /// ������ ��� ������ �������� � ����� ������
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
    /// ���������� �������� �������, ����������� �� �����
    /// </summary>
    /// <returns></returns>
    public int GetLoadedRecord()
    {
        return loadedRecord;
    }

    /// <summary>
    /// ���������� �������� ���������, ����������� �� �����
    /// </summary>
    /// <returns></returns>
    public float GetLoadedVolume()
    {
        return loadedVolume;
    }

    /// <summary>
    /// ���������� �������� ���/���� �����, ����������� �� �����
    /// </summary>
    /// <returns></returns>
    public bool GetLoadedStateSound()
    {
        return loadedStateSound;
    }
}
