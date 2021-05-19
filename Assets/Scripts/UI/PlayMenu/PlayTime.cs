using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayTime : MonoBehaviour
{

    public Text valueCurrentTime;       //��������� ����, ��� ������ �������� ������� ����

    private float playTime = 0f;        //����� ����
    private bool isRun = false;          //����, ��� ���� ��������

    
    void Update()
    {
        if (isRun)
            playTime += Time.deltaTime;
        //����� �������� �������
        valueCurrentTime.text = GetTime().ToString();
    }

    /// <summary>
    /// ������ �������
    /// </summary>
    public void StartTime()
    {
        isRun = true;
    }

    /// <summary>
    /// ����� �������
    /// </summary>
    public void PauseTime()
    {
        isRun = false;
    }

    /// <summary>
    /// ��������� � ����� �������
    /// </summary>
    public void StopTimer()
    {
        isRun = false;
        playTime = 0;
    }

    /// <summary>
    /// ������� �������� �������� �������
    /// </summary>
    /// <returns></returns>
    public float GetTime()
    {
        return Mathf.Floor(playTime);
    }
}
