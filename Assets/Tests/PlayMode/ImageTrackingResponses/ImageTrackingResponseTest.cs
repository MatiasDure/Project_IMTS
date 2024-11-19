using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.XR.ARFoundation;

public class ImageTrackingResponseTest
{
    [UnityTest]
	public IEnumerator PlayMode_ImageTrackingResponseTest_Activate()
	{
		GameObject objectToAcivate = new GameObject();
		objectToAcivate.SetActive(false);

		ARTrackedImage trackedImage = new GameObject().AddComponent<ARTrackedImage>();

		ImageTrackingActivateResponse imageTrackingActivateResponse = new GameObject().AddComponent<ImageTrackingActivateResponse>();
		imageTrackingActivateResponse.Respond(objectToAcivate, trackedImage);
		
		yield return null;

		Assert.IsTrue(objectToAcivate.activeSelf);
	}

	[UnityTest]
	public IEnumerator PlayMode_ImageTrackingResponseTest_Spawn()
	{
		GameObject objectToSpawn = new GameObject("ObjectToSpawn");
		ARTrackedImage trackedImage = new GameObject().AddComponent<ARTrackedImage>();

		ImageTrackingSpawnResponse imageTrackingSpawnResponse = new GameObject().AddComponent<ImageTrackingSpawnResponse>();
		imageTrackingSpawnResponse.Respond(objectToSpawn, trackedImage);

		yield return null;

		string spawnedObjectName = objectToSpawn.name + "(Clone)";
		Assert.IsNotNull(GameObject.Find(spawnedObjectName));
	}

	[UnityTest]
	public IEnumerator PlayMode_ImageTrackingResponseTest_SyncWithImage()
	{
		GameObject objectToSpawn = new GameObject("ObjectToSpawn");
		ARTrackedImage trackedImage = new GameObject().AddComponent<ARTrackedImage>();
		trackedImage.transform.position = new Vector3(1, 2, 3);
		trackedImage.transform.rotation = Quaternion.Euler(10, 20, 30);

		ImageTrackingSyncWithImageResponse imageTrackingSyncWithImageResponse = new GameObject().AddComponent<ImageTrackingSyncWithImageResponse>();
		GameObject spawnedObject = imageTrackingSyncWithImageResponse.Respond(objectToSpawn, trackedImage);

		yield return null;

		Assert.AreEqual(trackedImage.transform.position, spawnedObject.transform.position);
		Assert.AreEqual(trackedImage.transform.rotation, spawnedObject.transform.rotation);
	}

	[UnityTest]
	public IEnumerator PlayMode_ImageTrackingActivateResponseTest_Manager()
	{
		ImageTrackingResponseManager imageTrackingResponseManager = new GameObject().AddComponent<ImageTrackingResponseManager>();

		ARTrackedImage trackedImage = new GameObject("TrackedImage").AddComponent<ARTrackedImage>();
		trackedImage.transform.position = new Vector3(1, 2, 3);

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
		
		imageTrackingResponseManager.HandleTrackedImageAddedResponse(trackedImage, imageObjectReferenceSpawn);
		
		yield return null;

		string spawnedObjectName = objectToSpawn.name + "(Clone)";
		GameObject spawnedObject = GameObject.Find(spawnedObjectName);
		Assert.IsNotNull(spawnedObject);
		Assert.AreEqual(trackedImage.transform.position, spawnedObject.transform.position);

		// mocking the ImageObjectReference with ActivateObject response
		GameObject objectToActivate = new GameObject("ObjectToActivate");
		objectToActivate.SetActive(false);

		ImageObjectReference imageObjectReferenceActivate = new ImageObjectReference();
		imageObjectReferenceActivate._imageName = "TrackedImage";
		imageObjectReferenceActivate._addedResponse = ImageTrackingResponses.ActivateObject;
		imageObjectReferenceActivate._objectReference = objectToActivate;

		imageTrackingResponseManager.HandleTrackedImageAddedResponse(trackedImage, imageObjectReferenceActivate);

		yield return null;

		Assert.IsTrue(objectToSpawn.activeSelf);
		Assert.AreEqual(trackedImage.transform.position, objectToActivate.transform.position);
	}
}
