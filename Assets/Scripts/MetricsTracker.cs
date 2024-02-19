using UnityEngine;
using System;
using UnityEngine.UI; 

// TODO: Add comments
// TODO: Make metrics private and add getters/setters
// TODO: Transfer input handling to InputHandler.cs

/// <summary>
/// Stores and updates metrics related to the users performance in the simulator.
/// </summary>
public class MetricsTracker : MonoBehaviour {

	private bool _readyToGo = false; //when true, starts tracking for metrics

	[Header("Measured Metrics")]
	
	//METRICS TO BE TRACKED
	public int regraspCounterTotal; //IMPLEMENTED in metricsTracker.cs
	public int regraspCounterA; //IMPLEMENTED in readTrakStar.cs
	public int regraspCounterB; //IMPLEMENTED in readTrakStar.cs

	[Tooltip("Maximum suture depth in the substrate")]
	public float maximumSutureDepth; //IMPLEMENTED in simpleSuture.cs
	[Tooltip("Current suture depth in the substrate")]
	public float currentSutureDepth; //IMPLEMENTED in simpleSuture.cs

	public int forcepsToTissueContactCounterTotal; //IMPLEMENTED in metricsTracker.cs
	public int forcepsToTissueContactCounterA; //IMPLEMENTED in substrateMetricsScript.cs
	public int forcepsToTissueContactCounterB; //IMPLEMENTED in substrateMetricsScript.cs

	public float totalDistanceTraveledByForcepsTotal; //IMPLEMENTED in metricsTracker.cs
	public float totalDistanceTraveledByForcepsA; //IMPLEMENTED in readTrakStar.cs
	public float totalDistanceTraveledByForcepsB; //IMPLEMENTED in readTrakStar.cs

	//METRICS TO BE TRACKED AND CURRENTLY IN TESTING
	public float averageForcepVelocityTotal;
	public float averageForcepVelocityA;
	public float averageForcepVelocityB;

	//METRICS TO BE TRACKED AND CURRENTLY IN TESTING
	[Tooltip("Number of times the user reset the suture")]
	public int sutureResets = 0;

	[Header("Object Links")]
	
	public GameObject uiLink;
	private Text _uiLinkText;

	private int _metricsMode = 0;
	
	[Header("Misc.")]
	public float timer; //IMPLEMENTED in metricsTracker.cs

	// Use this for initialization
	private void Start () 
	{
		uiLink = GameObject.Find ("MetricsText"); // sets up MetricsText to be the displayed text
		_uiLinkText = uiLink.GetComponent<Text> (); 
		uiLink.SetActive (true); // makes metric text visible
		_readyToGo = false; // boolean value starts false and turns true when the timer is ready to begin
	}

	// Update is called once per frame
	private void Update () 
	{
		if (Input.GetKeyDown (KeyCode.M)) 
		{ //M key toggles metrics ui on/off
			uiLink.SetActive (!uiLink.activeInHierarchy);
		}
		if (Input.GetKeyUp (KeyCode.N)) 
		{ 
			//N key toggles through total metrics, forceps A metrics, and forceps B metrics
			_metricsMode++;

			//loop back to the beginning if out of bounds
			if (_metricsMode > 2) {
				_metricsMode = 0;
			}
		}
		if (_readyToGo) 
		{ //do metrics that update every frame here (totals and time)
			timer += Time.deltaTime;
			regraspCounterTotal = regraspCounterA + regraspCounterB;
			forcepsToTissueContactCounterTotal = forcepsToTissueContactCounterA + forcepsToTissueContactCounterB;
			totalDistanceTraveledByForcepsTotal = totalDistanceTraveledByForcepsA + totalDistanceTraveledByForcepsB;
			
			// Velocity = displacment / time
			averageForcepVelocityA = totalDistanceTraveledByForcepsA / timer;
			averageForcepVelocityB = totalDistanceTraveledByForcepsB / timer;
			averageForcepVelocityTotal = averageForcepVelocityA + averageForcepVelocityB;
		}

		//Update gui text
		_uiLinkText.text = GetMetrics(_metricsMode);
	}

	//shows metrics based on mode: total metrics, forceps A metrics, and forceps B metrics and returns updated gui text.
	private string GetMetrics(int mode) 
	{
		const int total = 0;
		const int left = 1;
		const int right = 2;

		string metricsGUIText = "";
		
		switch (mode)
		{
			case total:
				metricsGUIText = "Time: " + Math.Round((Decimal)timer, 1) + "\nGrasps (T): " + regraspCounterTotal + "\nTissue Contacts (T): " + forcepsToTissueContactCounterTotal + "\nPath Length (T): " + Math.Round((Decimal)totalDistanceTraveledByForcepsTotal, 2);
				break;
			case left:
				metricsGUIText = "Time: " + Math.Round((Decimal)timer, 1) + "\nGrasps (R): " + regraspCounterA + "\nTissue Contacts (R): " + forcepsToTissueContactCounterA + "\nPath Length (R): " + Math.Round((Decimal)totalDistanceTraveledByForcepsA, 2);
				break;
			case right:
				metricsGUIText = "Time: " + Math.Round((Decimal)timer, 1) + "\nGrasps (B): " + regraspCounterB + "\nTissue Contacts (B): " + forcepsToTissueContactCounterB + "\nPath Length (B): " + Math.Round((Decimal)totalDistanceTraveledByForcepsB, 2);
				break;
		}

		return metricsGUIText;
	}

	public void SetIsTrackingMetrics(bool isTracking)
	{
		_readyToGo = isTracking;
	}

	public bool GetIsTrackingMetrics()
	{
		return _readyToGo;
	}
	
}
