using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneFromImage : MonoBehaviour
{
    [SerializeField] private GameObject planePrefab;

    private ARTrackedImageManager aRTrackedImgManager;
    private GameObject spawnedPlane;

    private void OnEnable()
    {
        aRTrackedImgManager = GetComponent<ARTrackedImageManager>();

        aRTrackedImgManager.trackedImagesChanged += OnImageChanged;
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs obj)
    {
        foreach (ARTrackedImage image in obj.added)
        {
            Debug.Log("Spawned Plane");
            spawnedPlane = Instantiate(planePrefab, image.transform);
            spawnedPlane.transform.localScale = new Vector3(5, 5, 5);
        }
    }
}
