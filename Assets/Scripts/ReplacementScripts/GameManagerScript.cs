using UnityEngine;
using UnityEngine.Serialization;

public class GameManagerScript : MonoBehaviour
{
    
    [Header("Settings")]

    [SerializeField]
    private bool isUsingForceps = true;

    public static bool IsUsingForceps = false;

    [SerializeField] 
    private float gravity = 9.8f;

    [Header("Object Links")]

    [SerializeField]
    private GUIController guiController;

    // Start is called before the first frame update
    private void Start()
    {
        IsUsingForceps = isUsingForceps;
        Physics.gravity = new Vector3(0, -gravity);
        
        if (isUsingForceps && !TrackStar.IsConnected)
        {
            TrackStar.Connect();
        }

        if (guiController == null)
        {
            guiController = FindObjectOfType<GUIController>();
            Debug.Log("guiController link is null on " + gameObject.name);
        }

        if (!isUsingForceps)
        {
            guiController.SetCalibrationImageActive(false);
            guiController.SetConnectionImageActive(false);
            guiController.SetLoadingBarActive(false);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        // if in unity editor
        #if UNITY_EDITOR
        Physics.gravity = new Vector3(0, -gravity);
        IsUsingForceps = isUsingForceps;
        #endif

        if (!isUsingForceps)
        {
            return;
        }

        if (TrackStar.IsCalibrated) {
            //TrackStar.UpdateLocation(); 
            guiController.SetLoadingBarActive(false);
        }

        if (TrackStar.IsConnected && !TrackStar.IsCalibrated)
        {
            guiController.SetCalibrationImageActive(true);
            guiController.SetConnectionImageActive(false);
            TrackStar.SetLoadingProgress(1f);
            guiController.SetLoadingBarValue(TrackStar.GetLoadingProgress());
            guiController.SetLoadingBarActive(false);
        }

        if (TrackStar.IsConnecting)
        {
            guiController.SetStatusText(TrackStar.GetConnectionErrorMessage());
            guiController.SetLoadingBarValue(TrackStar.GetLoadingProgress());
            if (TrackStar.GetLoadingProgress() >= 0.1f && TrackStar.GetLoadingProgress() < 0.99f) { 
                TrackStar.SetLoadingProgress(TrackStar.GetLoadingProgress() + (0.06f * Time.deltaTime));
            }
        }
    }

    private void OnApplicationQuit()
    {
        if (TrackStar.IsConnected)
        {
            TrackStar.Disconnect();
        }
    }

}
