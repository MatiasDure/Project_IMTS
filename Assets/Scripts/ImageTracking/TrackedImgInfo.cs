using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class TrackedImgInfo : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager _trackedImageManager;

    [SerializeField]
    private List<string> _imageNames;

    [SerializeField]
    private List<GameObject> _objectsToSpawn;

    // Create a class with these properties and pass that instead of having two lists and a dictionary
    private Dictionary<string, GameObject> _objectsToSpawnMap = new Dictionary<string, GameObject>();

	// private Dictionary<string, float> _cooldownMap = new Dictionary<string, float>();
	// private float _cooldownTime = 5f;
	private List<string> _spawnedObjectsToInteractWith = new List<string>();

    private void Start()
    {
        MapNamesToObjects();
		CreatureInteractable.OnSpawnableInteracted += MarkAsInteracted;
    }

    private void MapNamesToObjects()
    {
        for (int i = 0; i < _imageNames.Count; i++)
        {
            _objectsToSpawnMap.Add(_imageNames[i], _objectsToSpawn[i]);
        }
    }

    void OnEnable() => _trackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => _trackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var newImage in eventArgs.added)
        {
            InstantiateSpawnableObject(newImage);

			// _cooldownMap.Add(newImage.referenceImage.name, _cooldownTime);
            // Handle added event
        }

        // This is where tracked images are looping
        // foreach (var updatedImage in eventArgs.updated)
        // {
        //     // Handle updated event
		// 	// Create a method to cooldown here
		// 	if(_cooldownMap.ContainsKey(updatedImage.referenceImage.name)) return;

		// 	_cooldownMap.Add(updatedImage.referenceImage.name, _cooldownTime);
        //     InstantiateSpawnableObject(updatedImage);
        // }

        foreach (var removedImage in eventArgs.removed)
        {
            // Handle removed event
            Debug.Log("here3");
        }
    }

	private void InstantiateSpawnableObject(ARTrackedImage newImage)
    {
		// Vector3 offset = newImage.transform.up * 0.15f;
        var instantiatedObj = Instantiate(_objectsToSpawnMap[newImage.referenceImage.name]);
		_spawnedObjectsToInteractWith.Add(newImage.referenceImage.name);
        instantiatedObj.transform.SetPositionAndRotation(newImage.transform.position, newImage.transform.rotation);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ListAllImages();
        }

		// Cooldown();
	}

    // private void Cooldown()
    // {
    //     List<string> keysToRemove = new List<string>();

    //     foreach (var image in new List<string>(_cooldownMap.Keys))
    //     {
	// 		// object has spawned but was not interacted with yet (Find a better way to handle this)
	// 		if(_spawnedObjectsToInteractWith.Contains(image)) return;

    //         _cooldownMap[image] -= Time.deltaTime;

    //         if(_cooldownMap[image] <= 0) 
	// 			_cooldownMap.Remove(image);
    //     }
    // }

    void ListAllImages()
    {
        Debug.Log($"There are {_trackedImageManager.trackables.count} images being tracked.");

        foreach (var trackedImage in _trackedImageManager.trackables)
        {
            Debug.Log($"Image: {trackedImage.referenceImage.name} is at " +
                      $"{trackedImage.transform.position}");
            
        }
    }

	void MarkAsInteracted(string imageName)
	{
		_spawnedObjectsToInteractWith.Remove(imageName);
	}

	private void OnDestroy()
	{
		CreatureInteractable.OnSpawnableInteracted -= MarkAsInteracted;
	}
}
