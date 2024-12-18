using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

[Serializable]
public struct ImageAnchorCollection
{
    public ARTrackedImage Image;
	public GameObject PlotObject;
	public GameObject AnchorObject;
}
