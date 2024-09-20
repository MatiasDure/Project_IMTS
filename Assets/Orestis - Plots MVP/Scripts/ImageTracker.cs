using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ImageTracker : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private float scaleSpawnedObjectAmount;

    private GameObject spawnedObj;

    private ARTrackedImageManager trackedImageManager;

    //TEMP
    private ARTrackedImage scannedImg;
    private bool canScanImg = false;

    private void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnImageChanged;
    }

    private void Update()
    {
        // Manually spawn object on scanned image when pressing SPACE
        if (Input.GetKeyDown(KeyCode.Space) && canScanImg) SpawnObjectOnTrackedImage(scannedImg);
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (ARTrackedImage image in obj.added)
        {
            scannedImg = image;
            canScanImg = true;
            //SpawnObjectOnTrackedImage(image);
        }
    }

    private void SpawnObjectOnTrackedImage(ARTrackedImage trackedImage)
    {
        canScanImg = false;

        // Scale object amount
        Vector3 objSize = trackedImage.size;
        objSize *= scaleSpawnedObjectAmount;
        objSize.z = 0.1f;

        // Spawn object
        spawnedObj = Instantiate(prefabToSpawn, trackedImage.transform);
        spawnedObj.transform.localScale = objSize;
    }
}
