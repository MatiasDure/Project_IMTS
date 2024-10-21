using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;

public class ImageTrackingResponseTest
{
    [UnityTest]
	public IEnumerator PlayMode_ImageTrackingResponseTest_Activate()
	{
		GameObject objectToAcivate = new GameObject();
		objectToAcivate.SetActive(false);

		GameObject trackedImageGameObject = new GameObject();

		ImageTrackingActivateResponse imageTrackingActivateResponse = new GameObject().AddComponent<ImageTrackingActivateResponse>();
		imageTrackingActivateResponse.Respond(objectToAcivate, trackedImageGameObject);
		
		yield return null;

		Assert.IsTrue(objectToAcivate.activeSelf);
	}

	[UnityTest]
	public IEnumerator PlayMode_ImageTrackingResponseTest_Spawn()
	{
		GameObject objectToSpawn = new GameObject("ObjectToSpawn");
		GameObject trackedImageGameObject = new GameObject();

		ImageTrackingSpawnResponse imageTrackingSpawnResponse = new GameObject().AddComponent<ImageTrackingSpawnResponse>();
		imageTrackingSpawnResponse.Respond(objectToSpawn, trackedImageGameObject);

		yield return null;

		string spawnedObjectName = objectToSpawn.name + "(Clone)";
		Assert.IsNotNull(GameObject.Find(spawnedObjectName));
	}

	[UnityTest]
	public IEnumerator PlayMode_ImageTrackingResponseTest_SyncWithImage()
	{
		GameObject objectToSpawn = new GameObject("ObjectToSpawn");
		GameObject trackedImageGameObject = new GameObject();
		trackedImageGameObject.transform.position = new Vector3(1, 2, 3);
		trackedImageGameObject.transform.rotation = Quaternion.Euler(10, 20, 30);

		ImageTrackingSyncWithImageResponse imageTrackingSyncWithImageResponse = new GameObject().AddComponent<ImageTrackingSyncWithImageResponse>();
		GameObject spawnedObject = imageTrackingSyncWithImageResponse.Respond(objectToSpawn, trackedImageGameObject);

		yield return null;

		Assert.AreEqual(trackedImageGameObject.transform.position, spawnedObject.transform.position);
		Assert.IsTrue(MathHelper.AreQuaternionsApproximatelyEqual(trackedImageGameObject.transform.rotation, spawnedObject.transform.rotation));
	}

	[UnityTest]
	public IEnumerator PlayMode_ImageTrackingActivateResponseTest_Manager()
	{
		ImageTrackingResponseManager imageTrackingResponseManager = new GameObject().AddComponent<ImageTrackingResponseManager>();

		GameObject trackedImageGameObject = new GameObject("TrackedImage");
		trackedImageGameObject.transform.position = new Vector3(1, 2, 3);

		// mocking the ImageTrackingResponseManager with ImageTrackingSpawnResponse and ImageTrackingActivateResponse
		List<IImageTrackingResponse> imageTrackingResponses = new List<IImageTrackingResponse>
		{
			new GameObject().AddComponent<ImageTrackingSpawnResponse>(),
			new GameObject().AddComponent<ImageTrackingActivateResponse>()
		};
		imageTrackingResponseManager._imageTrackingResponses = imageTrackingResponses;

		// mocking the ImageObjectReference with SpawnObject response
		GameObject objectToSpawn = new GameObject("ObjectToSpawn");

		ImageObjectReference imageObjectReferenceSpawn = new ImageObjectReference();
		imageObjectReferenceSpawn._imageName = "TrackedImage";
		imageObjectReferenceSpawn._addedResponse = ImageTrackingResponses.SpawnObject;
		imageObjectReferenceSpawn._objectReference = objectToSpawn;
		
		imageTrackingResponseManager.HandleTrackedImageAddedResponse(trackedImageGameObject, imageObjectReferenceSpawn);
		
		yield return null;

		string spawnedObjectName = objectToSpawn.name + "(Clone)";
		GameObject spawnedObject = GameObject.Find(spawnedObjectName);
		Assert.IsNotNull(spawnedObject);
		Assert.AreEqual(trackedImageGameObject.transform.position, spawnedObject.transform.position);

		// mocking the ImageObjectReference with ActivateObject response
		GameObject objectToActivate = new GameObject("ObjectToActivate");
		objectToActivate.SetActive(false);

		ImageObjectReference imageObjectReferenceActivate = new ImageObjectReference();
		imageObjectReferenceActivate._imageName = "TrackedImage";
		imageObjectReferenceActivate._addedResponse = ImageTrackingResponses.ActivateObject;
		imageObjectReferenceActivate._objectReference = objectToActivate;

		imageTrackingResponseManager.HandleTrackedImageAddedResponse(trackedImageGameObject, imageObjectReferenceActivate);

		yield return null;

		Assert.IsTrue(objectToSpawn.activeSelf);
		Assert.AreEqual(trackedImageGameObject.transform.position, objectToActivate.transform.position);
	}
}
