using UnityEngine;
using AscensionInput;

/// <summary>
/// Wrapper class for AscensionInput DLL. 
/// Exposes information that is relevant to the simulator while hiding everything else. 
/// Makes minor improvements to the AcsensionInput code by renaming functions and variables while adding documentation.
/// </summary>
public static class TrackStar
{
    private static ConnectionStatus TrakStarStatus => TrakStarDevice.DeviceConnected;

    public static bool IsCalibrated = false;
    
    public static bool IsConnected => TrakStarStatus == ConnectionStatus.Ready;

    public static bool IsConnecting => TrakStarStatus == ConnectionStatus.Connecting;

    public static bool IsDisconnected => TrakStarStatus == ConnectionStatus.Disconnected;

    /// <summary>
    /// Tells the TrakStar device to update its location data.
    /// Call this function before using GetAveragePosition(), GetAverageRotation(), and GetPercentClosed().
    /// </summary>
    public static void UpdateLocation()
    {
        TrakStarDevice.UpdateLocationTrakStar();
    }

    /// <summary>
    /// Tells the TrakStar device to begin connecting to the computer.
    /// Sets IsConnecting to true and IsDisconnected to false.
    /// When finished connecting, IsConnecting will be set to false and IsConnected will be set to true.
    /// </summary>
    public static void Connect()
    {
        TrakStarDevice.Connect();
    }

    /// <summary>
    /// Tells the TrakStar device to disconnect from the computer.
    /// Use this function when the TrakStar device is no longer needed or
    /// when exiting the application to free all resources.
    /// </summary>
    public static void Disconnect()
    {
        TrakStarDevice.Disconnect();
    }

    /// <summary>
    /// Calibrates the TrakStar sensors. Call this function before using the TrakStar device.
    /// </summary>
    public static void CalibrateSensors()
    {
        TrakStarDevice.CalibrateSensors();
    }

    public static void CalibrateWithForcepsOpen()
    {
        TrakStarDevice.CalibrateOpenDistance();
    }

    public static void CalibrateWithForcepsClosed()
    {
        TrakStarDevice.CalibrateClosedDistance();
    }

    /// <summary>
    /// Gets the average position of one of the forceps.
    /// </summary>
    /// <param name="forcepIndex">The index of the forcep to get the position of. Usually 0 for left and 1 for right.</param>
    /// <returns>The average position of the forcep in the world.</returns>
    public static Vector3 GetAveragePosition(int forcepIndex)
    {
        return TrakStarDevice.GetAvgPos(forcepIndex);
    }
    
    /// <summary>
    /// Gets the average rotation of one of the forceps.
    /// </summary>
    /// <param name="forcepIndex">The index of the forcep to get the position of. Usually 0 for left and 1 for right.</param>
    /// <returns>The average rotation of the forcep in the world.</returns>
    public static Quaternion GetAverageRotation(int forcepIndex)
    {
        return TrakStarDevice.GetAvgRot(forcepIndex);
    }

    /// <summary>
    /// Gets the percent closed of one of the forceps. i.e. how close the forceps blades are to each other.
    /// </summary>
    /// <param name="forcepIndex">The index of the forcep to get the position of. Usually 0 for left and 1 for right.</param>
    /// <returns>The percent closed of the forcep.</returns>
    public static float GetPercentClosed(int forcepIndex)
    {
        return TrakStarDevice.GetPercentClosed(forcepIndex);
    }

    public static string GetConnectionErrorMessage()
    {
        return TrakStarDevice.ConnectionErrorMessage;
    }

    public static float GetLoadingProgress()
    {
        //return TrakStarDevice.LoadingProgess;
        return 0.5f;
    }

    public static void SetLoadingProgress(float value)
    {
        //TrakStarDevice.LoadingProgess = value;
    }

}
