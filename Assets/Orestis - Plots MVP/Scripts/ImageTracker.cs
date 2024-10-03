using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

public class ImageTracker : MonoBehaviour
{
    public static ImageTracker Instance { get; private set; }

    public static event Action<ARTrackedImage, ImagePrefab.ImagePlacement> OnImageTracked;

    [SerializeField] private bool spawnPlanesFromImages;
    [SerializeField] private GameObject prefabToSpawn;
    [SerializeField] private float scaleSpawnedObjectAmount;
    [SerializeField] private List<ImagePrefab> imagePrefabs;

    private ARTrackedImageManager trackedImageManager;

    private GameObject spawnedObj;

    private void Awake()
    {
        // Singleton Pattern
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        trackedImageManager = GetComponent<ARTrackedImageManager>();
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
            ImagePrefab imgPrefab = GetImagePrefab(image);
            SpawnObjectOnTrackedImage(image, imgPrefab);

            // Call event if spawning planes from images is enabled
            if (spawnPlanesFromImages) OnImageTracked?.Invoke(image, imgPrefab.ImgPlacement);
        }
    }

    private void SpawnObjectOnTrackedImage(ARTrackedImage trackedImg, ImagePrefab imgPrefab)
    {
        GameObject objToSpawn = imgPrefab.Prefab;

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

        /**
        FindObjectOfType<AudioManager>().Play("BubblesBG");
        FindObjectOfType<AudioManager>().Play("Chime");
        /**/
        /**/
        FindObjectOfType<AudioManager>().Play("BubblesBG2");
        FindObjectOfType<AudioManager>().Play("Melody");
        FindObjectOfType<AudioManager>().Play("Appearance");
        /**/
    }

    // Get ImagePrefab based on the scanned image's name
    private ImagePrefab GetImagePrefab(ARTrackedImage trackedImg)
    {
        ImagePrefab imgPrefab = null;
        string imageName = trackedImg.referenceImage.name;
        for (int i = 0; i < imagePrefabs.Count; i++)
            if (imageName == imagePrefabs[i].ImgName)
            {
                imgPrefab = imagePrefabs[i];
                break;
            }

        if (imgPrefab == null)
        {
            throw new Exception($"ImageTracked: No ImagePrefab assigned for this image - {imageName}");
        }

        return imgPrefab;
    }
}