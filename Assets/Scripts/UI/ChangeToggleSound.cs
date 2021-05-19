using UnityEngine;
using UnityEngine.UI;

public class ChangeToggleSound : MonoBehaviour{

    /// <summary>
    /// вывод сообщения об включение/выключении звука
    /// </summary>
    /// <param name="t"></param>
    public void ToggleChangevalue(Toggle t)
    {
        if (t.isOn)
        {
            t.GetComponentInChildren<Text>().text = "Toggle is on";
        }
        else
        {
            t.GetComponentInChildren<Text>().text = "Toggle is off";
        }
    }
}
