using JetBrains.Annotations;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Detects and handles knot tie events.
/// </summary>
public class KnotTieDetectionScript : MonoBehaviour
{

    [Header("Parameters")]

    [SerializeField]
    [Min(1)]
    private int NodesPerKnot = 5;

    [SerializeField]
    private float AverageAngleThreshold = 30f;

    [Header("Object Links")]

    [SerializeField]
    private ForcepScript LeftForcep;

    [SerializeField] 
    private ForcepScript RightForcep;

    [SerializeField]
    private SutureFromSpline SutureScript;

    [SerializeField]
    private NodeScript[] nodes;

    private bool IsBothForcepsGrasping => LeftForcep.IsGraspingObject && RightForcep.IsGraspingObject;

    private bool KnotFlag = false;
    private HashSet<NodeScript> nodesInKnot = new HashSet<NodeScript>();

    // Start is called before the first frame update
    private void Start()
    {
        nodes = SutureScript.GetNodes();
    }

    // Update is called once per frame
    private void Update()
    {
        if (nodes != null && nodes.Length == 0)
        {
            nodes = SutureScript.GetNodes();
        } else if (nodes == null && SutureScript.GetNodes() != null)
        {
            nodes = SutureScript.GetNodes();
        }

        if (nodes == null)
        {
            return;
        }

        if (nodes[0] == null)
        {
            nodes = SutureScript.GetNodes();
        }

        if (IsBothForcepsGrasping)
        {
            CheckAverageAngleBetweenNodes();
            //ColorNodes();
        } else if (KnotFlag)
        {
            LockKnotInPlace();
            KnotFlag = false;
        }
    }

    private void CheckAverageAngleBetweenNodes()
    {
        if (nodes == null)
            return;

        for (int i = 0; i < nodes.Length - 1; ++i)
        {
            nodes[i].MaxAverageAngle = 0;
        }


        for (int i = 0; i < nodes.Length - 1; ++i)
        {
            float angle = 0f;
            for (int j = i; j < i + NodesPerKnot; ++j) {
                if (j == nodes.Length - 1)
                    break;
                Vector3 difference = nodes[j].transform.up - nodes[j + 1].transform.up;
                angle += Mathf.Abs(difference.x) + Mathf.Abs(difference.y) + Mathf.Abs(difference.z);
            }

            if ((angle / NodesPerKnot) > AverageAngleThreshold) {
                KnotFlag = true;
                for (int j = i; j < i + NodesPerKnot; ++j)
                {
                    if (j == nodes.Length - 1)
                        break;
                    
                    nodes[i].MaxAverageAngle = angle;
                    nodesInKnot.Add(nodes[i]);
                }
            }
        }
    }

    private void LockKnotInPlace()
    {
        foreach (NodeScript node in nodesInKnot)
        {
            node.FreezeRotation();
        }
    }

    private void ColorNodes()
    {
        if (nodes == null)
            return;

        for (int i = 0; i < nodes.Length - 1; i++)
        {
            NodeScript nodeScript = nodes[i];
            if (nodeScript.MaxAverageAngle == 0) {
                nodeScript.Renderer.material.color = Color.gray;
            } else
            {
                nodeScript.Renderer.material.color = Color.green;
            }
        }
    }

}
