using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName; // Название предмета
    public bool isPickable = true; // Можно ли взять предмет

    public void Pickup()
    {
        if (isPickable)
        {
            // Логика "взятия" предмета, например, можно добавить его в инвентарь или просто скрыть
            Debug.Log("Item picked up: " + itemName);
            Destroy(gameObject); // Уничтожаем предмет, так как он взят
        }
    }
}
