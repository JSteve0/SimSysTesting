using UnityEngine;

public class HoleTriggerScript : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		other.gameObject.layer = 10; //Move to "Inside" layer
		other.gameObject.GetComponent<Rigidbody>().useGravity = false; //Disable gravity
	
	}

	private void OnTriggerExit(Collider other)
	{
		Vector3 gru = other.transform.position;
		Vector3 gruRelative = gameObject.transform.InverseTransformPoint (gru);

		if (gruRelative.y > gameObject.transform.localPosition.y) 
		{
			other.gameObject.layer = 0; //Move to "Default" layer
			other.gameObject.GetComponent<Rigidbody>().useGravity = true; //Enable gravity
		}
	}
}
