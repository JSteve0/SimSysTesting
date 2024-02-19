using UnityEngine;
using UnityEngine.Serialization;

public class HoleScriptEnhanced : MonoBehaviour
{
    [SerializeField] private bool isEntrance;
    [SerializeField] private ForcepScript leftForcep;
    [SerializeField] private ForcepScript rightForcep;
    
    private void OnTriggerEnter(Collider other)
    {
        if (!isEntrance || (!leftForcep.IsGraspingObject && !rightForcep.IsGraspingObject))
            return;
        
        if (other.gameObject.CompareTag("Suture") && other.gameObject.TryGetComponent(out NodeScript nodeScript))
        {
            nodeScript.CapsuleCollider.enabled = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (isEntrance)
            return;
        
        if (other.gameObject.CompareTag("Suture") && other.gameObject.TryGetComponent(out NodeScript nodeScript))
        {
            nodeScript.CapsuleCollider.enabled = true;
        }
    }
}
