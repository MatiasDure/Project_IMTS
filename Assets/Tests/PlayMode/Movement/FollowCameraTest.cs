using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;

public class FollowCameraTest
{
	// Note: If this test fails, the mocked time might be too short. Try increasing the simulatedTime variable.
    [UnityTest]
	public IEnumerator PlayMode_FollowCameraTest_Follow()
	{
		FollowConfiguration followConfiguration = new FollowConfiguration();
		followConfiguration._target = new GameObject().transform;
		followConfiguration._speed = 1.5f;
		followConfiguration._offset = new Vector3(0, 0, 0);
		followConfiguration._lookAtTarget = false;
		followConfiguration._distance = 1;

		FollowCamera followCamera = new GameObject().AddComponent<FollowCamera>();
		followCamera._followConfiguration = followConfiguration;

		followConfiguration.Target.position = new Vector3(3, 0, 4);

		// Mock passage of time by manually calling LateUpdate
        float simulatedTime = 10f;
        float fpsToMS = 1f / 60f; // 60 frames per second to ms
        int steps = Mathf.CeilToInt(simulatedTime / fpsToMS);

        for (int i = 0; i < steps; i++) // Around 600 frames
        {
            followCamera.LateUpdate();
            yield return null; // Wait for the next frame
        }
		
        Vector3 expectedPosition = followConfiguration.Target.position + followConfiguration.Target.forward * followConfiguration.Distance + followConfiguration.Offset;
        Assert.IsTrue(IsApproximatelyEqual(followCamera.transform.position, expectedPosition));
	} 

	private bool IsApproximatelyEqual(Vector3 a, Vector3 b, float tolerance = 0.3f)
	{
		return Mathf.Abs(a.x - b.x) < tolerance && Mathf.Abs(a.y - b.y) < tolerance && Mathf.Abs(a.z - b.z) < tolerance;
	}
}
