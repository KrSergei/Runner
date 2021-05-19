using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    public GameObject[] lowObstaclePlatform;        //Массив платформ с низкими препятствиями
    public GameObject[] highObstaclePlatform;       //Массив платформ с высокими препятствиями
    public GameObject[] StartPlatform;               //Массив платформ без препятсвий препятствиями
    public GameObject[] rampPlatform;               //Массив платформ с рампами
    public GameObject[] trapPlatform;               //Массив платформ с ловушками

    public Transform platformContainer;             //Трансформ контейнера, в который помещаются сгенерированные платформы

    public float chanceSpawnCoin = 0.5f;            //Шанс сгенерировать монеты на платформе

    [SerializeField]    
    private float timePlaying = 0f;                 //Время игры

    private bool changeDiffuclty;

    private Transform lastPlatform = null;          //Трансформ последней сгенерированной платформы

    private bool canDoSpawnArea = false;            //Флаг генерации монет на платформе

    void Start()
    {
        //Создание стартовых платформ
        CreateStartPlatform();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            CreatePlatform();
        }

        if(timePlaying < 100)
        {
            timePlaying++;
        }
    }

    /// <summary>
    /// Метод стартовой инициализации платформ
    /// </summary>
    public void StartInit()
    {
        //Генерация рандомных платформ
        for (int i = 0; i < 4; i++)
        {
            CreatePlatform();
        }
    }

    /// <summary>
    /// Метод генерации свободной платфомры
    /// </summary>
    public void CreateStartPlatform()
    {
        //Определение координат генерации платформы. Проверка на наличие последней платформы
        Vector3 spawnPlatformPosition = (lastPlatform == null) ?
            platformContainer.position :                                        //выбор в месте platformContainer, если платформа отсутсвует
            lastPlatform.GetComponent<PlatformControl>().endPoint.position;     //выбор в точке end последней платформы
  
            //Генерация платформы из выбранного массива по указанному индексу в точке создания платформы - spawnPlatformPosition и инициализация
            GameObject resultPlatform = Instantiate(
                //выбор массива по переменной choiceTypePlatform, если 0 - то выбор тип low, если 1 - то типа high, с указанием индекса indexPlatform  
                (StartPlatform[0]),
                //указание места генерации платформы, поворота платформы и родительского объекта
                spawnPlatformPosition, Quaternion.identity, platformContainer);
            lastPlatform = resultPlatform.transform;
    }
    
    /// <summary>
    /// Метод генерации платформы. Осуществляется проверка на наличие сгенерированной платформы
    /// если сгенерированной платформы нет, то платформа генерируется в точке - трансформе platformContainer,
    /// если платформа есть, то платформа генерируется в точке end последней сгенерированной платформы
    /// </summary>
    public void CreatePlatform()
    {
        //Определение координат генерации платформы. Проверка на наличие последней платформы
        Vector3 spawnPlatformPosition = (lastPlatform == null) ?
            platformContainer.position :                                        //выбор в месте platformContainer, если платформа отсутсвует
            lastPlatform.GetComponent<PlatformControl>().endPoint.position;     //выбор в точке end последней платформы

        //Определение типа выбранной платформы для генерации
        GameObject choiceTypePlatform = GeneratePlatform();

       //Генерация платформы из выбранного массива по указанному индексу в точке создания платформы - spawnPlatformPosition 
       //с указанием места генерации платформы, поворота платформы и родительского объекта
       GameObject resultPlatform = Instantiate( choiceTypePlatform,spawnPlatformPosition, Quaternion.identity, platformContainer);

        //вызвов метода случайной генерации монет на созданной платформе
        SpawnCoins(resultPlatform);

        lastPlatform = resultPlatform.transform;
    }

    private GameObject GeneratePlatform()
    {
        //Определение типа выбранной платформы в зависимости от пройденного времени игры 
        float choiceTypePlatform = Random.Range(0f, 300f) + timePlaying;

        //Определение платформы по случайному значению choiceTypePlatform и индексу из массива indexPlatform
        GameObject choicedTypePlatform = null;

        if (choiceTypePlatform <= 70f)
            choicedTypePlatform = lowObstaclePlatform[Random.Range(0, lowObstaclePlatform.Length)];

        if (choiceTypePlatform > 70f && choiceTypePlatform <= 140f)
            choicedTypePlatform = rampPlatform[Random.Range(0, rampPlatform.Length)];

        if (choiceTypePlatform > 140f && choiceTypePlatform <= 270f)
            choicedTypePlatform = highObstaclePlatform[Random.Range(0, highObstaclePlatform.Length)];

        if (choiceTypePlatform > 270f)
            choicedTypePlatform = trapPlatform[Random.Range(0, trapPlatform.Length)];

        return choicedTypePlatform;
    }

    /// <summary>
    /// Метод генерации монет на созданной платформе
    /// </summary>
    /// <param name="resultPlatform">Сгенерированная платформа</param>
    private void SpawnCoins(GameObject resultPlatform)
    {
        //Получение длинны массива зон генерации монет
        int lengthAreaSpawn = resultPlatform.GetComponent<PlatformControl>().spawnArea.Length;

        if (lengthAreaSpawn > 0)
        {
            //Генерация монет в зоне создания, если флаг canDoSpawnArea = ture,в зоне создания монет, генерируются монеты
            for (int i = 0; i < lengthAreaSpawn; i++)
            {
                //вызво метода для случайного определения разрешения генерации монет
                RandomBool();
                if (canDoSpawnArea)
                {
                    //Вызов у сгенерированной платформы метода генерации монет в разрешенной зоне 
                    resultPlatform.GetComponent<PlatformControl>().spawnArea[i].GetComponentInChildren<SpawnCones>().Spawn();
                }
            }
        }
    }

    /// <summary>
    /// Метод рандомного получения значения canDoSpawnArea
    /// </summary>
    private void RandomBool()
    {
        canDoSpawnArea = Random.Range(0f, 1f) <= chanceSpawnCoin;
    }
}
