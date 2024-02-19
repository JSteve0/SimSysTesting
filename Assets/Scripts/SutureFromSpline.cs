using UnityEngine;
using ComponentMocks;

// TODO: Add comments

/// <summary>
/// Handles the suture logic and behavior.
/// Models the path of a bezier curve configured in the unity scene editor.
/// </summary>
[HelpURL("https://docs.unity3d.com/2020.3/Documentation/ScriptReference/ConfigurableJoint.html")]
public class SutureFromSpline : MonoBehaviour {

	[Header("GameObject Links")]

	[SerializeField] private BezierSpline sutureSpline;
	// Needed to reset the forcep grasping objects when suture resets.
	[SerializeField] private ReadTrakStar readTrakStar;
	// Used to keep track of the number of resets.
	[SerializeField] private MetricsTracker metricsTracker;

	[Header("Prefab Links")]

	[SerializeField] private GameObject nodePrefab;

    [Space]

    [Header("Suture Settings")]

	[Tooltip("Scale of each node. Used to calculate total number of nodes")]
	[SerializeField] private float scaleFactor;

	[Header("Configurable Joint Settings")]

	[Range(0f, 50f)]
    [Tooltip("Angular limit cannot be between 0 and 3. If set to 0, spring and dampener are ignored and joint is inelastic. Recommended value between 3 - 10.")]
    [SerializeField] private float angularLimit = 7.5f;

    [Tooltip("Set the movement along the X, Y or Z axes to be Free, completely Locked, or Limited according to the limit properties described below.")]
	[SerializeField] private ConfigurableJointMotion xMotion = ConfigurableJointMotion.Locked;

	[Tooltip("Set the movement along the X, Y or Z axes to be Free, completely Locked, or Limited according to the limit properties described below.")]
	[SerializeField] private ConfigurableJointMotion yMotion = ConfigurableJointMotion.Locked;

	[Tooltip("Set the movement along the X, Y or Z axes to be Free, completely Locked, or Limited according to the limit properties described below.")]
	[SerializeField] private ConfigurableJointMotion zMotion = ConfigurableJointMotion.Locked;

	[Tooltip("Set the rotation around the X, Y or Z axes to be Free, completely Locked, or Limited according to the limit properties described below.")]
	[SerializeField] private ConfigurableJointMotion angularXMotion = ConfigurableJointMotion.Limited;

	[Tooltip("Set the rotation around the X, Y or Z axes to be Free, completely Locked, or Limited according to the limit properties described below.")]
	[SerializeField] private ConfigurableJointMotion angularYMotion = ConfigurableJointMotion.Limited;

	[Tooltip("Set the rotation around the X, Y or Z axes to be Free, completely Locked, or Limited according to the limit properties described below.")]
	[SerializeField] private ConfigurableJointMotion angularZMotion = ConfigurableJointMotion.Limited;

	[SerializeField] private LinearLimitSpringStruct mLinearLimitSpring = new LinearLimitSpringStruct(2f, 5f);

	[SerializeField] private JointDriveStruct angularXDrive = new JointDriveStruct(2f, 5f, Mathf.Infinity);

    [SerializeField] private JointDriveStruct angularYZDrive = new JointDriveStruct(2f, 5f, Mathf.Infinity);

    //[SerializeField] private ConfigurableJointMock ConfigurableJointMock;

    [System.Serializable]
    public struct LinearLimitSpringStruct
    {
		public float spring;
		public float damper;

        public LinearLimitSpringStruct(float spring, float damper)
        {
			this.spring = spring;
			this.damper = damper;
        }
    }

    [System.Serializable]
    public struct JointDriveStruct
    {
        public float spring;
        public float damper;
		public float maxForce;

		public JointDriveStruct(float spring, float damper, float maxForce)
        {
            this.spring = spring;
            this.damper = damper;
			this.maxForce = maxForce;
        }
    }

    [System.Serializable]
    public struct SoftJointLimitSpringStruct
    {
        public float spring;
        public float damper;
        public float maxForce;

        public SoftJointLimitSpringStruct(float spring, float damper, float maxForce)
        {
            this.spring = spring;
            this.damper = damper;
            this.maxForce = maxForce;
        }
    }


    [SerializeField] private SoftJointLimitSpringStruct angularXLimitSpring = new SoftJointLimitSpringStruct { spring = 0f, damper = 0f };
    [SerializeField] private SoftJointLimitSpringStruct angularYZLimitSpring = new SoftJointLimitSpringStruct { spring = 0f, damper = 0f };

	[Tooltip("If a force larger than this value pushes the joint beyond its constraints then the joint is permanently “broken” and deleted. Cannot be broken if set to 0.")]
	[Min(0f)]
    [SerializeField] private float breakForce = 1_000_000f;

    private int _numNodes; // Calculated using scale factor and length of bezier
	private Vector3[] _nodePositions;
	private Vector3[] _nodeRotations;
	private NodeScript[] _nodeArray;

	// Use this for initialization
	void Start () 
	{
		// Enforce singleton
		if (gameObject.transform.childCount == 0)
		{
			InitSuture ();
		}
		else
		{
			SetSuture();
		}

		if (readTrakStar == null)
		{
			Debug.LogWarning ("ReadTrakStar is null on the SutureFromSpline script.\nInclude direct link to improve performance.", gameObject);
			//readTrakStar = GameObject.Find("SurgicalTools").GetComponent<ReadTrakStar>();
		}
		if (metricsTracker == null)
		{
			Debug.LogWarning ("MetricsTracker is null on the SutureFromSpline script.\nInclude direct link to improve performance.", gameObject);
			metricsTracker = GameObject.Find("MetricsTracker").GetComponent<MetricsTracker>();
		}

	}

	// Update is called once per frame
	void Update () 
	{
		
		if (Input.GetKey (KeyCode.R)) 
		{
			ResetSuture();
		}

		#if UNITY_EDITOR
		UnityInspectorUpdate();
		#endif


		GravityOffIfInside ();
		ColorNodesByStress();
		CheckSuture();
	}
		
	// function calculates the coordinates for the spline
	// the spline is setup as a series of nodes
	// if an object interacts with a given the node
	// the adjacent nodes are also affected depending
	// on the collision and movement of the force
	// acting on the node
	private void CalculateCoordinates() 
	{
		
		//Calculate length of spline curve by dividing into segments

		float splineLength = 0f;
		const float numSegments = 100f;

		for (int i = 1; i < numSegments; i++) 
		{
			splineLength += Vector3.Distance (sutureSpline.GetPoint (i / numSegments), sutureSpline.GetPoint ((i - 1) / numSegments));
		}

		_numNodes = Mathf.RoundToInt (splineLength / (scaleFactor * nodePrefab.transform.localScale.y)); //Calculates the number of nodes needed to fill the spline
        //_numNodes = Mathf.RoundToInt(splineLength / scaleFactor);

        _nodePositions = new Vector3[_numNodes]; //An array where the ith element contains the position of the ith node
		_nodeRotations = new Vector3[_numNodes]; //An array where the ith element contains the rotation of the ith node
		_nodePositions [0] = sutureSpline.GetPoint (0);
		_nodeRotations [0] = sutureSpline.GetDirection (0);

		float t = 0;

		for (int i = 1; i < _numNodes; i++) 
		{

			float delta = Vector3.Distance (sutureSpline.GetPoint (t), _nodePositions [i - 1]);

			do 
			{
				t += 0.0001f;
				delta = Vector3.Distance (sutureSpline.GetPoint (t), _nodePositions [i - 1]);
			} 
			while (delta < scaleFactor);

			_nodePositions [i] = sutureSpline.GetPoint (t);
			_nodeRotations [i] = sutureSpline.GetDirection (t);

		}
	}

	// function creates the individual nodes for the spline
	private void CreateNodes() 
	{
		
		_nodeArray = new NodeScript[_numNodes];

		NodeScript prev = null;

		for (int i = 0; i < _numNodes; ++i) 
		{

			GameObject newNode = Instantiate(nodePrefab, transform);

			if (!newNode.TryGetComponent(out NodeScript nodeScript))
			{
				Debug.LogError("Error creating suture nodes. Node prefab must have a NodeScript component attached.");
			}

			if (prev != null)
			{
				nodeScript.Previous = prev;
				prev.Next = nodeScript;
			}
				
			newNode.name = "Node" + i;
			newNode.transform.localScale = new Vector3 (scaleFactor * newNode.transform.localScale.x, scaleFactor * newNode.transform.localScale.y, scaleFactor * newNode.transform.localScale.z);

			newNode.transform.SetPositionAndRotation(_nodePositions[i], Quaternion.FromToRotation (newNode.transform.up, _nodeRotations [i]));

			//Check if the node overlaps with a collider in the "Substrate" layer

			Collider[] hitColliders = Physics.OverlapSphere (newNode.transform.position, scaleFactor, 1 << 8); //Note the layer mask

			if (hitColliders.Length > 0) 
			{
				newNode.layer = 10;
			}

			_nodeArray [i] = nodeScript;
			prev = nodeScript;
		}

		//Make first node the same color as the rest
		//nodeArray[0].Renderer.material.color = nodeArray[1].Renderer.material.color;
	}
	
	private void SetSuture()
	{

		_numNodes = transform.childCount;
		_nodeArray = new NodeScript[_numNodes];

		for (int i = 0; i < _numNodes; ++i) 
		{
			var node = transform.GetChild(i).gameObject;
			if (node.TryGetComponent(out NodeScript nodeScript))
			{
				_nodeArray[i] = nodeScript;
			}
			else
			{
				Debug.LogError("Error creating suture nodes. Node prefab must have a NodeScript component attached.");
			}
		}
	}

	// Creates the suture joints that hold it together and make it act like a fish line
	private void CreatePrimaryLinks() 
	{
		// Reduce garbage collection
		float offset = 0.66f;
		Vector3 anchor = new Vector3(0,-offset,0);
		Vector3 connectedAnchor = new Vector3(0,offset,0);
		
		Debug.Log("CreatePrimaryLinks");
		
		// Joint settings
		for (int i = 1; i < _numNodes; i++) 
		{ 

			ConfigurableJoint bob = _nodeArray[i].gameObject.AddComponent<ConfigurableJoint>();

			bob.connectedBody = _nodeArray[i - 1].GetComponent<Rigidbody>();

			bob.axis = Vector3.up;

			bob.autoConfigureConnectedAnchor = false;
			bob.anchor = anchor;
			bob.connectedAnchor = connectedAnchor;

			bob.xMotion = xMotion;
			bob.yMotion = yMotion;
			bob.zMotion = zMotion;
			bob.angularXMotion = angularXMotion;
			bob.angularYMotion = angularYMotion;
			bob.angularZMotion = angularZMotion;
            
			bob.linearLimitSpring = new SoftJointLimitSpring { spring = mLinearLimitSpring.spring, damper = mLinearLimitSpring.damper };
			bob.angularXLimitSpring = new SoftJointLimitSpring { spring = angularXLimitSpring.spring, damper = angularXLimitSpring.damper };
            bob.angularYZLimitSpring = new SoftJointLimitSpring { spring = angularYZLimitSpring.spring, damper = angularYZLimitSpring.damper };

            bob.angularXDrive = new JointDrive { maximumForce = angularXDrive.maxForce, positionSpring = angularXDrive.spring, positionDamper = angularXDrive.damper };
			bob.angularYZDrive = new JointDrive { maximumForce = angularYZDrive.maxForce, positionSpring = angularYZDrive.spring, positionDamper = angularYZDrive.damper };

            //bob.targetRotation = nodeArray[i].transform.rotation;

			// Maximum angular rotation of a joint between two nodes in degrees.
			SoftJointLimit bobAngularLimit = new SoftJointLimit{ limit = angularLimit }; 

			// Maximum angular rotation of a joint between two nodes in degrees.
            bob.angularYLimit = bobAngularLimit;
			bob.angularZLimit = bobAngularLimit;

			// Makes hit detections worse
			//bob.projectionMode = JointProjectionMode.PositionAndRotation;

			//bob.enablePreprocessing = false;

			bob.breakForce = breakForce;

			bob.enableCollision = false;
		}
	}

	// colors the node based on how much stress they are under. Stress is calculated by the distance between sequential nodes.
	private void ColorNodesByStress() 
	{

		for (int i = 1; i < _numNodes; i++) 
		{

			float sep = 2f*Mathf.Min (Vector3.Distance (_nodeArray [i].transform.position, _nodeArray [i - 1].transform.position) / scaleFactor - 1, 0.5f);
			//nodeArray [i].GetComponent<Renderer> ().material.color = new Color (sep,0,1f-sep, 1f);
		}
	}

	private void GravityOffIfInside() 
	{
		for (int i = 1; i < _numNodes; i++)
		{
			_nodeArray[i].SetIsUsingGravity(_nodeArray[i].gameObject.layer != 10);
		}
	}

    [ContextMenu("Spawn Suture")]
    // initializes the suture and all its components
    public void InitSuture() 
	{
	
		CalculateCoordinates ();
		CreateNodes ();
		CreatePrimaryLinks ();
	
	}

    [ContextMenu("Delete Suture")]
    private void DeleteSutureInInspector()
	{
		DeleteSuture();
    }

    public void DeleteSuture()
    {
	    while (transform.childCount > 0)
	    {
		    DestroyImmediate(transform.GetChild(0).gameObject);
	    }
    }

    // resets the suture to its initial state
    public void ResetSuture() 
	{
		// TODO: enable correctly
		//readTrakStar.ResetGraspedObjects();

        DeleteSuture();
		
		if (metricsTracker != null)
			++metricsTracker.sutureResets;

		InitSuture ();
	}

	private void CheckSuture()
	{
		for (int i = 0; i < _numNodes - 1; ++i)
		{
			float distance = Vector3.Distance (_nodeArray[i].transform.position, _nodeArray[i + 1].transform.position);
			if (distance > 1.0f)
			{
				Debug.Log("Suture error");
				Debug.Log("Respawning Suture");
				//resetSuture();
			}
		}
	}

	private void UnityInspectorUpdate()
	{
		int interval = 20;
		int curr = Time.frameCount % interval;
		for (int i = curr; i < _numNodes; i += interval)
		{
			// First node does not have a joint
			if (i == 0)
				continue;

			if (!_nodeArray[i].TryGetComponent(out ConfigurableJoint bob))
				continue;

			bob.xMotion = xMotion;
			bob.yMotion = yMotion;
			bob.zMotion = zMotion;
			bob.angularXMotion = angularXMotion;
			bob.angularYMotion = angularYMotion;
			bob.angularZMotion = angularZMotion;

			SoftJointLimit bobAngularLimit = new SoftJointLimit{ limit = angularLimit }; 

			// Maximum angular rotation of a joint between two nodes in degrees.
            bob.angularYLimit = bobAngularLimit;
			bob.angularZLimit = bobAngularLimit;

            //bob.linearLimitSpring = linearLimitSpring;
            //bob.angularXDrive = new JointDrive { maximumForce = angularXDrive.maxForce, positionSpring = angularXDrive.spring, positionDamper = angularXDrive.damper };
            //bob.angularYZDrive = new JointDrive { maximumForce = angularYZDrive.maxForce, positionSpring = angularYZDrive.spring, positionDamper = angularYZDrive.damper };

            //bob.angularXDrive = new JointDrive { maximumForce = angularXDrive.maxForce, positionSpring = angularXDrive.spring, positionDamper = angularXDrive.damper };
            //bob.angularYZDrive = new JointDrive { maximumForce = angularYZDrive.maxForce, positionSpring = angularYZDrive.spring, positionDamper = angularYZDrive.damper };

            bob.breakForce = breakForce;
		}
	}

	public NodeScript[] GetNodes()
	{
        return _nodeArray;
	}

}
