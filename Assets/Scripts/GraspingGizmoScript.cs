using UnityEngine;
using System.Collections;

public class GraspingGizmoScript : MonoBehaviour {

	void OnDrawGizmosSelected() 
	{
		Gizmos.color = Color.red;

		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireCube (Vector3.zero, Vector3.one);
	}
	
}
