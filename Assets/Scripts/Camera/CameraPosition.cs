using UnityEngine;

public class CameraPosition : MonoBehaviour
{
    public Transform Camera;               //��������� ������ ������
    public Transform MenuCameraPosition;          //������� ������ � ������� ���� � ���� ��������
    public Transform GameCameraPosition;          //������� ������ �� ����� ����

    public float SpeedMovement;         //�������� ����������� ������
    public float SpeedRotation;         //�������� �������� ������

    private bool isCameraGamePosition = false;  //���� ���������� ����������� � �������� ������

    void Awake()
    {
        //��������� ��������� ������� ������ � ������� ����
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
            //����������� ������
            Camera.position = Vector3.MoveTowards(Camera.position, GameCameraPosition.position, SpeedMovement  * Time.deltaTime);
            //���������� ���� �������� ������
             Quaternion cameraAngle = Quaternion.Euler(new Vector3(
                 GameCameraPosition.rotation.x,
                 GameCameraPosition.rotation.y,
                 GameCameraPosition.rotation.z));
            //������� ������
            Camera.rotation = Quaternion.Lerp(Camera.rotation, cameraAngle, SpeedRotation * Time.deltaTime);
        }
        //�������� �� ����������, ���� ���������� ������ ������, ���  0.2f, �� ��������� ����������� ������
        if (Vector3.Distance(Camera.position, GameCameraPosition.position) < 0.2f)
            isCameraGamePosition = false;
    }

    //��������� ������ � ������� MenuCameraPosition
    public void SetCameraPositionMenu()
    {
        Camera.position = MenuCameraPosition.position;
        Camera.rotation = MenuCameraPosition.rotation;
    }

    //��������� ����� ����������� ������ ��� ������� ����
    public void SetCameraPositionGame()
    {
        isCameraGamePosition = true;
    }
}
