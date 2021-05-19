using UnityEngine;
using UnityEngine.UI;

public class ChangeVolumeOnMenu : MonoBehaviour
{
    /// <summary>
    /// Массив доступный аудиоресурсов
    /// AudioSource[0] - главное меню
    /// AudioSource[1] - игрок во время игры
    /// </summary>
    public AudioSource[] AudioSource;

    private float volume;

    /// <summary>
    /// Установка изначальной громкости
    /// </summary>
    public void Awake()
    {
        foreach (var item in AudioSource)
        {
            item.volume = volume;
        }
    }

    public void ChangeVolume(float volume)
    {
        foreach(var item in AudioSource)
        {
            item.volume = volume;
        }
    }

    /// <summary>
    /// метод полного включения/отключения звука 
    /// </summary>
    /// <param name="value"></param>
    public void OnOffSound(bool value)
    {
        if (value)
        {
            foreach (var item in AudioSource)
            {
                item.Play();
            }
        }
        else
        {
            foreach (var item in AudioSource)
            {
                item.Stop();
            }
        }
    }

    /// <summary>
    /// метод отключения звука в зависимости от того, где находится сейчас фокус игрока
    /// </summary>
    public void OnOffSound(int index)
    {
        AudioSource[index].Stop();
    }

    public float GetVolume()
    {
        return volume;
    }
}
