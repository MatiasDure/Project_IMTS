using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SwimmingBehaviour))]
public class Fish : MonoBehaviour
{
    public float MoveSpeed { get; set; } = 0.5f;

    private SwimmingBehaviour _swimmingBehaviour;

    private void Awake()
    {
        _swimmingBehaviour = GetComponent<SwimmingBehaviour>();
    }

    private void Start()
    {
        _swimmingBehaviour.CheckBounds = true;
        BeginSwimming();
    }

    private void Update()
    {
        Move();
    }

    private void BeginSwimming()
    {
        _swimmingBehaviour.DoSwimmingSequence();
    }

    private void Move()
    {
        _swimmingBehaviour.Move(MoveSpeed);
    }
}
