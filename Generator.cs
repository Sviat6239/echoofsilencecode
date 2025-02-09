using UnityEngine;

public class Generator : MonoBehaviour
{
    [Header("Настройки генератора")]
    public int maxCycles = 10;                // Максимальное количество циклов
    public float cycleDuration = 5f;          // Длительность одного цикла (в секундах)
    public float maxFuel = 100f;              // Максимальный объем топлива
    public float currentFuel;                 // Текущий уровень топлива
    public float fuelConsumptionPerCycle = 5f; // Потребление топлива за цикл
    public float powerOutput = 10f;           // Вырабатываемая мощность
    public float currentConsumption;          // Текущее потребление

    public int currentCycle = 0;              // Текущий цикл, доступен из других скриптов

    private float timeSinceLastCycle = 0f;    // Время с последнего цикла

    void Start()
    {
        currentFuel = maxFuel;  // Изначально полный бак топлива
    }

    void Update()
    {
        // Проверка на наличие топлива
        if (currentFuel > 0 && currentCycle < maxCycles)
        {
            timeSinceLastCycle += Time.deltaTime;

            // Проверка завершения цикла
            if (timeSinceLastCycle >= cycleDuration)
            {
                PerformCycle();
            }
        }
        else
        {
            // Остановка генератора, если топлива нет или достигнут максимальный цикл
            StopGenerator();
        }
    }

    // Метод для выполнения цикла генерации
    void PerformCycle()
    {
        // Вычисление текущего потребления
        currentConsumption = fuelConsumptionPerCycle / cycleDuration;

        // Потребление топлива за цикл
        currentFuel -= fuelConsumptionPerCycle;

        // Увеличиваем количество циклов
        currentCycle++;

        timeSinceLastCycle = 0f;

        // Вывод информации в консоль
        Debug.Log($"Цикл {currentCycle}/{maxCycles}: Потребление топлива: {fuelConsumptionPerCycle} / Мощность: {powerOutput}");
    }

    // Остановка генератора, если топливо закончилось
    void StopGenerator()
    {
        currentConsumption = 0f;
        powerOutput = 0f;
        Debug.Log("Генератор остановлен, топлива нет.");
    }
}
