using UnityEngine;

public class GeneratorUI : MonoBehaviour
{
    public GameObject generatorInfoObject; 
    public Generator generator;            
    public Camera mainCamera;              
    public Transform player;               
    public float activationDistance = 7f;  
    public float textSize = 10f;           

    private TextMesh textMesh;             
    private TMPro.TextMeshPro textMeshPro; 

    void Start()
    {
        if (generatorInfoObject == null || generator == null || mainCamera == null || player == null)
        {
            Debug.LogError("Не все компоненты привязаны в Inspector!");
        }

         textMesh = generatorInfoObject.GetComponent<TextMesh>();
        textMeshPro = generatorInfoObject.GetComponent<TMPro.TextMeshPro>();

         if (textMesh == null && textMeshPro == null)
        {
            Debug.LogWarning("Привязанный объект не содержит TextMesh или TextMeshPro!");
        }

        if (textMesh != null)
        {
            textMesh.fontSize = (int)textSize; 
        }
        if (textMeshPro != null)
        {
            textMeshPro.fontSize = textSize; 

        generatorInfoObject.SetActive(false);
    }

    void Update()
    {

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);


        if (distanceToPlayer <= activationDistance)
        {
            generatorInfoObject.SetActive(true);
            UpdateGeneratorInfo();
        }
        else
        {
            generatorInfoObject.SetActive(false);
        }
    }

    void UpdateGeneratorInfo()
    {
        string status = generator.currentFuel > 0 ? "Включен" : "Выключен";
        string infoText =
            "Циклы: " + generator.currentCycle + "/" + generator.maxCycles + "\n" +
            "Топливо: " + generator.currentFuel.ToString("F1") + " / " + generator.maxFuel + "\n" +
            "Выработка мощности: " + generator.powerOutput + " W\n" +
            "Потребление топлива: " + generator.currentConsumption.ToString("F1") + " /с\n" +
            "Статус: " + status;

          if (textMesh != null)
        {
            textMesh.text = infoText;
        }

        if (textMeshPro != null)
        {
            textMeshPro.text = infoText;
        }
    }

    bool IsCursorOverGenerator()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            return hit.transform == transform; 
        }
        return false;
    }
}