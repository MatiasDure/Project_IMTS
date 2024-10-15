using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
	private float speed = 5.0f;
	private float threshold = 3f;

	private Vector3 initialPos;
	private Vector3 targetPos;

	private Coroutine moveCoroutine;
	private bool isFrozen = false;

    // Start is called before the first frame update
    void Start()
    {
		initialPos = transform.position;
		targetPos = new Vector3(initialPos.x + threshold, initialPos.y, initialPos.z);
    }

    // Update is called once per frame
    void Update()
    {
		if(Vector3.Distance(transform.position, targetPos) < 0.1f) {
			targetPos = targetPos.x > initialPos.x ? new Vector3(initialPos.x - threshold, initialPos.y, initialPos.z) : new Vector3(initialPos.x + threshold, initialPos.y, initialPos.z);
		}

		if(isFrozen || moveCoroutine != null) return;

		moveCoroutine = StartCoroutine(MoveObject());
    }

	IEnumerator MoveObject()
	{
		// random movement
		transform.position = new Vector3(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f), transform.position.z + Random.Range(-1f, 1f));
		yield return new WaitForSeconds(.5f);
		moveCoroutine = null;

		// transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
	}

	public IEnumerator FreezeMovement(float duration)
	{
		StopCoroutine(moveCoroutine);
		moveCoroutine = null;
		isFrozen = true;
		yield return new WaitForSeconds(duration);
		isFrozen = false;
	}

	public void StartFreezeMovement(float duration)
	{
		StartCoroutine(FreezeMovement(duration));
	}
}
