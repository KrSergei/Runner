using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PSController : MonoBehaviour
{
    public ParticleSystem PSLeftLeg,        //система частиц левой ноги
                          PSRightLeg,       //система частиц правой ноги
                          PSTopCrash,       //система частиц для эффекта столкновения с высоким препятствием
                          PSBottomCrash;    //система частиц для эффекта столкновения с низким препятствием


    /// <summary>
    /// запуск эффекта пыли для левой ноги
    /// </summary>
    public void PlayLeftLegPS()
    {
        PSLeftLeg.GetComponent<ParticleSystem>().Play();
    }

    /// <summary>
    /// запуск эффекта пыли для правой ноги
    /// </summary>
    public void PlayRightLegPS()
    {
        PSRightLeg.GetComponent<ParticleSystem>().Play();
    }

    /// <summary>
    /// запуск эффекта столкновения с высоким препятствием
    /// </summary>
    public void PlayTopCrash()
    {
        PSTopCrash.GetComponent<ParticleSystem>().Play();
    }

    /// <summary>
    /// запуск эффекта столкновения с низким препятствием
    /// </summary>
    public void PlayBottomCrash()
    {
        PSBottomCrash.GetComponent<ParticleSystem>().Play();
    }

}
