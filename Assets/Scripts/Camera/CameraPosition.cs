using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Transform Camera;               //Трансформ камеры камера
    public Transform MenuCameraPosition;          //Позиция камеры в главном меню и меню настроек
    public Transform GameCameraPosition;          //Позиция камеры во время игры

    public float SpeedMovement;         //Скорость перемещения камеры
    public float SpeedRotation;         //Скорость поворота камеры

    private bool isCameraGamePosition = false;  //Флаг разрешения перемещения и поворота камеры

    void Awake()
    {
        //Начальная устанвока позиции камеры в главном меню
        SetCameraPositionMenu();
    }

    void Start()
    {
        //Camera.transform.position = MenuCameraPosition.transform.position;
        SetCameraPositionMenu();
    }

    void LateUpdate()
    {
        if (isCameraGamePosition)
        {
            //Перемещение камеры
            Camera.position = Vector3.MoveTowards(Camera.position, GameCameraPosition.position, SpeedMovement  * Time.deltaTime);
            //вычисление угла поворота камеры
             Quaternion cameraAngle = Quaternion.Euler(new Vector3(
                 GameCameraPosition.rotation.x,
                 GameCameraPosition.rotation.y,
                 GameCameraPosition.rotation.z));
            //Поворот камеры
            Camera.rotation = Quaternion.Lerp(Camera.rotation, cameraAngle, SpeedRotation * Time.deltaTime);
        }
        //Проверка на расстояние, если расстояние камеры меньше, чем  0.2f, то остановка перемещения камеры
        if (Vector3.Distance(Camera.position, GameCameraPosition.position) < 0.2f)
            isCameraGamePosition = false;
    }

    //Установка камеры в позицию MenuCameraPosition
    public void SetCameraPositionMenu()
    {
        Camera.position = MenuCameraPosition.position;
        Camera.rotation = MenuCameraPosition.rotation;
    }

    //Активация флага перемещения камеры для позиции игры
    public void SetCameraPositionGame()
    {
        isCameraGamePosition = true;
    }
}
