using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SwimmingBehaviour))]
[RequireComponent(typeof(AvoidObjectSwimmingBehavior))]
public class Fish : MonoBehaviour
{
    public float MoveSpeed { get; set; } = 0.5f;

    private SwimmingBehaviour _swimmingBehaviour;
    private AvoidObjectSwimmingBehavior _avoidObjectSwimmingBehavior;

    private void Awake()
    {
        _avoidObjectSwimmingBehavior = GetComponent<AvoidObjectSwimmingBehavior>();
        _swimmingBehaviour = GetComponent<SwimmingBehaviour>();
    }

    private void Start()
    {
        _swimmingBehaviour.CheckBounds = true;
        BeginSwimming();
    }

    private void Update()
    {
        AvoidObjectMove();
    }

    private void BeginSwimming()
    {
        _swimmingBehaviour.DoSwimmingSequence();
    }

    private void Move()
    {
        _swimmingBehaviour.Move(MoveSpeed);
    }

    private void AvoidObjectMove()
    {
        _avoidObjectSwimmingBehavior.Move(MoveSpeed);
    }
}
