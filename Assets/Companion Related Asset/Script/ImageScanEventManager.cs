using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARTrackedImageManager))]
public class ImageScanEventManager : MonoBehaviour
{
    [SerializeField] private GameObject prefabsToSpawn;
    private ARTrackedImageManager _arTrackedImageManager;
    
    private void Awake()
    {
        _arTrackedImageManager = GetComponent<ARTrackedImageManager>();

        
    }

    private void Start()
    {
        //listen to event related to changed in trackedImage
        _arTrackedImageManager.trackedImagesChanged += OnTrackImageChanged;
        
        //spawn the game object per image and hide them
        prefabsToSpawn = Instantiate(prefabsToSpawn, Vector3.zero, quaternion.identity);
        prefabsToSpawn.gameObject.SetActive(false);
        
    }

    private void OnTrackImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        //identify the change on trackedImage
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            //show, hide or reposition gameObjects
            UpdateTrackedImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            //show, hide or reposition gameObjects
            UpdateTrackedImage(trackedImage);
        }
        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            //event for when remove object
        }
    }

    private void UpdateTrackedImage(ARTrackedImage trackedImage)
    {
        //check tracking status of the trackedImage
        if (trackedImage.trackingState is TrackingState.Limited or TrackingState.None)
        {
            //event for when weak track or no track
        }
        //show, hide or reposition gameObjects
        if (prefabsToSpawn != null)
        {
            prefabsToSpawn.gameObject.SetActive(true);
        }
    }
    
    //remove this eventArg when destroy
    private void OnDestroy()
    {
        _arTrackedImageManager.trackedImagesChanged -= OnTrackImageChanged;
    }
}
