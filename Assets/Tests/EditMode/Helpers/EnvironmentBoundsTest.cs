using NUnit.Framework;
using UnityEngine;

public class EnvironmentBoundsTest
{
    [Test]
    public void EditorMode_EnvironmentBoundsTest_ExceedsWidthBounds_HappyPath()
    {

        Vector3 middlePoint = Vector3.zero;
        Vector3 size = new Vector3(5, 0.8f, 5);
        float currentX = 2;

        EnvironmentBounds environmentBoundsScript = new EnvironmentBounds(middlePoint, size);

        Assert.IsFalse(environmentBoundsScript.ExceedsWidthBounds(currentX));
    }

    [Test]
    public void EditorMode_EnvironmentBoundsTest_ExceedsWidthBounds_UnhappyPath()
    {

        Vector3 middlePoint = Vector3.zero;
        Vector3 size = new Vector3(5, 0.8f, 5);
        float currentX = 7;

        EnvironmentBounds environmentBoundsScript = new EnvironmentBounds(middlePoint, size);

        Assert.IsTrue(environmentBoundsScript.ExceedsWidthBounds(currentX));
    }

    [Test]
    public void EditorMode_EnvironmentBoundsTest_ExceedsDepthBounds_HappyPath()
    {

        Vector3 middlePoint = Vector3.zero;
        Vector3 size = new Vector3(5, 0.8f, 5);
        float currentZ = 2;

        EnvironmentBounds environmentBoundsScript = new EnvironmentBounds(middlePoint, size);

        Assert.IsFalse(environmentBoundsScript.ExceedsDepthBounds(currentZ));
    }

    [Test]
    public void EditorMode_EnvironmentBoundsTest_ExceedsDepthBounds_UnhappyPath()
    {

        Vector3 middlePoint = Vector3.zero;
        Vector3 size = new Vector3(5, 0.8f, 5);
        float currentZ = 7;

        EnvironmentBounds environmentBoundsScript = new EnvironmentBounds(middlePoint, size);

        Assert.IsTrue(environmentBoundsScript.ExceedsDepthBounds(currentZ));
    }

    [Test]
    public void EditorMode_EnvironmentBoundsTest_ExceedsHeightBounds_HappyPath()
    {

        Vector3 middlePoint = Vector3.zero;
        Vector3 size = new Vector3(5, 0.8f, 5);
        float currentY = 0.3f;

        EnvironmentBounds environmentBoundsScript = new EnvironmentBounds(middlePoint, size);

        Assert.IsFalse(environmentBoundsScript.ExceedsHeightBounds(currentY));
    }

    [Test]
    public void EditorMode_EnvironmentBoundsTest_ExceedsHeightBounds_UnhappyPath()
    {

        Vector3 middlePoint = Vector3.zero;
        Vector3 size = new Vector3(5, 0.8f, 5);
        float currentY = 7;

        EnvironmentBounds environmentBoundsScript = new EnvironmentBounds(middlePoint, size);

        Assert.IsTrue(environmentBoundsScript.ExceedsHeightBounds(currentY));
    }
}
