using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Spawns cubes in the Pegboard scene.
/// </summary>
public class SpawnCubes : MonoBehaviour {

	[Header("Settings")]
	
	/// <summary> The number of cubes to spawn. </summary>
	[SerializeField] private int numCubes = 32;
	
	/// <summary> The edge length of the cubes.</summary>
	[SerializeField] private float cubeSize = 0.18f;

	[Header("Prefab Links")]
	
	/// <summary>A reference to the cube prefab.</summary>
	[SerializeField] private GameObject cubePrefab;
	
	private int _currentCubeCount;

	// Use this for initialization
	private void Start () 
	{
		_currentCubeCount = numCubes;
		InvokeRepeating (nameof(CreateCube), 1,0.1f);
	}

	private void CreateCube () 
	{

		GameObject myCube = Instantiate (cubePrefab, gameObject.transform.position, Random.rotation);

		myCube.name = "Cube" + _currentCubeCount;

		myCube.transform.localScale = new Vector3 (cubeSize,cubeSize,cubeSize);

		myCube.GetComponent<CubeScript>().cubeFountain = gameObject;

		if (--_currentCubeCount == 0)
			CancelInvoke (nameof(CreateCube));
	}
}
