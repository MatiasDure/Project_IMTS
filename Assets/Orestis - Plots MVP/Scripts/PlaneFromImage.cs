using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

public class PlaneFromImage : MonoBehaviour
{
    [SerializeField] private Transform planesParentGO;
    [SerializeField] private GameObject planePrefab;

    static int planesSpawned = 0;

    private void OnEnable()
    {
        ImageTracker.OnImageTracked += CreatePlaneFromTrackedImage;
    }

    private void OnDisable()
    {
        ImageTracker.OnImageTracked -= CreatePlaneFromTrackedImage;
    }

    private void CreatePlaneFromTrackedImage(ARTrackedImage trackedImg,
                               ImagePrefab.ImagePlacement imgPlacement)
    {
        // Spawn plane prefab
        GameObject ARPlane = Instantiate(planePrefab, trackedImg.transform.position,
                                      trackedImg.transform.rotation, planesParentGO);

        // Adjust position
        Vector3 newPos = ARPlane.transform.position;
        newPos.y = newPos.x = 0;
        ARPlane.transform.position = newPos;

        // Change name
        ARPlane.name = $"ARPlane{++planesSpawned}";

        LineRenderer rend = ARPlane.GetComponent<LineRenderer>();
        rend.SetPositions(GetImageVertexPositions(trackedImg));
    }

    private Vector3[] GetImageVertexPositions(ARTrackedImage trackedImg)
    {
        Vector3[] positions = new Vector3[4];

        // Image details
        float width = trackedImg.size.x;
        float height = trackedImg.size.y;
        Vector3 pos = trackedImg.transform.position;

        // Top Left
        positions[0] = new Vector3(pos.x - width / 2, 0, pos.y + height / 2);
        // Top Right
        positions[1] = new Vector3(pos.x + width / 2, 0, pos.y + height / 2);
        // Bottom Left
        positions[2] = new Vector3(pos.x - width / 2, 0, pos.y - height / 2);
        // Bottom Right
        positions[3] = new Vector3(pos.x + width / 2, 0, pos.y - height / 2);

        return positions;
    }
}