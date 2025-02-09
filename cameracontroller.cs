using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    public Transform playerBody;
    public float mouseSensitivity = 100f;
    public float minVerticalAngle = -75f;
    public float maxVerticalAngle = 75f;
    public float tiltSpeed = 5f;
    public float tiltAmount = 0.5f;

    private float verticalRotation = 0f;
    private Vector3 initialPosition;

    private Light flashlightLight;
    private GameObject flashlight;
    private bool isFlashlightOn = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        initialPosition = transform.localPosition;
        SetupFlashlight();
    }

    void Update()
    {
        HandleMouseLook();
        HandleCameraTilt();
        ToggleFlashlight();
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        playerBody.Rotate(Vector3.up * mouseX);

        verticalRotation -= mouseY;
        verticalRotation = Mathf.Clamp(verticalRotation, minVerticalAngle, maxVerticalAngle);
        transform.localRotation = Quaternion.Euler(verticalRotation, 0f, 0f);
    }

    private void HandleCameraTilt()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + Vector3.left * tiltAmount, Time.deltaTime * tiltSpeed);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + Vector3.right * tiltAmount, Time.deltaTime * tiltSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition, Time.deltaTime * tiltSpeed);
        }
    }

    private void SetupFlashlight()
    {
        flashlight = new GameObject("Flashlight");
        flashlightLight = flashlight.AddComponent<Light>();

        flashlightLight.type = LightType.Spot;
        flashlightLight.color = new Color(1f, 0.95f, 0.8f);
        flashlightLight.intensity = 5f;
        flashlightLight.spotAngle = 30f;
        flashlightLight.range = 60f;
        flashlightLight.shadowStrength = 0.8f;
        flashlightLight.enabled = false;

        flashlight.transform.SetParent(transform);
        flashlight.transform.localPosition = Vector3.zero;
    }

    private void ToggleFlashlight()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            isFlashlightOn = !isFlashlightOn;
            flashlightLight.enabled = isFlashlightOn;
        }
    }
}
