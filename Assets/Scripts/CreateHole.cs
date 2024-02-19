using UnityEngine;
using System.Collections;

public class CreateHole : MonoBehaviour {

	public float thresholdAngle = 160f; //Set the threshold angle of the raycast to the surface normal; Above this angle a hole will be instantiated

	void OnTriggerEnter(Collider other) 
	{

		if (other.gameObject.layer == LayerMask.NameToLayer("Substrate")) 
		{ //Proceed only if the collider is in the "Substrate" layer

			Debug.Log ("Entered the trigger");

			Vector3 dir = this.transform.up; //Set the direction of the raycast equal to the UP (green) axis of the gameobject holding the script

			RaycastHit hit; //Define the variable to contain the raycasthit data

			if(Physics.Raycast(this.transform.position, dir, out hit, 0.1f)) 
			{ 
				//Cast a ray and get raycasthit data; Scalar determines max length of ray

				float angleBetween = Vector3.Angle (dir, hit.normal); //Calculate the angle between the raycast and the surface normal

				//Debug.Log (angleBetween);

				if (angleBetween > thresholdAngle) 
				{

					//Debug.Log ("I made a hole");

					//Vector3 pos = hit.point;

					GameObject hole = new GameObject();

					hole.transform.position = hit.point;
					hole.transform.rotation = Quaternion.FromToRotation(Vector3.up,hit.normal);

					//There needs to be a line of code here so that when the hole is instantiated, its axis is oriented parallel to hit.normal

					Rigidbody holeRb = hole.AddComponent<Rigidbody>();
					holeRb.useGravity = false;
					holeRb.mass = 1.0f;
					holeRb.drag = 5.0f;
					
					FixedJoint kevin = hole.AddComponent<FixedJoint> ();
					kevin.connectedBody = other.gameObject.GetComponent<Rigidbody> ();

					hole.AddComponent<CreateHoleColliders>();

				}
			}
		}
	}
}