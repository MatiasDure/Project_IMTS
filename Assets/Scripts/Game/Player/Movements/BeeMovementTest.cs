using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.Serialization;

public class BeeMovementTest : MonoBehaviour
{
    [SerializeField] private Transform _secondPortal;
    [SerializeField] private Transform _portal;
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _firstCamera;
    private BeeMovement _beeMovement;

    private bool _allowMoving;
    private bool _moveIn = true;
    private void Start()
    {
        _beeMovement = GetComponent<BeeMovement>();
        SubscribeToEvents();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _allowMoving = true;
        }

        if (_allowMoving)
        {
            if (_moveIn)
            {
                _beeMovement.MoveTowardPositionThroughPortal(_portal.position,
                    _secondPortal.position,
                    _target.position,
                    Vector3.zero);
            }
            else
            {
                _beeMovement.MoveTowardPositionThroughPortal(_secondPortal.position,
                    _portal.position,
                    _firstCamera.position,
                    Vector3.zero);
            }
                
            
            
        }
    }

    private void SubscribeToEvents()
    {
        BeeMovement.BeeMovementEnd += HandleMovementEnd;
    }
    
    private void UnSubscribeToEvents()
    {
        BeeMovement.BeeMovementEnd -= HandleMovementEnd;
    }
    private void HandleMovementEnd()
    {
        _allowMoving = false;
        _moveIn = !_moveIn;
    }
    private void OnDestroy()
    {
        UnSubscribeToEvents();
    }
}
