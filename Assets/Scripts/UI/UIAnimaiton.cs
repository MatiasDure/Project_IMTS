using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimaiton : MonoBehaviour
{
	private Vector2 _initialPosition;

	private bool _isGoingUp = false;
	private bool _isGoingDown = false;
	private bool _doneJump = false;
	private bool _doesJump = false;

	private Coroutine _jumpCoroutine;

    void Start()
    {
        _initialPosition = transform.position;

    }

    void Update()
    {
		if(!_doesJump && Inventory.Instance.NewObjectsInBox > 0) {
			if(_jumpCoroutine != null) return;

			_jumpCoroutine = StartCoroutine(StartJump());
		}

		JumpAnimation();
    }

	private IEnumerator StartJump() {
		ResetJump();
		yield return new WaitForSeconds(2f);
		_jumpCoroutine = null;
	}

	private void JumpAnimation() {
		if(!_doesJump) return;

		if(_doneJump) {
			GoBackToInitialPosition();
			return;
		}
		if(!_isGoingDown && !_isGoingUp) {
			_isGoingUp = true;
		}
		else if(_isGoingUp) {
			transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, _initialPosition.y + 15f), 0.05f);
			if(Mathf.Abs(transform.position.y - (_initialPosition.y + 15f)) <= 0.1f) {
				_isGoingUp = false;
				_isGoingDown = true;
			}
		}
		else {
			transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, _initialPosition.y - 5f), 0.05f);
			if(Mathf.Abs(transform.position.y - (_initialPosition.y - 5f)) <= 0.1f) {
				_isGoingDown = false;
				_doneJump = true;
			}
		}
	}

	private void GoBackToInitialPosition() {
		if(Mathf.Abs(transform.position.y - _initialPosition.y) <= 0.1f) {
			_doesJump = false;
			return;
		} 
		transform.position = Vector2.Lerp(transform.position, _initialPosition, 0.05f);
	}

	private void ResetJump() {
		transform.position = _initialPosition;
		_isGoingUp = false;
		_isGoingDown = false;
		_doneJump = false;
		_doesJump = true;
	}
}
