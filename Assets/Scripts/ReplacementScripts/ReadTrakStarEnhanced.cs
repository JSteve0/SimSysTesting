using UnityEngine;
using AscensionInput;

/// <summary>
/// Handles communication with TrakStarDevice
/// </summary>
public class ReadTrakStarEnhanced : MonoBehaviour
{
    private ConnectionStatus TrakStarStatus => TrakStarDevice.DeviceConnected;
    
    // ReSharper disable once MemberCanBePrivate.Global
    public bool TrakStarIsReady => TrakStarStatus == ConnectionStatus.Ready;

    public bool TrakStarIsConnecting => TrakStarStatus == ConnectionStatus.Connecting;

    public bool TrakStarIsDisconnected => TrakStarStatus == ConnectionStatus.Disconnected;

    private void Start()
    {
        ConnectTrakStarDevice();
    }

    private void Update()
    {
        if (TrakStarIsReady)
        {
            TrakStarDevice.UpdateLocationTrakStar();
        }
    }

    /// <summary>
    /// Disconnects the TrakStar when the application ends.
    /// </summary>
    private void OnApplicationQuit()
    {
        if (TrakStarIsReady) 
        {
            TrakStarDevice.Disconnect();
        }
    }
    
    /// <summary>
    /// Initiates the connection process with the TrakStar device if it is not already connected.
    /// </summary>
    private void ConnectTrakStarDevice()
    {
        if (!TrakStarIsReady)
        {
            TrakStarDevice.Connect();
        }
    }

    /// <summary>
    /// Calibrates the TrakStar sensors if it is ready to.
    /// </summary>
    public void CalibrateSensors()
    {
        if (TrakStarIsReady)
        {
            TrakStarDevice.CalibrateSensors();
        }
    }

    public void CalibrateOpenDistance()
    {
        if (TrakStarIsReady)
        {
            TrakStarDevice.CalibrateOpenDistance();
        }
    }

    public void CalibrateClosedDistance()
    {
        if (TrakStarIsReady)
        {
            TrakStarDevice.CalibrateClosedDistance();
        }
    }

    public Vector3 GetAveragePosition(int forcepIndex)
    {
        return TrakStarDevice.GetAvgPos(forcepIndex);
    }
    
    public Quaternion GetAverageRotation(int forcepIndex)
    {
        return TrakStarDevice.GetAvgRot(forcepIndex);
    }

    public float GetPercentClosed(int forcepIndex)
    {
        return TrakStarDevice.GetPercentClosed(forcepIndex);
    }
    
    

}
