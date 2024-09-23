using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private float scaleSpawnedObjectAmount;
    [SerializeField] private List<ImagePrefab> imagePrefabs;

    private ARTrackedImageManager trackedImageManager;
    private ARPlaneManager planeManager;

    private GameObject spawnedObj;

    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        planeManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnImageChanged;

    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (ARTrackedImage image in obj.added)
        {
            SpawnObjectOnTrackedImage(image);
            SpawnPlaneOnTrackedImage(image);
        }
    }

    private void SpawnObjectOnTrackedImage(ARTrackedImage trackedImg)
    {
        // Get ImagePrefab based on the scanned image's name
        GameObject objToSpawn = null;
        ImagePrefab imgPrefab = null;
        string imageName = trackedImg.referenceImage.name;
        for (int i = 0; i < imagePrefabs.Count; i++)
            if (imageName == imagePrefabs[i].ImgName)
            {
                imgPrefab = imagePrefabs[i];
                break;
            }

        // Assign object to spawn
        objToSpawn = imgPrefab.Prefab;

        if (objToSpawn == null)
        {
            throw new System.Exception($"ImageTracked: No prefab assigned for this image - {imageName}");
        }

        // Scale object amount
        //TEMP
        Vector3 objSize = trackedImg.size;
        objSize *= scaleSpawnedObjectAmount;
        float newValue = objSize.x;

        // Change dimensions based on ImgPlacement (Wall / Floor)
        objSize = imgPrefab.ImgPlacement == ImagePrefab.ImagePlacement.Wall ?
                                      new Vector3(newValue, newValue, 0.1f) :
                                      new Vector3(newValue, 1, newValue);

        // Spawn object
        spawnedObj = Instantiate(objToSpawn, trackedImg.transform);
        spawnedObj.transform.localScale = objSize;
    }

    private void SpawnPlaneOnTrackedImage(ARTrackedImage trackedImg)
    {
        GameObject newArPlane = new GameObject("ARPlane");
        ARPlane planeComponent = newArPlane.AddComponent<ARPlane>();
        planeComponent.transform.position = trackedImg.transform.position;
        planeComponent.transform.rotation = trackedImg.transform.rotation;
        planeComponent.transform.localScale = new Vector3(trackedImg.size.x, 1f, trackedImg.size.y);
    }
}