using System.Collections;
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

    private Dictionary<string, GameObject> _objectsToSpawnMap = new Dictionary<string, GameObject>();

    private void Start()
    {
        MapNamesToObjects();
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
            // Handle added event
        }

        foreach (var updatedImage in eventArgs.updated)
        {
            // Handle updated event
        }

        foreach (var removedImage in eventArgs.removed)
        {
            // Handle removed event
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ListAllImages();
        }
    }

    void ListAllImages()
    {
        Debug.Log($"There are {_trackedImageManager.trackables.count} images being tracked.");

        foreach (var trackedImage in _trackedImageManager.trackables)
        {
            Debug.Log($"Name of tracked image: {trackedImage.name}");
            Debug.Log($"Image: {trackedImage.referenceImage.name} is at " +
                      $"{trackedImage.transform.position}");
            var instantiatedObj = Instantiate(_objectsToSpawnMap[trackedImage.referenceImage.name], trackedImage.transform);
            instantiatedObj.transform.localPosition = Vector3.zero;
        }
    }
}
