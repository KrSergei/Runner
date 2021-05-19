using UnityEngine;
using UnityEngine.UI;

public class CalculateResultGame : MonoBehaviour
{
    public Text resultTxt;              //��������� ����, ��� ������ ��������� ���������� ����

    private PlayTime PlayTime;
    private CoinsCollector CoinsCollector;

    private int result = 0;          //�������� ���������� �����

    void Start()
    {
        PlayTime = GetComponent<PlayTime>();
        CoinsCollector = GetComponent<CoinsCollector>();
    }

    void Update()
    {
        CalculateResult();
        resultTxt.text = result.ToString();
    }

    /// <summary>
    /// ����� �������� �������� ������� � ����������� �����
    /// </summary>
    private void CalculateResult()
    {
        result = (int)PlayTime.GetTime() + CoinsCollector.GetCurrentValueCoin();
    }

    /// <summary>
    /// ������ �������� ���������� ���� 
    /// </summary>
    /// <returns></returns>
    public double GetResultAfterEndedPlay()
    {
        return result;
    }
}
