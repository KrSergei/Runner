using UnityEngine;
using UnityEngine.UI;

public class CoinsCollector : MonoBehaviour
{
    public Text valueCurrentCoin;        //��������� ����, ��� ������ �������� ���������� ����������� �����
    private int collectedCoin = 0;       //���������� ��������� �����

    void Update()
    {
        valueCurrentCoin.text = collectedCoin.ToString();
    }

    /// <summary>
    /// ������� ��������� �����
    /// </summary>
    public void CalculateCoins()
    {
        collectedCoin ++;
    }

    /// <summary>
    /// ������� ���������� ����������� �����
    /// </summary>
    /// <returns></returns>
    public int GetCurrentValueCoin()
    {
        return collectedCoin;
    }
}
