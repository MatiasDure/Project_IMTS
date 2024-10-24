using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class EnvironmentBounds
{
    public Bounds bounds;

    public void SetCenter(Vector3 centerPoint)
    {
        bounds.center = centerPoint;
    }

    public bool ExceedsWidthBounds(float currentX) => currentX > bounds.center.x + bounds.extents.x / 2 || currentX < bounds.center.x + -bounds.extents.x / 2;
    public bool ExceedsDepthBounds(float currentZ) => currentZ > bounds.center.z + bounds.extents.z / 2 || currentZ < bounds.center.z + -bounds.extents.z / 2;
    public bool ExceedsHeightBounds(float currentY) => currentY > bounds.center.y + bounds.extents.y / 2 || currentY < bounds.center.y + -bounds.extents.y / 2;

}
