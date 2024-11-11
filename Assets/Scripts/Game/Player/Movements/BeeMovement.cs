using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BeeMovement : MonoBehaviour
{
    [SerializeField] private protected float _moveSpeed;
    [SerializeField] private protected float _rotationSpeed = 1f;
}