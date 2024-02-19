using UnityEngine;

// TODO: Add input handling to InputHandler.cs

/// <summary>
/// Handles the logic for the Pan and Zoom feature.
/// Pans the camera based on arrow key input.
/// Zooms the camera based on the UI slider.
/// </summary>
public class PanAndZoom : MonoBehaviour
{
    [SerializeField] private float zoomValue = 30.0f;
    [SerializeField] private float panSpeed = 2.0f;
    
    //TODO: Add camera link

    /// <summary>
    /// Unity method, called once per frame.
    /// </summary>
    private void Update()
    {
        // Pan the camera based on arrow key input
        PanCamera();
    }

    /// <summary>
    /// Pans the camera based on arrow key input.
    /// </summary>
    private void PanCamera()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += panSpeed * Time.deltaTime * Vector3.right;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += panSpeed * Time.deltaTime * Vector3.left;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += panSpeed * Time.deltaTime * Vector3.forward;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += panSpeed * Time.deltaTime * Vector3.back;
        }

    }

    /// <summary>
    /// Unity method, called when changes are made to attached UI elements.
    /// In this case, called when slider value is changed.
    /// </summary>
    private void OnGUI()
    {
        zoomValue = GUI.VerticalSlider(new Rect(25, 25, 10, (Screen.height - 50)), zoomValue, 50f, 10f);
        UpdateCameraZoom();
    }

    /// <summary>
    /// Updates the camera's zoom based on the slider's value.
    /// </summary>
    private void UpdateCameraZoom()
    {
        Camera.main.fieldOfView = zoomValue;
    }
}