using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PrefabCreator : MonoBehaviour
{
    Dictionary<string, GameObject> trackedImagesPrefabsDict = new Dictionary<string, GameObject>();

    [SerializeField] private string image1Name;
    [SerializeField] private string image2Name;

    [SerializeField] private GameObject image1Prefab;
    [SerializeField] private GameObject image2Prefab;

    private ARTrackedImageManager aRTrackedImgManager;
    private GameObject spawnedPrefab;

    private void OnEnable()
    {
        aRTrackedImgManager = GetComponent<ARTrackedImageManager>();

        aRTrackedImgManager.trackedImagesChanged += OnImageChanged;
    }

    private void Awake()
    {
        SetupTrackedImagesPrefabsDict();
    }

    private void OnImageChanged(ARTrackedImagesChangedEventArgs obj)
    {
        foreach(ARTrackedImage image in obj.added)
        {
            string imageName = image.referenceImage.name;
            spawnedPrefab = Instantiate(trackedImagesPrefabsDict[imageName], image.transform);
        }
    }

    private void SetupTrackedImagesPrefabsDict()
    {
        trackedImagesPrefabsDict.Add(image1Name, image1Prefab);
        trackedImagesPrefabsDict.Add(image2Name, image2Prefab);
    }
}
