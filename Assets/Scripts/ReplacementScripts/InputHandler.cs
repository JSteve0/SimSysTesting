using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Handles all input from keyboard, controller, mouse, etc. Forwards input to other scripts.
/// </summary>
public class InputHandler : MonoBehaviour
{

    [Header("Object Links")]
    
    [SerializeField] private GUIController guiLink;
    [SerializeField] private MetricsTracker metricsTrackerLink;

    [SerializeField] private ForcepScript leftForcep;
    [SerializeField] private ForcepScript rightForcep;

    [Header("Settings")]
    
    [SerializeField] private float forcepsSpeed = 10f;
    [SerializeField] private bool leftForcepActive = true;
    [SerializeField] private bool rightForcepActive = true;

    private Vector3 _movementVector = Vector3.zero;
    private Vector3 _rotationVector3 = Vector3.zero;
    
    private float MovementDelta => forcepsSpeed * Time.deltaTime;
    
    public bool LeftForcepsActive
    {
        get => leftForcepActive;
        set => leftForcepActive = value;
    }
    
    public bool RightForcepsActive
    {
        get => rightForcepActive;
        set => rightForcepActive = value;
    }

    private void Start()
    {
        CheckObjectLinks();
    }

    private void Update()
    {
        ProcessKeys();
    }

    /// <summary>
    /// Processes keyboard input.
    /// </summary>
    private void ProcessKeys()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (TrackStar.IsConnected)
            {
                TrackStar.CalibrateSensors();
                metricsTrackerLink.SetIsTrackingMetrics(true);
                guiLink.SetAllImagesActive(false);
                TrackStar.IsCalibrated = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            if (TrackStar.IsConnected)
            {
                TrackStar.CalibrateWithForcepsOpen();
                metricsTrackerLink.SetIsTrackingMetrics(true);
                guiLink.SetAllImagesActive(false);
                TrackStar.IsCalibrated = true;
            }
        }
        
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            if (TrackStar.IsConnected)
            {
                TrackStar.CalibrateWithForcepsClosed();
                metricsTrackerLink.SetIsTrackingMetrics(true);
                guiLink.SetAllImagesActive(false);
                TrackStar.IsCalibrated = true;
            }
        }
        
        if (leftForcepActive)
        {
            leftForcep.transform.position += _movementVector;
            leftForcep.transform.Rotate(_rotationVector3 * (MovementDelta * 1000f));
        }
        if (rightForcepActive)
        {
            rightForcep.transform.position += _movementVector;
            rightForcep.transform.Rotate(_rotationVector3 * (MovementDelta * 1000f));
        }
    }

    /// <summary>
    /// Checks that links to other scripts and objects are not null.
    /// Sets them if they are null, but gives user a console warning.
    /// </summary>
    private void CheckObjectLinks()
    {   
        if (guiLink == null)
        {
            Debug.LogWarning("guiLink is missing direct link on InputHandler script\nUse direct link to improve performance");
            guiLink = FindObjectOfType<GUIController>();
        }
    }

    #region Controller Input Handlers

    public void LeftStickHandler(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        //var movementVector = new Vector3(horizontal * MovementDelta, 0, forward * MovementDelta);
        _movementVector.x = input.x * MovementDelta;
        _movementVector.z = input.y * MovementDelta;
    }
    
    public void RightStickHandler(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();

        //var movementVector = new Vector3(horizontal * MovementDelta, 0, forward * MovementDelta);
        _rotationVector3.z = -input.x * MovementDelta;
        _rotationVector3.y = -input.y * MovementDelta;
    }

    public void MoveUp(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _movementVector.y = MovementDelta;
        } else if (context.canceled)
        {
            _movementVector.y = 0;
        }
    }
    
    public void MoveDown(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _movementVector.y = -MovementDelta;
        } else if (context.canceled)
        {
            _movementVector.y = 0;
        }
    }
    
    public void Rotate(InputAction.CallbackContext context)
    {
        float input = -context.ReadValue<float>();
        _rotationVector3.x = MovementDelta * input;
    }
    
    public void CloseLeftForcep(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();
        leftForcep.SetPercentClosed(input * 100f);
    }
    
    public void CloseRightForcep(InputAction.CallbackContext context)
    {
        float input = context.ReadValue<float>();
        rightForcep.SetPercentClosed(input * 100f);
    }

    public void ToggleActiveForcep(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (leftForcepActive)
            {
                leftForcepActive = false;
                rightForcepActive = true;
            }
            else
            {
                leftForcepActive = true;
                rightForcepActive = false;
            }
        }
    }
    
    #endregion
}
