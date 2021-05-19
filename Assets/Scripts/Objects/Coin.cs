using UnityEngine;

public class Coin : MonoBehaviour
{
    public ParticleSystem CoinPs;

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag =="Player")
        {
            CoinPs.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject, 0.1f);
        }
    }
}
