using UnityEngine;

public class TrapActivate : MonoBehaviour
{
    public Rigidbody rj;

    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            ActivateTrap();
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.CompareTag("Player"))
        {
            ActivateTrap();
        }
    }

    public void ActivateTrap()
    {
        rj.isKinematic = false;
    }
}

