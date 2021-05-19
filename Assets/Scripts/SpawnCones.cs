using UnityEngine;

public class SpawnCones : MonoBehaviour
{
    public Transform[] SpawnSpots;      //Массив точек генерации
    public GameObject SpawnObj;         //Генерируемый объект
    
    /// <summary>
    /// Метод генерации объекта
    /// </summary>
    public  void Spawn()
    { 
        for (int i = 0; i < SpawnSpots.Length; i++)
        {
            //Создание объектов в точках генерации массива
            Instantiate(SpawnObj,
                        SpawnSpots[i].position,
                        Quaternion.Euler(90, 0, 0),
                        gameObject.transform);
        }
    }
}
