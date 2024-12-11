using UnityEngine;

[
    RequireComponent(typeof(Rigidbody)),
    RequireComponent(typeof(Collider)),
]
public class Sheep : MonoBehaviour
{
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
    }

    public void Jump(float jumpForce)
    {
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
    
}
