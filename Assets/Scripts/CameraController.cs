using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float _dragSpeed = 8f;
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;
    [SerializeField] private float _minCameraZoom = 10f;
    [SerializeField] private float _maxCameraZoom = 70f;
    [SerializeField] private float _zoomStep = 2f;

    private PlayerInput _playerInput;
    private bool _isDragging;
    private bool _isRotating;
    private Vector2 _lastMousePosition;
    private Quaternion _defaultRotation;
    private float _defaultCameraZoom;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        _defaultRotation = transform.rotation;
        _defaultCameraZoom = _cinemachineVirtualCamera.m_Lens.FieldOfView;
    }

    private void OnEnable()
    {
        _playerInput.Enable();        
        _playerInput.Camera.Drag.started += OnDragStart;
        _playerInput.Camera.Drag.canceled += OnDragCancel;
        _playerInput.Camera.Rotate.started += OnRotateStart;
        _playerInput.Camera.Rotate.canceled += OnRotateCancel;
        _playerInput.Camera.RotateToDefault.performed += OnRotateToDefault;
        _playerInput.Camera.Zoom.performed += OnZoom;
        _playerInput.Camera.ZoomToDefault.performed += OnZoomToDefault;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Camera.Drag.started -= OnDragStart;
        _playerInput.Camera.Drag.canceled -= OnDragCancel;
        _playerInput.Camera.Rotate.started -= OnRotateStart;
        _playerInput.Camera.Rotate.canceled -= OnRotateCancel;
        _playerInput.Camera.RotateToDefault.performed -= OnRotateToDefault;
        _playerInput.Camera.Zoom.performed -= OnZoom;
        _playerInput.Camera.ZoomToDefault.performed -= OnZoomToDefault;
    }

    private void Update()
    {
        if (_isDragging || _isRotating)
        {
            Vector2 mouseMovementDelta = Mouse.current.position.ReadValue() - _lastMousePosition;
            float minValue = 0.1f;

            if (mouseMovementDelta.sqrMagnitude < minValue)
                return;

            if (_isDragging)
                Drag(mouseMovementDelta);
            else if (_isRotating)
                Rotate(mouseMovementDelta);

            UpdateLastMousePosition();
        }
    }

    private void Drag(Vector2 mouseMovementDelta)
    {
        float scaledDragSpeed = _dragSpeed * Time.deltaTime;
        Vector3 offset = new Vector3(-mouseMovementDelta.x, 0f, -mouseMovementDelta.y) * scaledDragSpeed;

        transform.Translate(offset);
    }

    private void Rotate(Vector2 mouseMovementDelta)
    {
        float scaledRotationSpeed = _rotationSpeed * Time.deltaTime;
        Vector3 offset = new Vector3(0f, mouseMovementDelta.x, 0f) * scaledRotationSpeed;

        transform.Rotate(offset);
    }

    private void OnDragStart(InputAction.CallbackContext context)
    {
        _isDragging = true;
        UpdateLastMousePosition();
    }

    private void OnDragCancel(InputAction.CallbackContext context)
    {
        _isDragging = false;
    }

    private void OnRotateStart(InputAction.CallbackContext context)
    {
        _isRotating = true;
        UpdateLastMousePosition();
    }

    private void OnRotateCancel(InputAction.CallbackContext context)
    {
        _isRotating = false;
    }

    private void OnRotateToDefault(InputAction.CallbackContext context)
    {        
        transform.rotation = _defaultRotation;        
    }

    private void UpdateLastMousePosition()
    {
        _lastMousePosition = Mouse.current.position.ReadValue();
    }

    private void OnZoom(InputAction.CallbackContext context)
    {
        float zoomValue = _cinemachineVirtualCamera.m_Lens.FieldOfView;
        float mouseScrollDelta = context.ReadValue<Vector2>().y;

        if (mouseScrollDelta > 0f)
            zoomValue = Mathf.Clamp(zoomValue - _zoomStep, _minCameraZoom, _maxCameraZoom);
        else
            zoomValue = Mathf.Clamp(zoomValue + _zoomStep, _minCameraZoom, _maxCameraZoom);

        _cinemachineVirtualCamera.m_Lens.FieldOfView = zoomValue;
    }

    private void OnZoomToDefault(InputAction.CallbackContext context)
    {
        _cinemachineVirtualCamera.m_Lens.FieldOfView = _defaultCameraZoom;
    }
}
