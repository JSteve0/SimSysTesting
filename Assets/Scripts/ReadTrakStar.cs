using System;
using UnityEngine;
using System.Collections.Generic;
using AscensionInput;
using TMPro;

/// <summary>
/// Class that handles the reading of the TrakStar device and the updating of the forceps on screen.
/// Is now obsolete, use ReadTrakStarEnhanced instead.
/// </summary>
[Obsolete]
public class ReadTrakStar : MonoBehaviour {

    [Header("Component Links")]
    public GameObject[] forcepGameObjects;
    internal Forcep[] forceps;

    //SAMS VARIABLES
    public MetricsTracker metricsScriptLink;
    // Distance that the forcep needs to move in a frame for it to be added to total distance
    public float forcepsMetricsDistanceThreshold;
    // Used to check if distance traveled is greater than the threshold
    private float _storedDistanceTraveled = 0.0f;
    public GameObject connectionImageLink;
    public GameObject calibrationImageLink;

    public GameObject hintTextLink;

    //timer
    private float _hintTextTimer = 0.0f;
    
    [Header("Timer")]
    [Min(5), Tooltip("Minimum number of seconds needed to pass before a hint appears which tells the user to make sure that the TrakStar device is turned on.")]
    public float maxTimeBeforeHint = 30.0f;

    [Header("Forcep Settings")]
    public Vector3 startPos = new Vector3(0f, 0f, 0f);

    // Use this for initialization
    private void Start () 
    {
        EnsureSingleInstance();
        ConnectTrakStarDevice();
        CreateForceps();
    }
    
    // Update is called once per frame
    private void Update () 
    {
        UpdateForceps();
        ProcessKeys();
        UpdateGUI();
    }

    private void EnsureSingleInstance()
    {
        if (FindObjectsOfType<ReadTrakStar>().Length > 1)
        {
            ReadTrakStar[] stars = FindObjectsOfType<ReadTrakStar>();
            foreach (ReadTrakStar star in stars)
            {
                if (star.gameObject != gameObject)
                {
                    Destroy(star.gameObject);
                }
            }
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    private void ConnectTrakStarDevice()
    {
        if (TrakStarDevice.DeviceConnected != ConnectionStatus.Ready)
        {
            TrakStarDevice.Connect();
        }
    }

    private void CreateForceps()
    {
        forceps = new Forcep[forcepGameObjects.Length]; //size 2
        for (int i = 0; i < forcepGameObjects.Length; i++)
        {
            forceps[i] = new Forcep
            {
                forcepMasterGameObject = forcepGameObjects[i],
                objectNames =
                {
                    [0] = new List<string>(),
                    [1] = new List<string>()
                },
                graspedObjects = new List<GameObject>()
            };
        }

        foreach (var forcep in forceps)
        {
            Transform[] tempGO = forcep.forcepMasterGameObject.GetComponentsInChildren<Transform>();//load in different forcep images

            //For each forceps model made, emulate the structure of these if statments with the names of the new model.
            for (int i = 0; i < tempGO.Length; i++)
            {
                if (tempGO[i].name == "bentForceps")
                {
                    forcep.devices[0] = tempGO[i].gameObject;
                }
                else if (tempGO[i].name == "straightForceps")
                {
                    forcep.devices[1] = tempGO[i].gameObject;
                }
            }

            forcep.colliders = new List<GameObject>();

            //the two children of the full model gameobject. left and right represent the two sides of the forceps. 
            for (int i = 0; i < forcep.left.Length; i++)
            {
                forcep.left[i] = forcep.devices[i].transform.Find("left");
                forcep.right[i] = forcep.devices[i].transform.Find("right");

                ForcepsColliderScript leftFCS = forcep.left[i].GetComponent<ForcepsColliderScript>();
                ForcepsColliderScript rightFCS = forcep.right[i].GetComponent<ForcepsColliderScript>();

                for (int j = 0; j < leftFCS.insideTipColliders.Length; j++)
                {
                    forcep.colliders.Add(leftFCS.insideTipColliders[j]);
                    forcep.colliders.Add(rightFCS.insideTipColliders[j]);
                }
            }

            //setting all of the models under the masterforceps gameobject to disabled except for one which is specified at the initial value of deviceIndex
            for (int i = 0; i < forcep.devices.Length; i++)
            {
                if (i != forcep.deviceIndex)
                {
                    forcep.devices[i].SetActive(false);
                }
            }
        }
    }

    void UpdateForceps() 
    {
        if (TrakStarDevice.DeviceConnected == ConnectionStatus.Ready) 
        {
            TrakStarDevice.UpdateLocationTrakStar();

            for (int forcepOn = 0; forcepOn < forceps.Length; forcepOn++)
            {
                Forcep.UpdateForcepsHelper(TrakStarDevice.GetAvgPos(forcepOn), TrakStarDevice.GetAvgRot(forcepOn), TrakStarDevice.GetPercentClosed(forcepOn), forceps[forcepOn], forcepOn, this); //sends UpdateForceps method the necessary information so the forceps on screen can be disaplyed accurately
                //UpdateForceps(TrakStarDevice.GetAvgPos(1), TrakStarDevice.GetAvgRot(1) , TrakStarDevice.GetPercentClosed(1), forcepOn);
            }
        }
    }

    void ProcessKeys()
    {
        //Only check keys if the forceps are connected
        if (TrakStarDevice.DeviceConnected != ConnectionStatus.Ready)
        {
            return;
        }

        if (Input.GetKey (KeyCode.Space)) //Calibrate with forceps flat facing the trakStar cube
        {
            TrakStarDevice.CalibrateSensors();
            metricsScriptLink.SetIsTrackingMetrics(true);
            gameObject.transform.position = startPos;
            //foreach (Forcep f in forceps)
            //{
                //f.devices[f.deviceIndex].gameObject.transform.position = startPos;
            //}

            HideImages();
        }

        if (Input.GetKey (KeyCode.Equals)) //Calibrate with forceps flat facing the trakStar cube
        {
            TrakStarDevice.CalibrateOpenDistance();

            HideImages();
        }

        if (Input.GetKey (KeyCode.Minus)) //Calibrate with forceps flat facing the trakStar cube
        {
            TrakStarDevice.CalibrateClosedDistance();

            HideImages ();
        }

        for (int i = 1; i <= 4; i++)
        {
            // Check if the player is pressing a key with the current number [1, 4]
            if (Input.GetKey(KeyCode.Alpha0 + i))
            {
                // Call the SwitchForcepsModel function with the current number as a parameter
                SwitchForcepsModel(i);
            }
        }
    }

    void HideImages()
    {
        if (calibrationImageLink.activeInHierarchy)
        {
            calibrationImageLink.SetActive(false);
        }
        if (hintTextLink.activeInHierarchy)
        {
            hintTextLink.SetActive(false);
        }
    }

    void UpdateGUI()
    {
        switch (TrakStarDevice.DeviceConnected)
        {
            case ConnectionStatus.Connecting:
            {
                if (!connectionImageLink.activeInHierarchy) 
                {
                    connectionImageLink.SetActive (true);
                }

                if (_hintTextTimer > maxTimeBeforeHint && hintTextLink != null) 
                {
                    // Show hint
                    hintTextLink.SetActive (true);
                } 
                else if (hintTextLink != null)
                {
                    _hintTextTimer += Time.deltaTime;
                }

                if (string.Compare(TrakStarDevice.ConnectionErrorMessage, "", StringComparison.Ordinal) != 0)
                {
                    hintTextLink.SetActive(true);
                    hintTextLink.GetComponent<TextMeshProUGUI>().text = TrakStarDevice.ConnectionErrorMessage;
                }

                break;
            }
            case ConnectionStatus.Ready:
            {
                if (connectionImageLink.activeInHierarchy) 
                {
                    connectionImageLink.SetActive (false);
                }

                if (!calibrationImageLink.activeInHierarchy && !metricsScriptLink.GetIsTrackingMetrics()) 
                {
                    calibrationImageLink.SetActive (true);
                }

                if (hintTextLink.activeInHierarchy)
                {
                    hintTextLink.SetActive (false);
                }

                break;
            }
            case ConnectionStatus.Disconnected:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /**
     * Updates the metric tracker with relevant forcep information. 
     * i.e. total distance traveled
     * param position - the position of the forcep?
     * param forcepIndex - the index of the forcep that the metric is being updated on
     */
    public void UpdateMetrics(Vector3 position, int forcepIndex)
    {

        //DISTANCE TRAVELLED BY FORCEPS METRICS
        if (metricsScriptLink.GetIsTrackingMetrics()) 
        { //IF THIS DOESN'T WORK, CHANGE WHAT WE TIE THE METRIC TO
            Vector3 distanceTraveledThisFrame = position - forceps [forcepIndex].forcepMasterGameObject.transform.position;
            //metricsScriptLink.totalDistanceTraveledByForceps += distanceTraveledThisFrame.magnitude;
            if (distanceTraveledThisFrame.magnitude + _storedDistanceTraveled >= forcepsMetricsDistanceThreshold) 
            {
                if (forcepIndex == 0) 
                {
                    metricsScriptLink.totalDistanceTraveledByForcepsA += distanceTraveledThisFrame.magnitude + _storedDistanceTraveled;
                } 
                else if (forcepIndex == 1) 
                {
                    metricsScriptLink.totalDistanceTraveledByForcepsB += distanceTraveledThisFrame.magnitude + _storedDistanceTraveled;
                }
                _storedDistanceTraveled = 0.0f;
            } 
            else 
            {
                _storedDistanceTraveled += distanceTraveledThisFrame.magnitude;
            }
        }
    }

    public void UpdateGraspCounter(int forcepIndex)
    {
        if (metricsScriptLink.GetIsTrackingMetrics()) 
        {
            if (forcepIndex == 0) 
            {
                metricsScriptLink.regraspCounterA++;
            } 
            else if (forcepIndex == 1) 
            {
                metricsScriptLink.regraspCounterB++;
            }
        }
    }

    public void ResetGraspedObjects()
    {
        
        foreach (Forcep forcep in forceps)
        {
            forcep.graspedObjects.Clear();
            forcep.objectNames[0].Clear();
            forcep.objectNames[1].Clear();
        }

    }


    //Switch the forceps model on screen when a numpad number is pressed
    void SwitchForcepsModel(int num)
    {
        const int LEFT_FORCEP = 0;
        const int RIGHT_FORCEP = 1;

        const int STRAIGHT_FORCEP = 0;
        const int BENT_FORCEP = 1;

        int forcepIndex;
        int deviceIndex;

        switch (num)
        {
            case 1:
                forcepIndex = LEFT_FORCEP;
                deviceIndex = STRAIGHT_FORCEP;
                break;
            case 2:
                forcepIndex = LEFT_FORCEP;
                deviceIndex = BENT_FORCEP;
                break;
            case 3:
                forcepIndex = RIGHT_FORCEP;
                deviceIndex = STRAIGHT_FORCEP;
                break;
            case 4:
                forcepIndex = RIGHT_FORCEP;
                deviceIndex = BENT_FORCEP;
                break;
            default:
                return;
        }

        forceps[forcepIndex].deviceIndex = deviceIndex;
        SetActiveForcepModel(forceps[forcepIndex]);
    }

    void SetActiveForcepModel(Forcep forcep)
    {
        for (int i = 0; i < forcep.devices.Length; i++)
        {
            forcep.devices[i].SetActive(i == forcep.deviceIndex);
        }
    }

}
