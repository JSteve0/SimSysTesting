using UnityEngine;
using System.Collections;
using System.Linq;

public class ForcepsColliderScript : MonoBehaviour {

	public GameObject[] insideTipColliders = new GameObject[2];

	private int bladeIndex;
	//private int offset = 0;
	private int masterIndex;

	private ReadTrakStar trakStarComponent;

	private MetricsTracker met_tracker;

	private IEnumerator halt_coroutine;

	// Use this for initialization
	void Start () 
	{
		bladeIndex = gameObject.name == "right" ? 0 : 1;
		masterIndex = gameObject.transform.parent.parent.name == "ForcepsMaster2" ? 1 : 0;
		
		trakStarComponent = gameObject.transform.parent.parent.parent.gameObject.GetComponent<ReadTrakStar> ();
	}

	// when the forceps come into contact with another rigidbody
	// this function begins
	void OnCollisionEnter(Collision collision) 
	{
		//Debug.Log ("OK");
		bool flag = false;
		//Debug.Log (collision.contacts.Length);
		foreach (ContactPoint contact in collision.contacts)
		{
			if (insideTipColliders.Any(t => contact.thisCollider.gameObject == t))
			{
				flag = true;
			}

			if (!flag)
				continue;
			
			if (contact.otherCollider.gameObject.GetComponent<Rigidbody>()) 
			{ //If the object to be grasped is NOT a compound collider object
				trakStarComponent.forceps[masterIndex].objectNames[bladeIndex].Add(contact.otherCollider.gameObject.name);

				// Delays the grasp counter
				halt_coroutine = grasp_Coroutine (1f);
				StartCoroutine (halt_coroutine);

			}
			else if (contact.otherCollider.gameObject.GetComponent<CompoundObjectFlag>() != null)
			{
				trakStarComponent.forceps[masterIndex].objectNames[bladeIndex].Add(contact.otherCollider.gameObject.GetComponent<CompoundObjectFlag>().parentObject.name);
			}
		}
	}

	// if the suture is grasped there is a delay before the grasp count is incremented again
	// this is used to prevent glitches where the grasp counter would increment more than once
	// after a single grasp
	private IEnumerator grasp_Coroutine (float waitTime)
	{
		// This is defined in Read track star. Accidently here?
		// Currently met_tracker is always null
		if (met_tracker != null)
		{
			met_tracker.regraspCounterTotal++;
		}
		yield return new WaitForSeconds (waitTime);
	}

	void OnCollisionExit(Collision collision) 
	{
		if (collision.gameObject.name != "Plane") 
		{
			trakStarComponent.forceps[masterIndex].objectNames[bladeIndex].Remove(collision.gameObject.name);
		}
	}
}
