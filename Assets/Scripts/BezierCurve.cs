// Currently not in use.
// Source code and more documentation: https://catlikecoding.com/unity/tutorials/curves-and-splines/

using UnityEngine;

public class BezierCurve : MonoBehaviour {
	
	public Vector3[] points;
	
	// returns the bezier point
	public Vector3 GetPoint (float t) 
	{
		return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
	}
	
	// returns the velocity at the given frame 
	public Vector3 GetVelocity (float t) 
	{
		return transform.TransformPoint(Bezier.GetFirstDerivative(points[0], points[1], points[2], points[3], t)) - transform.position;
	}
	
	// returns the direction using the velocity function
	public Vector3 GetDirection (float t) 
	{
		return GetVelocity(t).normalized;
	}
	
	// resets the bezier curve
	public void Reset () 
	{
		points = new Vector3[] 
		{
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f),
			new Vector3(4f, 0f, 0f)
		};
	}
}
