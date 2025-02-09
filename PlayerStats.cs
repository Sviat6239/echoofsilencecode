using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [Header("Параметры игрока")]
    public float health = 100f;
    public float stamina = 100f;
    public float hunger = 100f;
    public float paranoia = 0f;

    [Header("Максимальные значения")]
    public float maxHealth = 100f;
    public float maxStamina = 100f;
    public float maxHunger = 100f;
    public float maxParanoia = 100f;

    [Header("Скорости изменения параметров")]
    public float staminaRegenRate = 1.5f;    // Скорость восстановления стамины
    public float staminaDrainIdle = 0.01f; // Потеря стамины в состоянии покоя
    public float staminaDrainWalk = 0.05f; // Потеря стамины при ходьбе
    public float staminaDrainRun = 0.1f;   // Потеря стамины при беге
    public float staminaDrainJump = 0.5f;  // Потеря стамины при прыжке
    public float hungerDecayRate = 0.01f;  // Базовое снижение голода
    public float healthDecayRate = 0.5f;   // Урон от голода

    private bool isRunning = false;
    private bool isJumping = false;
    private bool isMoving = false;

    [Header("Связь с UI")]
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI hungerText;
    public TextMeshProUGUI paranoiaText;

    void Update()
    {
        UpdateStamina();
        UpdateHunger();
        UpdateHealth();
        UpdateParanoia();
        UpdateUI();
    }

    void UpdateStamina()
    {
        if (isJumping)
        {
            stamina -= staminaDrainJump * Time.deltaTime;
        }
        else if (isRunning)
        {
            stamina -= staminaDrainRun * Time.deltaTime;
        }
        else if (isMoving)
        {
            stamina -= staminaDrainWalk * Time.deltaTime;
        }
        else
        {
            float regenModifier = hunger < 20f ? 0.3f : 1f; 
            float staminaRegen = staminaRegenRate * regenModifier * Time.deltaTime;

            if (stamina < maxStamina)
            {
                float actualRegen = Mathf.Min(staminaRegen, maxStamina - stamina);
                stamina += actualRegen;
                hunger -= actualRegen / 2f;
            }
        }

        stamina = Mathf.Clamp(stamina, 0f, maxStamina);
    }

    // 🔹 Обновление голода
    void UpdateHunger()
    {
        hunger -= hungerDecayRate * Time.deltaTime; // Медленное снижение голода

        hunger = Mathf.Clamp(hunger, 0f, maxHunger);
    }

    // 🔹 Обновление здоровья (если голод = 0, уменьшается здоровье)
    void UpdateHealth()
    {
        if (hunger <= 0f)
        {
            health -= healthDecayRate * Time.deltaTime;
        }

        health = Mathf.Clamp(health, 0f, maxHealth);
    }

    // 🔹 Обновление паранойи
    void UpdateParanoia()
    {
        if (IsInDarkPlace())
            paranoia += 1f * Time.deltaTime;
        else
            paranoia -= 0.5f * Time.deltaTime;

        paranoia = Mathf.Clamp(paranoia, 0f, maxParanoia);
    }

    // Проверка на тёмное место
    bool IsInDarkPlace()
    {
        return RenderSettings.ambientIntensity < 0.3f;
    }

    // 🔹 Обновление UI
    void UpdateUI()
    {
        healthText.text = $"{health:F1}";
        staminaText.text = $"{stamina:F1}";
        hungerText.text = $"{hunger:F1}";
        paranoiaText.text = $"{paranoia:F1}";
    }

    // 🔹 Получение урона
    public void TakeDamage(float amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0f, maxHealth);
    }

    // 🔹 Движение игрока
    public void SetMovement(bool moving)
    {
        isMoving = moving;
    }

    // 🔹 Бег игрока
    public void SetRunning(bool running)
    {
        isRunning = running;
    }

    // 🔹 Прыжок игрока
    public void SetJumping(bool jumping)
    {
        isJumping = jumping;
    }

    // 🔹 Смерть игрока
    void Die()
    {
        Debug.Log("Игрок умер!");
    }
}
