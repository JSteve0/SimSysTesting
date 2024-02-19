using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles GUI interaction in surgery scenes
/// </summary>
public class GUIController : MonoBehaviour
{
    
    [Header("Object Links")]
    
    [SerializeField]
    private MetricsTracker metricsTrackerLink;
    
    [SerializeField]
    private GameObject connectionImageLink;
    
    [SerializeField]
    private GameObject calibrationImageLink;

    [SerializeField]
    private TextMeshProUGUI statusText;

    [SerializeField]
    private Slider loadingBarSlider; 


    private void Start()
    {
        CheckObjectLinks();
    }

    private void CheckObjectLinks()
    {
        if (metricsTrackerLink == null)
        {
            Debug.LogWarning("MetricsTrackerLink is missing direct link on GUI script\nUse direct link to improve performance");
            metricsTrackerLink = FindObjectOfType<MetricsTracker>();
        }
    }

    public void SetConnectionImageActive(bool isActive)
    {
        connectionImageLink.SetActive(isActive);
    }

    public void SetCalibrationImageActive(bool isActive)
    {
        calibrationImageLink.SetActive(isActive);
    }

    public void SetAllImagesActive(bool isActive)
    {
        SetConnectionImageActive(isActive);
        SetCalibrationImageActive(isActive);
    }

    public void SetStatusText(string text)
    {
        statusText.text = text;
    }

    public void SetLoadingBarValue(float value)
    {
        loadingBarSlider.value = value;
    }

    public void SetLoadingBarActive(bool isActive)
    {
        loadingBarSlider.gameObject.SetActive(isActive);
    }

}
