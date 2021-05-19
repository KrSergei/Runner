using UnityEngine;
using UnityEngine.UI;

public class CoinsCollector : MonoBehaviour
{
    public Text valueCurrentCoin;        //“екстовое поле, дл€ вывода текущего количества подобранных монет
    private int collectedCoin = 0;       // оличество собранных монет

    void Update()
    {
        valueCurrentCoin.text = collectedCoin.ToString();
    }

    /// <summary>
    /// ѕодсчет собранных монет
    /// </summary>
    public void CalculateCoins()
    {
        collectedCoin ++;
    }

    /// <summary>
    /// ¬озврат количества подобранных монет
    /// </summary>
    /// <returns></returns>
    public int GetCurrentValueCoin()
    {
        return collectedCoin;
    }
}
