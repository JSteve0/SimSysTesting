using UnityEngine;

/// <summary>
/// Handles cube logic and behavior.
/// </summary>
/// <remarks>Currently only used in the Pegboard scene</remarks>
public class CubeScript : MonoBehaviour {
	
	/// <summary>
	/// A reference to the cube fountain.
	/// </summary>
	public GameObject cubeFountain;
	
	/// <summary>
	/// A reference to the cube's rigidbody.
	/// </summary>
	private Rigidbody _cubeRigidbody;

	/// <summary>
	/// Unity Start function. Called once when the script is created.
	/// </summary>
	private void Start ()
	{
		_cubeRigidbody = gameObject.GetComponent<Rigidbody>();
	}
	
	/// <summary>
	/// Unity Update function. Called once per frame.
	/// </summary>
	private void Update () 
	{
		if (gameObject.transform.position.y < -2) 
		{
			gameObject.transform.position = cubeFountain.transform.position;
			_cubeRigidbody.velocity = new Vector3(0,0,0);
		}
	}
}
