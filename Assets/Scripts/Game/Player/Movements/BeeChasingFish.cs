using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BeeSwimming))]
public class BeeChasingFish : BeeMovement
{
    private bool _isChasing = false;
    private BeeSwimming swimmingScript;

    private void Awake()
    {
        swimmingScript = GetComponent<BeeSwimming>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (_isChasing) Chase();
    }

    private void BeeStateChanged(BeeState state)
    {
        if (state != BeeState.ChasingFish) return;
    }

    private void Chase()
    {
        //swimmingScript.SmoothRotateBeeQuaternion(swimmingScript.GetRotationToPoint());
    }
}
