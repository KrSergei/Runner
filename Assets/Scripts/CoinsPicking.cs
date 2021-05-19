using UnityEngine;

public class CoinsPicking : MonoBehaviour
{
    public GameObject UIplay;

    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Coin"))
        {
            UIplay.GetComponent<CoinsCollector>().CalculateCoins();
        }
    }
}