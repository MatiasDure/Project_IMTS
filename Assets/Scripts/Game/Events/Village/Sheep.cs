using System.Collections;
using UnityEngine;

[
	RequireComponent(typeof(PlayAnimation))
]
public class Sheep : MonoBehaviour
{
	[SerializeField] private string _jumpAnimationTriggerName;
	[SerializeField] private string _jumpAnimationStateName;
	private PlayAnimation _playAnimation;

	private void Awake() {
		_playAnimation = GetComponent<PlayAnimation>();
	}

    public void Jump(float jumpForce,float jumpDuration)
    {
        StartCoroutine(JumpEnumerator(jumpForce, jumpDuration));
		// StartCoroutine(JumpAnimation());
    }

	private IEnumerator JumpAnimation() {
		_playAnimation.SetTrigger(_jumpAnimationTriggerName);
		yield return StartCoroutine(_playAnimation.WaitForAnimationToStart(_jumpAnimationStateName));
		yield return StartCoroutine(_playAnimation.WaitForAnimationToEnd());
	}

    private IEnumerator JumpEnumerator(float jumpForce, float jumpDuration)
    {
        Vector3 startPosition = transform.position;
        Vector3 peakPosition = startPosition + Vector3.up * jumpForce;

        float elapsedTime = 0f;
        
        while (elapsedTime < jumpDuration / 2f)
        {
            transform.position = Vector3.Lerp(startPosition, peakPosition, (elapsedTime / (jumpDuration / 2f)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = peakPosition;

        elapsedTime = 0f;
        
        while (elapsedTime < jumpDuration / 2f)
        {
            transform.position = Vector3.Lerp(peakPosition, startPosition, (elapsedTime / (jumpDuration / 2f)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        transform.position = startPosition;
    }
    
}
