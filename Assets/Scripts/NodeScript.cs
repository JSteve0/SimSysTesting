using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles node logic and behavior. Script is attached to each node on the suture.
/// </summary>
public class NodeScript : MonoBehaviour
{
    private int _nodeIndex;

    private ConfigurableJoint _configurableJoint;
    private Rigidbody _rigidbody;
    private Renderer _renderer;
    private CapsuleCollider _capsuleCollider;

    private NodeScript _next;
    private NodeScript _previous;
    
    private GameObject _orginalParent;

    private float _maxAverageAngle;

    public int NodeIndex
    {
        get => _nodeIndex;
        set => _nodeIndex = value;
    }

    public Renderer Renderer => _renderer;

    public CapsuleCollider CapsuleCollider=> _capsuleCollider;

    public NodeScript Next
    {
        get => _next;
        set => _next = value;
    }
    public NodeScript Previous
    {
        get => _previous;
        set => _previous = value;
    }

    public float MaxAverageAngle
    {
        get => _maxAverageAngle;
        set => _maxAverageAngle = value;
    }

    // TODO: Add a link to the next node in the suture
    // TODO: Add a link to the previous node in the suture
    // TODO: Store the index of the node in the suture

    /// <summary>
    /// Unity method, called when script is initialized.
    /// </summary>
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _configurableJoint = GetComponent<ConfigurableJoint>();
        _renderer = GetComponent<Renderer>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _orginalParent = transform.parent.gameObject;
    }

    /// <summary>
    /// Unity event method. Called when a joint attached to the same game object break
    /// </summary>
    /// <param name="breakForce">Amount of force that broke the joint</param>
    private void OnJointBreak(float breakForce)
    {
        _rigidbody.velocity = new Vector3(0f, 0f, 0f);
    }

    public void FreezeRotation()
    {
        if (_rigidbody == null)
        {
            return;
        }
        _rigidbody.freezeRotation = true;
    }
    
    public void ResetParent()
    {
        transform.parent = _orginalParent.transform;
    }
    
    public void SetIsKinematic(bool state)
    {
        _rigidbody.isKinematic = state;
    }
    
    public void SetIsUsingGravity(bool state)
    {
        _rigidbody.useGravity = state;
    }

}
