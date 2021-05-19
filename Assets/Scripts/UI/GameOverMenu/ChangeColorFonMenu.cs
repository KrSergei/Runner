using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ChangeColorFonMenu : MonoBehaviour
{
    public GameObject FonMenu;
    private Color finishColor = new Color(0.35f, 0.16f, 0.11f, 0.83f);
    private Color startColor;

    private float time = 1.5f;          //Время, за которое должен смениться фон экрана
    private Image imageFon;

    private void Start()
    {
        startColor = FonMenu.GetComponent<Image>().color;
        imageFon = FonMenu.GetComponent<Image>();
    }

    void Update()
    {
        //Вызов корутины для смены фона
        StartCoroutine(ChangeColorFon(startColor, finishColor, time));
    }

    /// <summary>
    /// Корутина смены фона при окончании игры
    /// </summary>
    /// <param name="startColor">стартовый цвет фона</param>
    /// <param name="finishColor">финишный цвет фона</param>
    /// <param name="time">время, за которое фон должен смениться</param>
    /// <returns></returns>
    IEnumerator ChangeColorFon(Color startColor, Color finishColor, float time)
    {
        //текущее время работы корутины
        float currentTime = 0;
        do
        {
            imageFon.color = Color.Lerp(startColor, finishColor, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);
    }
}
