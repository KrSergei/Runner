using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour
{

    public Collider[] RagdollCollaiders;  //Массив коллайдеров, которые необходимо отключать при столкновении с препятсвием

    /// <summary>
    /// Отключение всех триггеров у всех коллайдеров в массиве
    /// </summary>
    public void OffTriggerCollaiders()
    {

        for (int i = 0; i < RagdollCollaiders.Length; i++)
        {
            RagdollCollaiders[i].isTrigger = false;
        }
    }
}
