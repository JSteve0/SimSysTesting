﻿// Used staticly in BezierSpline.cs
// Source code and more documentation: https://catlikecoding.com/unity/tutorials/curves-and-splines/

using UnityEngine;

public static class Bezier {

	/**
	 * returns the bezier point containing 3 vectors and a float 
	 * Uses the following formula: B(t) = (1 - t)^2 P0 + 2 (1 - t) t P1 + t^2 P2.
	 */
	public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, float t) 
	{
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			oneMinusT * oneMinusT * p0 +
			2f * oneMinusT * t * p1 +
			t * t * p2;
	}

	/**
	 * returns the first bezier derivative containing args of 3 vectors and a float
	 * Uses the following formula: B'(t) = 2 (1 - t) (P1 - P0) + 2 t (P2 - P1)
	 */
	public static Vector3 GetFirstDerivative (Vector3 p0, Vector3 p1, Vector3 p2, float t) 
	{
		return
			2f * (1f - t) * (p1 - p0) +
			2f * t * (p2 - p1);
	}

	/**
	 * returns the first bezier point containing 4 vectors and a float
	 * Uses the following formula: B(t) = (1 - t)^3 P0 + 3 (1 - t)^2 t P1 + 3 (1 - t) t^2 P2 + t^3 P3
	 */
	public static Vector3 GetPoint (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) 
	{
		t = Mathf.Clamp01 (t);
		float oneMinusT = 1f - t;
		return
			oneMinusT * oneMinusT * oneMinusT * p0 +
			3f * oneMinusT * oneMinusT * t * p1 +
			3f * oneMinusT * t * t * p2 +
			t * t * t * p3;
	}

	/**
	 * gets the first bezier derivative of 3 vectors and a float
	 * Uses the following formula: B'(t) = 3 (1 - t)^2 (P1 - P0) + 6 (1 - t) t (P2 - P1) + 3 t^2 (P3 - P2)
	 */
	public static Vector3 GetFirstDerivative (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) 
	{
		t = Mathf.Clamp01(t);
		float oneMinusT = 1f - t;
		return
			3f * oneMinusT * oneMinusT * (p1 - p0) +
			6f * oneMinusT * t * (p2 - p1) +
			3f * t * t * (p3 - p2);
	}
}