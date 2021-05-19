using UnityEngine;
using UnityEngine.UI;

public class ChangeVolumeOnMenu : MonoBehaviour
{
    /// <summary>
    /// ������ ��������� �������������
    /// AudioSource[0] - ������� ����
    /// AudioSource[1] - ����� �� ����� ����
    /// </summary>
    public AudioSource[] AudioSource;

    private float volume;

    /// <summary>
    /// ��������� ����������� ���������
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
    /// ����� ������� ���������/���������� ����� 
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
    /// ����� ���������� ����� � ����������� �� ����, ��� ��������� ������ ����� ������
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
