using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SubstrateMetricsScript : MonoBehaviour {
	
	// series of variables that grants this class access
	// to the metrics variables needed for measurements and counting
	public GameObject metricsObjectLink;
	private MetricsTracker metricsScriptLink;
	public bool countContacts = true;

	// Use this for initialization
	void Start () 
	{
		metricsScriptLink = metricsObjectLink.GetComponent<MetricsTracker> ();
	}

	// if the forceps collide with any game object this function starts
	void OnCollisionEnter (Collision other) 
	{
		
		if (other.gameObject.tag == "Forceps" && countContacts) 
		{

			countContacts = false;
			Invoke("ActivateCounting",1f);

			// if the master1 set of forceps collides with an object, the counter increases
			// otherwise the master2 counter increases
			if (other.gameObject.transform.parent.parent.gameObject.name == "ForcepsMaster1") 
			{
				metricsScriptLink.forcepsToTissueContactCounterA++;

			} 
			else if (other.gameObject.transform.parent.parent.gameObject.name == "ForcepsMaster2") 
			{
				metricsScriptLink.forcepsToTissueContactCounterB++;
			}
		}
			
	}

	void ActivateCounting () 
	{
		countContacts = true;
	}

}
