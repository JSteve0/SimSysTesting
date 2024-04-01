using System;
using System.Collections.Generic;
using UnityEngine;
using Obi;

/// <summary>
/// Handles Forceps logic. Attached to each forceps. Specifically handles grasping and releasing objects and modifying the forceps position, rotation, and blade percent open.
/// </summary>
public class ForcepScript : MonoBehaviour
{
    
    [Header("Object Links")]
    
    [SerializeField] private GameObject leftBlade;
    [SerializeField] private GameObject rightBlade;
    [SerializeField] private ForcepsColliderScriptEnhanced leftForcepsColliderScript;
    [SerializeField] private ForcepsColliderScriptEnhanced rightForcepsColliderScript;
    [SerializeField] private ObiCollisionMaterial stickyMaterial;

    [Header("Settings")]
    
    [Min(0)]
    [SerializeField] 
    [Tooltip("Index of the forcep. Used to Differentiate between the two forceps. Make sure one forcep is set to 0 and the other is set to 1")]
    private int forcepIndex;

    [SerializeField] 
    [Tooltip("How far the forceps need to be opened before an object is released")]
    private float releaseThreshold = 0.8f;

    [SerializeField]
    private float graspThreshold = 0.2f;

    [SerializeField] 
    [Tooltip("Sets how far apart the forceps tips are when fully opened. Use to calibrate the two forceps if one is opened wider than the other. This is needed because the forceps do not always open fully, but the other will, causing differences in how it is displayed in the simulator")]
    private float maxOpenAngle = 7f;
    
    private readonly HashSet<GameObject> _graspedObjects = new HashSet<GameObject>();
    
    // Stores the nodes that the left blade is touching
    private readonly HashSet<GameObject> _leftBladeObjects = new HashSet<GameObject>();
    
    // Stores the nodes that the right blade is touching
    private readonly HashSet<GameObject> _rightBladeObjects = new HashSet<GameObject>();

    private Quaternion _startingRotation;

    private const int MaxHeldNodes = 1;

    /// <summary>
    /// Returns true if the forceps are currently grasping an object.
    /// </summary>
    public bool IsGraspingObject => _graspedObjects.Count > 0;

    private float _percentClosed;

    private float _initialStickyValue = 0.2f;
    private float _initialStaticFriction = 1f;
    private float _initialDynamicFriction = 1f;
    private float _initialStickDistance = 0.03f;
    
    public enum Blade
    {
        Left = 0,
        Right = 1,
    }

    /// <summary>
    /// Unity Event function called when the script is first initialized.
    /// </summary>
    private void Start()
    {
        _startingRotation = transform.rotation;
        
        _initialStickyValue = stickyMaterial.stickiness;
        _initialStaticFriction = stickyMaterial.staticFriction;
        _initialDynamicFriction = stickyMaterial.dynamicFriction;
        _initialStickDistance = stickyMaterial.stickDistance;
    }

    /// <summary>
    /// Unity Event function called once per frame.
    /// </summary>
    private void Update()
    {
        if (GameManagerScript.IsUsingForceps && !TrackStar.IsCalibrated) 
            return;

        if (GameManagerScript.IsUsingForceps)
            _percentClosed = TrackStar.GetPercentClosed(forcepIndex);

        if (_percentClosed >= 85) {
            const float TOLERANCE = 0.01f;
            if (Math.Abs(stickyMaterial.stickiness - _initialStickyValue) > TOLERANCE) {
                stickyMaterial.stickiness = _initialStickyValue;
                stickyMaterial.staticFriction = _initialStaticFriction;
                stickyMaterial.dynamicFriction = _initialDynamicFriction;
                stickyMaterial.stickDistance = _initialStickDistance;
                stickyMaterial.UpdateMaterial();
            }
        } else {
            if (stickyMaterial.stickiness != 0) {
                stickyMaterial.stickiness = 0;
                stickyMaterial.staticFriction = 0;
                stickyMaterial.dynamicFriction = 0;
                stickyMaterial.stickDistance = 0;
                stickyMaterial.UpdateMaterial();
            }
        }
        
        float forcepBladeAngle = CalculateForcepBladeAngle(_percentClosed);
        
        if (_graspedObjects.Count > 0)
        {
            if (forcepBladeAngle > releaseThreshold)
            {
                foreach (var graspedObject in _graspedObjects)
                {
                    ReleaseObject(graspedObject);
                }
                _graspedObjects.Clear();
                _leftBladeObjects.Clear();
                _rightBladeObjects.Clear();
            }
        }
        
        // TODO: Update metrics
        SetForcepBladeAngle(forcepBladeAngle);

        if (GameManagerScript.IsUsingForceps)
        {
            TrackStar.UpdateLocation();
            //GetComponent<Rigidbody>().MovePosition(TrackStar.GetAveragePosition(forcepIndex));
            //GetComponent<Rigidbody>().MoveRotation(TrackStar.GetAverageRotation(forcepIndex) * _startingRotation);
            /*foreach (Transform obj in transform)
            {
                obj.GetComponent<Rigidbody>().MovePosition(TrackStar.GetAveragePosition(forcepIndex));
                obj.GetComponent<Rigidbody>().MoveRotation(TrackStar.GetAverageRotation(forcepIndex).normalized);
            }*/
            transform.position = TrackStar.GetAveragePosition(forcepIndex);
            transform.rotation = TrackStar.GetAverageRotation(forcepIndex) * _startingRotation;
        }
    }

    private void OnApplicationQuit() {
        stickyMaterial.stickiness = _initialStickyValue;
        stickyMaterial.staticFriction = _initialStaticFriction;
        stickyMaterial.dynamicFriction = _initialDynamicFriction;
        stickyMaterial.stickDistance = _initialStickDistance;
    }

    /// <summary>
    /// Releases the object from the forceps.
    /// </summary>
    /// <param name="otherGameObject"> The object to be released. </param>
    private void ReleaseObject(GameObject otherGameObject)
    {
        if (otherGameObject == null || !otherGameObject.TryGetComponent(out Rigidbody rigidbody)) 
            return;
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;

        if (otherGameObject.TryGetComponent(out NodeScript nodeScript))
        {
            nodeScript.CapsuleCollider.enabled = true;

            NodeScript next = nodeScript.Next;
            NodeScript prev = nodeScript.Previous;

            /*if (next != null)
            {
                next.CapsuleCollider.enabled = true;
            }
            if (prev != null)
            {
                prev.CapsuleCollider.enabled = true;
            }*/

            nodeScript.ResetParent();

            foreach (GameObject obj in GetAllTipColliders())
            {
                if (next != null)
                {
                    Physics.IgnoreCollision(obj.GetComponent<Collider>(), next.GetComponent<Collider>(), false);
                    if (next.Next != null)
                        Physics.IgnoreCollision(obj.GetComponent<Collider>(), next.Next.GetComponent<Collider>(), false);
                }
                if (prev != null)
                {
                    Physics.IgnoreCollision(obj.GetComponent<Collider>(), prev.GetComponent<Collider>(), false);
                    if (prev.Previous != null)
                        Physics.IgnoreCollision(obj.GetComponent<Collider>(), prev.Previous.GetComponent<Collider>(), false);
                }
            }
        }
    }

    /// <summary>
    /// Grasps the object with the forceps.
    /// </summary>
    /// <param name="otherGameObject"> The object to be grasped. </param>
    private void GraspObject(GameObject otherGameObject)
    {
        Rigidbody rigidbody = otherGameObject.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;

        otherGameObject.transform.parent = transform;

        _graspedObjects.Add(otherGameObject);

        if (otherGameObject.TryGetComponent(out NodeScript nodeScript))
        {
            NodeScript next = nodeScript.Next;
            NodeScript prev = nodeScript.Previous;

            nodeScript.CapsuleCollider.enabled = false;

            /*if (next != null)
            {
                next.CapsuleCollider.enabled = false;
            }
            if (prev != null)
            {
                prev.CapsuleCollider.enabled = false;
            }*/

            /*Debug.Log("disabling collisions on node " + prev.gameObject.name.ToString());
            Debug.Log("disabling collisions on node " + next.gameObject.name.ToString());*/

            foreach (GameObject obj in GetAllTipColliders())
            {
                if (next != null)
                {
                    Physics.IgnoreCollision(obj.GetComponent<Collider>(), next.GetComponent<Collider>());
                    Physics.IgnoreCollision(obj.GetComponent<Collider>(), next.Next.GetComponent<Collider>());
                }
                if (prev != null)
                {
                    Physics.IgnoreCollision(obj.GetComponent<Collider>(), prev.GetComponent<Collider>());
                    Physics.IgnoreCollision(obj.GetComponent<Collider>(), prev.Previous.GetComponent<Collider>());
                }
            }
        }
    }

    /// <summary>
    /// Handles forceps collision with a node. Determines if the node should be grasped or not.
    /// </summary>
    /// <param name="node"> The node that the forceps collided with. </param>
    /// <param name="bladeIndex"> The index of the blade that collided with the node. Indicates if it was the left or right blade. </param>
    public void CollideWithNode(GameObject node, int bladeIndex)
    {
        switch (bladeIndex)
        {
            case (int)Blade.Left:
            {
                _leftBladeObjects.Add(node);
                if (_rightBladeObjects.Contains(node))
                {
                    if (_graspedObjects.Count >= MaxHeldNodes || maxOpenAngle - GetForcepBladeAngle() < graspThreshold)
                        return;
                    GraspObject(node);
                }

                break;
            }
            case (int)Blade.Right:
            {
                _rightBladeObjects.Add(node);
                if (_leftBladeObjects.Contains(node))
                {
                    if (_graspedObjects.Count >= MaxHeldNodes || maxOpenAngle - GetForcepBladeAngle() < graspThreshold)
                        return;
                    GraspObject(node);
                }

                break;
            }
        }
    }

    /// <summary>
    /// Handles forceps exiting collision with a node. Determines if the node should be released or not.
    /// </summary>
    /// <param name="node"> The node that the forceps collided with. </param>
    /// <param name="bladeIndex"> The index of the blade that collided with the node. Indicates if it was the left or right blade. </param>
    public void ExitCollisionWithNode(GameObject node, int bladeIndex)
    {
        switch (bladeIndex)
        {
            case (int)Blade.Left:
                _leftBladeObjects.Remove(node);
                break;
            case (int)Blade.Right:
                _rightBladeObjects.Remove(node);
                break;
        }

        bool nodeBeenReleased = _graspedObjects.Remove(node);

        if (nodeBeenReleased)
        {
            ReleaseObject(node);
        }
    }

    /// <summary>
    /// Calculates the angle of the forcep blades based on the percent closed.
    /// </summary>
    /// <param name="percentClosed"> The percent closed of the forceps. </param>
    /// <returns> The angle of the forcep blades. </returns>
    private float CalculateForcepBladeAngle(float percentClosed)
    {
        return Mathf.Max(maxOpenAngle - (maxOpenAngle * percentClosed / 100), 0.0f);
    }
    
    /// <summary>
    /// Returns the angle of the forcep blades.
    /// </summary>
    /// <returns> The angle of the forcep blades. </returns>
    private float GetForcepBladeAngle()
    {
        return CalculateForcepBladeAngle(rightBlade.transform.localRotation.z);
    }
    
    /// <summary>
    /// Sets the angle of the forcep blades directly by modifying the local rotation of the forcep blades.
    /// </summary>
    /// <param name="forcepBladeAngle"> The angle to set the forcep blades to. </param>
    public void SetForcepBladeAngle(float forcepBladeAngle)
    {
        leftBlade.transform.localRotation = Quaternion.Euler(0f, 0f, -forcepBladeAngle);
        rightBlade.transform.localRotation = Quaternion.Euler(0f, 0f, forcepBladeAngle);
    }

    // ReSharper disable once UnusedMember.Local
    private List<GameObject> GetInsideTipColliders(int index)
    {
        return index == (int)Blade.Left ? leftForcepsColliderScript.GetAllTipColliders() : rightForcepsColliderScript.GetAllTipColliders();
    }

    private List<GameObject> GetAllTipColliders()
    {
        var combinedList = GetInsideTipColliders((int)Blade.Left);
        combinedList.AddRange(GetInsideTipColliders((int)Blade.Right));
        return combinedList;
    }

    /// <summary>
    /// Sets the percent closed of the forceps. Needed for controller support instead of forceps.
    /// </summary>
    /// <param name="newPercentClose"> The new percent closed of the forceps. </param>
    public void SetPercentClosed(float newPercentClose)
    {
        _percentClosed = newPercentClose;
    }
    
    /// <summary>
    /// Returns the forceps index of the forceps. Used to differentiate between the two forceps.
    /// </summary>
    /// <returns> The forceps index. </returns>
    public int GetForcepsIndex()
    {
        return forcepIndex;
    }

}
