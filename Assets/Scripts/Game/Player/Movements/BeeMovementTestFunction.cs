using UnityEngine;

public class BeeMovementTestFunction : MonoBehaviour
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
                _beeMovement.MoveTowardPositionThroughPortal(_portal,
                                                                _secondPortal,
                                                                _target,
                                                                Vector3.zero);
            }
            else
            {
                _beeMovement.MoveTowardPositionThroughPortal(_secondPortal,
                                                                _portal,
                                                                _firstCamera,
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
