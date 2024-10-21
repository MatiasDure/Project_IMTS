using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BeeMovement : MonoBehaviour
{
    [SerializeField] private protected float _moveSpeed;
    [SerializeField] private protected float _rotationSpeed = 1f;
    [SerializeField] private protected float _minDesicionSec;
    [SerializeField] private protected float _maxDesicionSec;
    [Range(0f, 90f)]
    [SerializeField] private protected float _horizontalTurnLimit;
    [SerializeField] private protected float _verticalTurnLimit = 3;
    [SerializeField] private protected float _verticalRotationBound = 10;
}
