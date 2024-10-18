using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 10f;
    [SerializeField] private int edgeScrollSize = 20;
    [SerializeField] private bool useEdgeScroll = true;
    [SerializeField] private float panSpeed = 10f;
    [SerializeField] private int zoomStep = 5;
    [SerializeField] private float zoomMin = 5;
    [SerializeField] private float zoomMax = 20;
    [SerializeField] private float zoomSpeed = 30f;
    [SerializeField] private CinemachineCamera vcam;

    private InputAction movement;
    private InputAction zoom;
    private InputAction pan;
    private bool currentlyPanning;
    private Vector2 lastMousePosition;

    private void Start()
    {
        movement = GameManager.instance.InputAction.FindAction("UI/CameraMovement");
        zoom = GameManager.instance.InputAction.FindAction("UI/ZoomCamera");
        pan = GameManager.instance.InputAction.FindAction("UI/PanCamera");
        movement.Enable();
        zoom.Enable();
        pan.Enable();
        zoom.performed += ZoomCamera;
        pan.started += PanCamera;
        pan.canceled += StopPanning;
    }

    private void OnDisable()
    {
        zoom.performed -= ZoomCamera;
        pan.started -= PanCamera;
        pan.canceled -= StopPanning;
        movement.Disable();
        zoom.Disable();
        pan.Disable();
    }

    private void Update()
    {
        MoveCamera();
    }

    private void PanCamera(InputAction.CallbackContext inputValue)
    {
        Debug.Log("panning");
        currentlyPanning = true;
        lastMousePosition = Mouse.current.position.ReadValue();
    }

    private void StopPanning(InputAction.CallbackContext inputValue)
    {
        Debug.Log("no longer panning");
        currentlyPanning = false;
    }
    private void MoveCamera()
    {
        if ( currentlyPanning )
        { // Panning Control
            Vector3 mouseMovementDelta = Mouse.current.position.ReadValue() - lastMousePosition;
            transform.position += mouseMovementDelta * panSpeed * Time.deltaTime;
            lastMousePosition = Mouse.current.position.ReadValue();
        } else
        { // Keyboard control
            Vector3 movementValue = movement.ReadValue<Vector2>();
            if (movementValue != Vector3.zero)
            {
                transform.position += movementValue * movementSpeed * Time.deltaTime;
            }
            else if (useEdgeScroll)
            { // Edge Scroll
                Vector3 currentMousePos = Mouse.current.position.ReadValue();

                if (currentMousePos.x < edgeScrollSize)
                {
                    movementValue.x -= 1f;
                }
                else if (currentMousePos.y < edgeScrollSize)
                {
                    movementValue.y -= 1f;
                }
                else if (currentMousePos.x > Screen.width - edgeScrollSize)
                {
                    movementValue.x += 1f;
                }
                else if (currentMousePos.y > Screen.height - edgeScrollSize)
                {
                    movementValue.y += 1f;
                }
                transform.position += movementValue * movementSpeed * Time.deltaTime;
            }
        }
    }

    private void ZoomCamera(InputAction.CallbackContext inputValue)
    {
        float value = -inputValue.ReadValue<Vector2>().y / 100f;
        float targetFOV = vcam.Lens.OrthographicSize;

        if (Mathf.Abs(value) > 0.1f)
        {
            //transform.position = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            targetFOV = vcam.Lens.OrthographicSize + value * zoomStep;
            targetFOV = Mathf.Clamp(targetFOV, zoomMin, zoomMax);
            vcam.Lens.OrthographicSize = Mathf.Lerp(vcam.Lens.OrthographicSize, targetFOV, Time.deltaTime * zoomSpeed);
        }
    }
}
