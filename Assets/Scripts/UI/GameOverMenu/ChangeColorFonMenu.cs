using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorFonMenu : MonoBehaviour
{
    public GameObject FonMenu;
    private Color finishColor = new Color(0.35f, 0.16f, 0.11f, 0.83f);
    private Color startColor;

    private float time = 1.5f;          //�����, �� ������� ������ ��������� ��� ������
    private Image imageFon;

    private void Start()
    {
        startColor = FonMenu.GetComponent<Image>().color;
        imageFon = FonMenu.GetComponent<Image>();
    }

    void Update()
    {
        //����� �������� ��� ����� ����
        StartCoroutine(ChangeColorFon(startColor, finishColor, time));
    }

    /// <summary>
    /// �������� ����� ���� ��� ��������� ����
    /// </summary>
    /// <param name="startColor">��������� ���� ����</param>
    /// <param name="finishColor">�������� ���� ����</param>
    /// <param name="time">�����, �� ������� ��� ������ ���������</param>
    /// <returns></returns>
    IEnumerator ChangeColorFon(Color startColor, Color finishColor, float time)
    {
        //������� ����� ������ ��������
        float currentTime = 0;
        do
        {
            imageFon.color = Color.Lerp(startColor, finishColor, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);
    }
}
