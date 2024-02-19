using UnityEngine;
using System.Collections;

public class CreateHoleColliders : MonoBehaviour {

	public float minorRadius = 0.1f; //Radius of each component sphere collider
	public float majorRadius = 0.2f; //Distance from the center of each component sphere collider to the rotation axis
	public bool includeTrigger = true; //If true, will create the trigger when creating the hole

	void Start ()
	{
		//Make the ring

		int numColliders = Mathf.CeilToInt (2 * Mathf.PI * majorRadius / minorRadius);
		float rotationInterval = 360.0f / (numColliders);

		for (int i = 0; i < numColliders; i++)
		{
			GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			sphere.name = "Sphere" + i;
			sphere.transform.parent = gameObject.transform;
			sphere.transform.localScale = new Vector3(2 * minorRadius, 2 * minorRadius, 2 * minorRadius);
			sphere.transform.position = gameObject.transform.position + Quaternion.AngleAxis(i * rotationInterval, transform.up) * transform.right * majorRadius;
			sphere.GetComponent<MeshRenderer>().enabled = false;
			sphere.layer = 10; //Instantiate in "Inside" layer
		}

		//Make the trigger

		if (includeTrigger) {

			GameObject trigger = new GameObject ();
			trigger.name = "Trigger";
			trigger.transform.parent = gameObject.transform;
			trigger.transform.position = gameObject.transform.position;
			trigger.transform.rotation = transform.rotation;
			trigger.layer = 9; //Instantiate in "Trigger" layer
			trigger.AddComponent<HoleTriggerScript> ();

			BoxCollider triggerCollider = trigger.AddComponent<BoxCollider> ();
			triggerCollider.isTrigger = true;
			triggerCollider.size = new Vector3 (2*majorRadius, 2*minorRadius, 2*majorRadius);


		}
	}
}



