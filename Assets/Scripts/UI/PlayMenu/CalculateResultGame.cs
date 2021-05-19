using UnityEngine;
using UnityEngine.UI;

public class CalculateResultGame : MonoBehaviour
{
    public Text resultTxt;              //Текстовое поле, для вывода итогового результата игры

    private PlayTime PlayTime;
    private CoinsCollector CoinsCollector;

    private int result = 0;          //Итоговое количество монет

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
    /// Метод сложения текущего времени и подобранных монет
    /// </summary>
    private void CalculateResult()
    {
        result = (int)PlayTime.GetTime() + CoinsCollector.GetCurrentValueCoin();
    }

    /// <summary>
    /// Возрат текущего результата игры 
    /// </summary>
    /// <returns></returns>
    public double GetResultAfterEndedPlay()
    {
        return result;
    }
}
