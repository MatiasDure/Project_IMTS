using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ImagePrefab
{
    public enum ImagePlacement
    {
        Wall,
        Floor,
    }

    public string ImgName;
    public GameObject Prefab;
    public ImagePlacement ImgPlacement;

}
