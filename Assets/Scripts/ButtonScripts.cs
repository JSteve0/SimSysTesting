using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ButtonScripts : MonoBehaviour {

	public MetricsTracker metricsLink;

	void Start() 
	{
		if (GameObject.Find ("MetricsTracker")) 
		{
			metricsLink = GameObject.Find ("MetricsTracker").GetComponent<MetricsTracker> ();
		}
	}

	public void quitApplication ()
	{
		Application.Quit ();
	}

	public void loadScene () 
	{
		TrackStar.Disconnect ();
		TrackStar.IsCalibrated = false;
		Debug.Log("Button Pressed!");
		string sceneName = EventSystem.current.currentSelectedGameObject.name; //make sure that the button object is named the exact same thing as the scene we want to switch to, thanks Brian
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}

  /*
  * Saves current users data to this pc. The data that is saved is the time 
  * stamp, total time, total grasps, total tissue contacts, total path length, 
  * total grasps (R), total tissue contacts (R), total path length (R),
  * total grasps (B), total tissue contacts (B), total path length (B),
  * where R is the right forcep and B is the bottom forcep. R + B for
  * any stat will you get you the overall category at the top.
  */
	public void SaveData ()
	{
		if (!metricsLink.GetIsTrackingMetrics()) 
			return;
		
		DateTime timestamp = DateTime.Now;
		string singleDigitSecond = "";
		if (timestamp.Second < 10) 
		{
			singleDigitSecond = "0";
		}
			
		string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\MSS_savefiles\\" + timestamp.Year.ToString () + timestamp.Month.ToString () + timestamp.Day.ToString () + "-" + timestamp.Hour.ToString ()
		              + timestamp.Minute.ToString () + singleDigitSecond + timestamp.Second.ToString () + ".txt";
		string metricsData = "Time: " + Math.Round ((Decimal)metricsLink.timer, 1) + "\r\nGrasps (T): " + metricsLink.regraspCounterTotal + "\r\nTissue Contacts (T): " +
		                     metricsLink.forcepsToTissueContactCounterTotal + "\r\nPath Length (T): " + Math.Round ((Decimal)metricsLink.totalDistanceTraveledByForcepsTotal, 2) + "\r\nGrasps (R): " + metricsLink.regraspCounterA
		                     + "\r\nTissue Contacts (R): " + metricsLink.forcepsToTissueContactCounterA + "\r\nPath Length (R): " + Math.Round ((Decimal)metricsLink.totalDistanceTraveledByForcepsA, 2) + "\r\nGrasps (B): "
		                     + metricsLink.regraspCounterB + "\r\nTissue Contacts (B): " + metricsLink.forcepsToTissueContactCounterB + "\r\nPath Length (B): " + Math.Round ((Decimal)metricsLink.totalDistanceTraveledByForcepsB, 2);
		System.IO.File.WriteAllText (path, metricsData);
	}

}
