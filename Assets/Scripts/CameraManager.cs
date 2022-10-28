using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Transform _orientation;
    
    private float _xRotation;
    private float _yRotation;

    [Header("Sensitivity")]
    [Tooltip("This box will be taken into account only if the 'useUnifiedSensitivity' is checked.")]
    public float sensitivity = 160.0f;
    public float verticalSensitivity = 160.0f; // Y axis.
    public float horizontalSensitivity = 160.0f; // X axis.

    [Header("Preferences")]
    [Tooltip("This checkbox will allow you to choose if you want to use the Sensitivity for both Horizontal and Vertical movement.")]
    public bool useUnifiedSensitivity = true;

    private void Start() {
        _orientation = GameObject.Find("Orientation").transform.GetComponent<Transform>();
        SetCursor(true);
    }

    private void Update() {
        Look();
        UnifySens();
    }

    public void UnifySens() {
        if (useUnifiedSensitivity) {
            verticalSensitivity = sensitivity;
            horizontalSensitivity = sensitivity;
        }
    }

    public void Look() {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.fixedDeltaTime * horizontalSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.fixedDeltaTime * verticalSensitivity;
        
        _xRotation -= mouseY;
        _yRotation += mouseX;
        _xRotation = Mathf.Clamp(_xRotation, -90.0f, 90.0f);

        transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
        _orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
    }

    public void SetCursor(bool isHidden) {
        if (isHidden) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
