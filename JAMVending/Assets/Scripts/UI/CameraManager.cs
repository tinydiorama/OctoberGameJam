using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    private InputAction movement;
    private InputAction zoom;
    private Transform cameraTransform;
    private CinemachineCamera vcam;

    // panning
    [SerializeField] private float maxSpeed = 20f;
    [SerializeField] private float acceleration = 10f;
    [SerializeField] private float damping = 15f;
    private float speed;

    // zooming
    [SerializeField] private float stepSize = 2f;
    [SerializeField] private float zoomDampening = 7.5f;
    [SerializeField] private float minHeight = 5f;
    [SerializeField] private float maxHeight = 50f;
    [SerializeField] private float zoomSpeed = 2f;

    private Vector3 targetPosition;
    private float zoomHeight;
    private Vector3 horizontalVelocity;
    private Vector3 lastPosition;

    private Vector3 startDrag;

    private void Awake()
    {
        cameraTransform = transform;
    }

    private void OnEnable()
    {
        lastPosition = transform.position;

        //movement.performed += placeDecoration;
    }

    private void Start()
    {
        movement = GameManager.instance.InputAction.FindAction("UI/CameraMovement");
        zoom = GameManager.instance.InputAction.FindAction("UI/ZoomCamera");
        vcam = GetComponent<CinemachineCamera>();
        zoomHeight = vcam.Lens.OrthographicSize;
        movement.Enable();
        zoom.Enable();

        zoom.performed += ZoomCamera;
    }

    private void OnDisable()
    {
        zoom.performed -= ZoomCamera;
        movement.Disable();
        zoom.Disable();
    }

    private void Update()
    {
        GetKeyboardMovement();
        UpdateVelocity();
        UpdateBasePosition();
        vcam.Lens.OrthographicSize = Mathf.Lerp(vcam.Lens.OrthographicSize, zoomHeight, Time.deltaTime * zoomSpeed);
    }

    private void UpdateVelocity()
    {
        horizontalVelocity = (transform.position - lastPosition) / Time.deltaTime;
        horizontalVelocity.z = 0;
        lastPosition = transform.position;
    }

    private void GetKeyboardMovement()
    {
        Vector3 inputValue = movement.ReadValue<Vector2>();
        inputValue.z = 0;
        inputValue = inputValue.normalized;
        if ( inputValue.sqrMagnitude > 0.1f )
        {
            targetPosition += inputValue;
            vcam.Follow = null;
        }
    }

    private void UpdateBasePosition()
    {
        if ( targetPosition.sqrMagnitude >= 0.1f )
        {
            speed = Mathf.Lerp( speed, maxSpeed, Time.deltaTime * acceleration );
            transform.position += speed * Time.deltaTime * targetPosition;
        } else
        {
            horizontalVelocity = Vector3.Lerp(horizontalVelocity, Vector3.zero, Time.deltaTime * damping);
            transform.position += horizontalVelocity * Time.deltaTime;
        }
        targetPosition = Vector3.zero;
    }

    private void ZoomCamera(InputAction.CallbackContext inputValue)
    {
        float value = -inputValue.ReadValue<Vector2>().y / 100f;

        if (Mathf.Abs(value) > 0.1f )
        {
            zoomHeight = vcam.Lens.OrthographicSize + value * stepSize;
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            targetPosition = mousePosition;
            if ( zoomHeight < minHeight)
            {
                zoomHeight = minHeight;
            } else if ( zoomHeight > maxHeight )
            {
                zoomHeight = maxHeight;
            }
        }
    }
}
