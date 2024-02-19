using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForcepEnhanced : MonoBehaviour
{

    private enum ForcepType
    {
        Straight,
        Bent
    }

    //[SerializeField] private ForcepType currentForcepType = ForcepType.Straight;
    
    [SerializeField] private GameObject StraightForcep;
    [SerializeField] private GameObject BentForcep;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
