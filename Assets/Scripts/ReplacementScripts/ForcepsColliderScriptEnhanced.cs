using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

/// <summary>
/// Handles the collision of the forceps with the suture nodes.
/// Attached to each blade of the forceps.
/// </summary>
public class ForcepsColliderScriptEnhanced : MonoBehaviour {

	[SerializeField] private List<GameObject> insideTipColliders;
    [SerializeField] private List<GameObject> tipColliders;
    [SerializeField] private ForcepScript forcep;
	[SerializeField] private Blade blade;

	private string tipColliderName = "TipCollider";

	private enum Blade
	{
		[UsedImplicitly] Left = 0,
		[UsedImplicitly] Right = 1,
	}

    private void Start()
    {
        foreach (Transform t in transform.GetChild(1)) { 
			if (t.gameObject.name.StartsWith(tipColliderName) && !tipColliders.Contains(t.gameObject))
			{
				tipColliders.Add(t.gameObject);
			}
		}
    }

    // when the forceps come into contact with another rigidbody
    // this function begins
    private void OnCollisionEnter(Collision collision) 
	{
		if (!collision.gameObject.CompareTag("Suture"))
			return;
		
		foreach (ContactPoint contact in collision.contacts)
		{
			// Check if the collision was with an inside tip colliders
			if (insideTipColliders.Any(t => contact.thisCollider.gameObject == t))
			{
				forcep.CollideWithNode(contact.otherCollider.gameObject, (int)blade);
			}
		}
	}

	public List <GameObject> GetAllTipColliders() 
	{
		return tipColliders;		
	}

}
