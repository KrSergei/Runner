using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    #region Поля
    public static bool IsPaused = false;        //Пауза в игре

    private const float stopSpeedPlatform = 0f;     //Скорость для остановки платформ
    private const float normalSpeedPlatform = 4f;   //Нормальная скорость платформы

    public Transform UpperRaycastSpot;          //Трансформ для позиции верхнего луча

    public float DistanceGround,                //Дистанция луча hit, которая указывает, что игрок находится на поверхности
                 GravytyForce,                  //Показатель силы гравитации
                 DistanceMoveToSide = 2.5f,     //Расстояние, которое проходит игрок за одну анимацию кувырка в сторону
                 DistanceMoveToUp = 2.5f,       //Расстояние, которое проходит игрок за одну анимацию прыжка 
                 ClimbingDistance,              //Расстояние перемещеиния при взбирании на вертикальную поверхность
                 JumpForce;                     //Сила прыжка

    public bool IsInMovement = false,           //Указатель состояния выполнения анимации
                IsGround = false,               //Указатель нахождения на поверхности
                IsClimbing = false,             //Указатель состояния взбирания на стены
                IsRolling = false,              //Указатель состояния кувырка
                IsAlive = true;                 //Указатель состояния игрока

    public GameObject UIManager,
                      PlatformContainer;

    delegate void SetSpeedForPlatform(float speed);
    SetSpeedForPlatform setSpeedForPlatform;

    private UIEvent UIEvent;                    //Объект UIEvent
    private Animator animator;
    private CharacterController cc;

    private Vector3 runVector,                  //Вектор направления движения персонажа 
                    moveVector,                 //Вектор направления движения персонажа при выполении перемещения
                    gravity;                    //Вектор направления гравитации

    private Vector3 ccCenterNorm = new Vector3(0, .91f, 0),             //Вектор центра коллайдера контроллера по умоланию          
                    ccCenterRoll = new Vector3(0, .19f, 0),             //Вектор центра коллайдера контроллера при анимации кувырка
                    ccCenterJump = new Vector3(0, 1.6f, 0);            //Вектор центра коллайдера контроллера при анимации прыжка

    private RaycastHit hitGround,
                       hitForward; 

    private float timeAnimationClip,            //Время длительности анимационного клипа
                  currentDistance = 0f,         //Текущая дистанция
                  currentDirection = 0f,        //Текущее направление   
                  currentSpeed,                 //Текущая скорость 
                  tmpDist,                      //Дистанция, пройденная за один кадр анимации 
                  chooseDistance;               //Показатель дистанции, на которую необходимо передвинуть персонажа в зависимости от выбранного направления

    private float directionSide,                //Направление управления влево-вправо
                  directionForward;             //Направление управления вниз-вверх

    private float ccHeightHorm = 1.91f,          //Высота коллайдера контроллера по умолчанию
                  ccHeightForRollAndJump = .4f;  //Высота коллайдера контроллера при кувырке и прыжке 

    private string nameWorkedTrigger;             //Имя триггера, по которому включается анимация
    private bool isClimbing = false;              //Указатель, что выполняется анимация взбирания на стену

    #endregion
    void Start()
    {
        //Определения значения Аниматора
        animator = GetComponent<Animator>();
        //Определение значения контроллера
        cc = GetComponent<CharacterController>();
        //Получение компонента UIEvent
        UIEvent = UIManager.GetComponent<UIEvent>();
        //Инициализация делегата сслыкой на метод
        setSpeedForPlatform = PlatformContainer.GetComponent<WorldController>().SetSpeedPlatform;
    }
    
    void Update()
    {
        //Направляем луч вниз, с позиции игрока 
        Ray rayGround = new Ray(transform.position, - Vector3.up);

        //Перезагрузка сцены по клавише R
        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(0);
            SetInactive();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                UIEvent.Resume();

            }
            //Если IsPaused = false, вызов метода GetPause()
            else
            {
                UIEvent.Pause();
            }
        }

        if (!isClimbing)
        {
            //int layer = (1 << LayerMask.NameToLayer("Collectible"));
            ////Инвертирование маски
            //layer = ~layer;
            //Запуск луча на определение дистанции до препятствия.
            //Physics.Raycast(rayGround, out hitGround, layer);
            Physics.Raycast(rayGround, out hitGround);

        }

        //Определение высоты игрока, если дистанция луча меньше либо равна расстоянию distanceGround, то isGround = true и отключение вектора графитации 
        //иначе isGround = false и задание игроку вектора гравитации 
        if ((hitGround.distance <= DistanceGround))
        {
            IsGround = true;
            gravity = Vector3.zero;
            directionSide = Input.GetAxisRaw("Horizontal");
            directionForward = Input.GetAxisRaw("Vertical");
        }
        else 
        {
            IsGround = false;
            gravity += Physics.gravity * Time.deltaTime * GravytyForce;
        }

        if (!IsInMovement && IsGround)
        {
            //вызов метода для перемещения персонажа влево-вправо          
            if (directionSide != 0)
            {
                IsInMovement = true;
                StartCoroutine(MoveToSideCoroutine());
            }
            //вызов метода для выполения анимаций кувырка, прыжка или взбирания на стену
            if (directionForward != 0)
            {
                IsInMovement = true;
                //Направляем луч вперед, с позиции игрока 
                Ray rayUpper = new Ray(UpperRaycastSpot.transform.position, Vector3.forward);
                //Получения номера маски для игнорирования объектов слоя "Trap"
                int layerMask = (1 << LayerMask.NameToLayer("Trap"));
                //Инвертирование маски
                layerMask = ~layerMask;
                //Запуск луча и проверка наличия высокого препятствия, при этом игнорируются объекты находящиеся на слое "Trap"
                //Если есть нужный объект, запуск корутины DoClimb(), иначе вызов корутины MoveToUpCoroutine()
                if (Physics.Raycast(rayUpper, out hitForward, 3f, layerMask))
                {
                    IsClimbing = true;
                    StartCoroutine(DoClimb());
                } else
                {
                    StartCoroutine(MoveToUpCoroutine());
                }
            }
        }
        if (IsAlive)
        {
            //Вычисление вектора движения с учетом гравитации
            runVector += gravity;
            runVector *= Time.deltaTime;
            //Задание вектора движения  персонажу
            cc.Move(runVector);
        }
    }

    /// <summary>
    /// метод перемещения влево-вправо и запуска анимаци кувырка в зависимости от значения directionSide
    /// </summary>
    IEnumerator MoveToSideCoroutine()
    {
        //установка nameWorkedTrigger в зависимости от directionSide
        if (directionSide < 0)
            nameWorkedTrigger = "Left";
        if (directionSide > 0)
            nameWorkedTrigger = "Right";

        animator.SetTrigger(nameWorkedTrigger);

        //Установка направления передвижения в зависимости от значения directionSide, если больше 0 - движение вправо, если меньше 0 - движение влево
        currentDirection = directionSide;
        //Установка текущей дистанции равной длине перемещения
        currentDistance = DistanceMoveToSide;

        //Получение длины текущего проигрываеммой анимации
        //timeAnimationClip = animator.GetCurrentAnimatorStateInfo(0).length;

        timeAnimationClip = animator.GetCurrentAnimatorClipInfo(0).Length;

        //время проигрывания текущей анимации
        float timeElapsed = 0;

        //выполнение цикла передвижения персонажа пока время выполнения перемещения меньше времени длительности анимации
        while (timeElapsed < timeAnimationClip)
        {
            //Установка вектора движения
            moveVector = Vector3.right;

            timeElapsed += Time.deltaTime;

            //При движении влево, для избежания перемещения вверх при действии гравитации, гравитация уможается на -1, 
            //что бы показатель графитации был отрицательным, иначе 
            if (nameWorkedTrigger.Equals("Left"))
            {
                //Задание вектору движения гравитации
                moveVector += gravity * - 1;
            }
            else
            {
                //Задание вектору движения гравитации
                moveVector += gravity;
            }
 
            //вычисление скорости движения персонажа во время анимации
            float playerSpeed = (currentDistance / timeAnimationClip);

            //вычисление вектора движения
            runVector = moveVector * currentDirection * playerSpeed * Time.deltaTime;
            
            //установка вектора движения для контроллера персонажа
            cc.Move(runVector);

            yield return null;
        }
        //Сброс флагов и вектора направления
        IsInMovement = false;
        currentDistance = 0;
        nameWorkedTrigger = "";
        moveVector = Vector3.zero;
    }

    /// <summary>
    /// метод перемещения и запуска анимаций кувырка, прыжка и взбирани я на стену, в зависимости от значения directionForward
    /// </summary>
    IEnumerator MoveToUpCoroutine()
    {
        //установка nameWorkedTrigger в зависимости от directionForward
        if (directionForward < 0)
        {
            nameWorkedTrigger = "Roll";
            IsRolling = true;
        }

        if (directionForward > 0)
        {
            nameWorkedTrigger = "Jump";
        }

        animator.SetTrigger(nameWorkedTrigger);

        if (directionForward != 0)
        {
            //Установка текущей дистанции равной длине перемещения для анимации прыжка
            currentDistance = DistanceMoveToUp;
        }

        //Получение длины текущего проигрываеммой анимации
        timeAnimationClip = (animator.GetCurrentAnimatorStateInfo(0).length);

        //время проигрывания текущей анимации
        float timeElapsed = 0;

        // выполнение цикла передвижения персонажапока время выполнения перемещения меньше времени длительности анимации
        while (timeElapsed < timeAnimationClip)
        {
            //вычисление скорости движения персонажа во время анимации
            float playerJumpSpeed = (currentDistance / timeAnimationClip);

            //Дистанция, которую проходит персонаж за один кадр
            tmpDist = Time.deltaTime * playerJumpSpeed;

            runVector = moveVector * playerJumpSpeed * tmpDist;

            cc.Move(runVector);

            currentDistance -= tmpDist;

            yield return null;

            timeElapsed += Time.deltaTime;
        }
    }

    /// <summary>
    /// Метод для смены позиции игрока между точками ClimbSpot для анимации Climb
    /// </summary>
    IEnumerator DoClimb()
    {
        isClimbing = true;

        //получаем длинну массива точек подьема на препятсвии
        int countSpots = hitForward.collider.gameObject.GetComponent<ClimbSpot>().spotsClimb.Length;

        //создание масисва точек подьема
        Transform[] climbSpots = new Transform[countSpots];

        //заполнение массива точек подъема
        for (int i = 0; i < countSpots; i++)
        {
            climbSpots[i] = hitForward.collider.gameObject.GetComponent<ClimbSpot>().spotsClimb[i];
        }

        //установка скорости движения платформы в 0 для корректной анимации взбирания  
        setSpeedForPlatform(stopSpeedPlatform);

        //Получение координаты x игрока
        float playerCurrentPosX = transform.position.x;

        animator.SetTrigger("Climb");

        //Получение длины текущего проигрываеммой анимации
        timeAnimationClip = (animator.GetCurrentAnimatorStateInfo(0).length);

        int index = 0;

        Transform nextClimbSpot = climbSpots[index];

        //строим вектор движения к точке от места нахождения игрока, с учетом позиции игрока по оси х
        Vector3 resultVectorMove = new Vector3(playerCurrentPosX, nextClimbSpot.position.y, nextClimbSpot.position.z);

        while (index < climbSpots.Length)
        {
            float playerClimbSpeed = (DistanceMoveToUp / timeAnimationClip);

            if (Vector3.Distance(transform.position, resultVectorMove) < 0.2f && index < climbSpots.Length - 1)
            {
                index++;
                nextClimbSpot = climbSpots[index];
            }
            else if (Vector3.Distance(transform.position, resultVectorMove) < 0.2f && (index < climbSpots.Length))
            {
                yield break;
            }

            resultVectorMove = new Vector3(playerCurrentPosX, nextClimbSpot.position.y, nextClimbSpot.position.z);

            //вычисление вектора направлеиня движения к точке подъема
            moveVector = resultVectorMove - transform.position;

            //задание вектора направления
            runVector = moveVector * playerClimbSpeed * Time.deltaTime;

            cc.Move(runVector);

            yield return null;
        }
    }

    public void AnimatioEventSetWorldSppeed()
    {
        //Возврат скорости платформы
        setSpeedForPlatform(normalSpeedPlatform);
    }

    /// <summary>
    /// Сброс всех флагов по окончанию анимаций
    /// </summary>
    private void AnimationEventCancelAllFlags()
    {
        //Возврат скорости платформы
        setSpeedForPlatform(normalSpeedPlatform);
        IsInMovement = false;
        IsRolling = false;
        isClimbing = false;
        timeAnimationClip = 0;
        currentDistance = 0;
        nameWorkedTrigger = "";
        moveVector = Vector3.zero;
    }

    //Добавление к вектору движения персонажа текущую силу прыжка  по событию из анимации
    private void AnimationEventJump() => moveVector.y = JumpForce;

    /// <summary>
    /// Метод для установки центра и высоты коллайдера для анимации кувырка. Вызывается по событию в анимации кувырка
    /// </summary>
    private void AnimationEventRoll()
    {
        //Изменение высоты коллайдера для кувырка
        cc.height = ccHeightForRollAndJump;
        //Изменение центра коллайдера для кувырка
        cc.center = ccCenterRoll;
    }
    
    /// <summary>
    /// Метод для установки центра и высоты коллайдера для анимации прыжка. Вызывается по событию в анимации прыжка
    /// </summary>
    private void SetCollaiderForJump()
    {
        //Изменение высоты коллайдера для прыжка
        cc.height = ccHeightForRollAndJump;
        //Изменение центра коллайдера для прыжка
        cc.center = ccCenterJump;
    }

    /// <summary>
    /// Метод для сброса центра и высоты коллайдера после анимации кувырка. Вызывается по событию в анимации
    /// </summary>
    private void SetNormalCollaider()
    {
        //Сброс центра и высоты коллайдера в нормальные знначения
        cc.height = ccHeightHorm;
        cc.center = ccCenterNorm;
    }

    /// <summary>
    /// Вызов метода для запуска системы частиц правой ноги персонажа по событию в анимации
    /// </summary>
    private void AnimationEventPlayPSRightLeg()
    {
        gameObject.GetComponentInChildren<PSController>().PlayRightLegPS();
    }

    /// <summary>
    /// Вызов метода для запуска системы частиц левой ноги персонажа по событию в анимации
    /// </summary>
    private void AnimationEventPlayPSLeftLeg()
    {
        gameObject.GetComponentInChildren<PSController>().PlayLeftLegPS();
    }

    /// <summary>
    /// Метод реализации столкновения игрока с препятсвием
    /// </summary> 
    private void OnTriggerEnter(Collider col)
    {
        //Провекра на столкновение с препятствиями
        if (!isClimbing)
        { 
            if (((col.CompareTag("FundamentBlock") || col.CompareTag("HTrap")) && !IsRolling) || col.CompareTag("Balcony"))
            {
                gameObject.GetComponentInChildren<PSController>().PlayTopCrash();
                //вызов метода для сброса всех флагов в исходные
                SetInactive();
                SaveCurrentValue();
            }
        }

        if ((col.CompareTag("Wall") && !IsRolling) || col.CompareTag("Trap"))
        {
            gameObject.GetComponentInChildren<PSController>().PlayBottomCrash();
            //вызов метода для сброса всех флагов в исходные
            SetInactive();
            SaveCurrentValue();
        }
    }

    /// <summary>
    /// Установки настроек при гибели персонажа
    /// </summary>
    private void SetInactive()
    {
        //Остановка скорости движения платформ
        setSpeedForPlatform(stopSpeedPlatform);
        //Остановка работы корутины по генерации платформ
        PlatformContainer.GetComponent<WorldController>().StopCoroutine();
        //Вызов метода, для показа меню окончания игры
        UIManager.GetComponent<UIEvent>().ShowMenuGameOver();
        IsAlive = false;
        animator.enabled = false;
        //Установка коллайдеров ragdoll isKinematik = false
        GetComponent<RagdollController>().OffTriggerCollaiders();
        SetNormalCollaider();
        cc.enabled = false;
    }
  
    /// <summary>
    /// Запись текущего показателя набранных очков
    /// </summary>
    private void SaveCurrentValue()
    {
        UIEvent.SaveCurrentRecord();
    }
}
