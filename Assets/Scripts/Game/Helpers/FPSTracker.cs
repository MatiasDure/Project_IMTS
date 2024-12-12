using UnityEngine;
using TMPro;
using System.Collections;

public class FPSTracker : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI _fpsText;
	[SerializeField] private float _updateInterval = 0.5f;

    // Update is called once per frame
    void Update()
    {
		StartCoroutine(UpdateFPS());	
    }

	private IEnumerator UpdateFPS()
	{
		while (true)
		{
			// Calculate FPS
			float fps = 1.0f / Time.deltaTime;
			_fpsText.text = $"FPS: {fps.ToString("0.00")}";	
			yield return new WaitForSeconds(_updateInterval);
		}
	}
}
