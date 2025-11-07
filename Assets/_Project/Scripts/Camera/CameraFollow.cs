using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _fastMoveSpeed = 20f;
    [SerializeField] private float _movementSmoothing = 5f;
    
    [Header("Zoom Settings")]
    [SerializeField] private float _zoomSpeed = 5f;
    [SerializeField] private float _minZoom = 5f;
    [SerializeField] private float _maxZoom = 50f;

    private Vector3 _targetPosition;
    private float _targetZoom ;
    
    private void Awake()
    {
        _targetPosition = transform.position;
        _targetZoom = (_maxZoom - _minZoom)/2f;
    }
    
    private void LateUpdate()
    {
        HandleMovement();
        HandleZoom();
        ApplyMovement();
    }
    
    private void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? _fastMoveSpeed : _moveSpeed;
        
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
        
        _targetPosition += direction * currentSpeed * Time.deltaTime;
    }
    
    private void HandleZoom()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        
        if (scrollDelta != 0f)
        {
            _targetZoom -= scrollDelta * _zoomSpeed;
            _targetZoom = Mathf.Clamp(_targetZoom, _minZoom, _maxZoom);
        }
    }

    private void ApplyMovement()
    {
        Vector3 targetPosition = new Vector3(_targetPosition.x, _targetZoom, _targetPosition.z);
    
        transform.position = Vector3.Lerp(
            transform.position,
            targetPosition,
            _movementSmoothing * Time.deltaTime
        );
        
    }
}
