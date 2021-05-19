using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    public Transform endPoint;
    public Transform[] spawnArea;

    void Start()
    {
        //Подписка на событие создания платформы
        WorldController.instance.OnPlatformMovement += TryDeleteAndAddPlatform;
    }

    /// <summary>
    /// Создание платформы 
    /// </summary>
    private void TryDeleteAndAddPlatform()
    {
        WorldController.instance.worldBuilder.CreatePlatform();
    }

    /// <summary>
    /// отписка от события создания платформы
    /// </summary>
    private void OnDestroy()
    {
        if (WorldController.instance != null)
        {
            WorldController.instance.OnPlatformMovement -= TryDeleteAndAddPlatform;
        }
    }
}
