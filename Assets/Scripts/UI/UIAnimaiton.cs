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

	private float _moveUp;
	private float _moveDown;

    void Start()
    {
        _initialPosition = transform.position;
		_moveUp = Screen.height * 0.03f;
		_moveDown = Screen.height * 0.01f;
    }

    void Update()
    {
		if(!_doesJump && Inventory.Instance.NewObjectsInBox > 0) {
			if(_jumpCoroutine != null) return;

			_jumpCoroutine = StartCoroutine(StartJump());
		}
    }

	void OnDisable() {
		if(_jumpCoroutine != null) {
			StopCoroutine(_jumpCoroutine);
			_jumpCoroutine = null;
		}
	}

	void FixedUpdate() {
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
			transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, _initialPosition.y + _moveUp), 0.2f);
			if(Mathf.Abs(transform.position.y - (_initialPosition.y + _moveUp)) <= 0.2f) {
				_isGoingUp = false;
				_isGoingDown = true;
			}
		}
		else {
			transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, _initialPosition.y - _moveDown), 0.2f);
			if(Mathf.Abs(transform.position.y - (_initialPosition.y - _moveDown)) <= 0.2f) {
				_isGoingDown = false;
				_doneJump = true;
			}
		}
	}

	private void GoBackToInitialPosition() {
		if(Mathf.Abs(transform.position.y - _initialPosition.y) <= 0.2f) {
			_doesJump = false;
			return;
		} 
		transform.position = Vector2.Lerp(transform.position, _initialPosition, 0.2f);
	}

	private void ResetJump() {
		transform.position = _initialPosition;
		_isGoingUp = false;
		_isGoingDown = false;
		_doneJump = false;
		_doesJump = true;
	}
}
