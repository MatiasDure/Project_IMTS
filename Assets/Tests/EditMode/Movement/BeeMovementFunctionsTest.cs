using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BeeMovementFunctionsTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void EditorMode_BeeMovementFunctionsTest_ExceedsWidthBounds_HappyPath()
    {
        BeeSwimming beeSwimmingScript = new GameObject().AddComponent<BeeSwimming>();

        Vector3 middlePoint = Vector3.zero;
        float myXPosition = 3;
        float width = 10;

        // Use the Assert class to test conditions
        Assert.IsFalse(beeSwimmingScript.ExceedsWidthBounds(myXPosition, middlePoint.x, width));
    }

    [Test]
    public void EditorMode_BeeMovementFunctionsTest_ExceedsWidthBounds_UnhappyPath()
    {
        BeeSwimming beeSwimmingScript = new GameObject().AddComponent<BeeSwimming>();

        Vector3 middlePoint = Vector3.zero;
        float myXPosition = 7;
        float width = 10;

        // Use the Assert class to test conditions
        Assert.IsTrue(beeSwimmingScript.ExceedsWidthBounds(myXPosition, middlePoint.x, width));
    }

    [Test]
    public void EditorMode_BeeMovementFunctionsTest_ExceedsDepthBounds_HappyPath()
    {
        BeeSwimming beeSwimmingScript = new GameObject().AddComponent<BeeSwimming>();

        Vector3 middlePoint = Vector3.zero;
        float myZPosition = 3;
        float depth = 10;

        // Use the Assert class to test conditions
        Assert.IsFalse(beeSwimmingScript.ExceedsDepthBounds(myZPosition, middlePoint.z, depth));
    }

    [Test]
    public void EditorMode_BeeMovementFunctionsTest_ExceedsDepthBounds_UnhappyPath()
    {
        BeeSwimming beeSwimmingScript = new GameObject().AddComponent<BeeSwimming>();

        Vector3 middlePoint = Vector3.zero;
        float myZPosition = 7;
        float depth = 10;

        // Use the Assert class to test conditions
        Assert.IsTrue(beeSwimmingScript.ExceedsDepthBounds(myZPosition, middlePoint.z, depth));
    }

    [Test]
    public void EditorMode_BeeMovementFunctionsTest_ExceedsHeightBounds_HappyPath()
    {
        BeeSwimming beeSwimmingScript = new GameObject().AddComponent<BeeSwimming>();

        Vector3 middlePoint = Vector3.zero;
        float myYPosition = 3;
        float height = 10;

        // Use the Assert class to test conditions
        Assert.IsFalse(beeSwimmingScript.ExceedsHeightBounds(myYPosition, middlePoint.y, height));
    }

    [Test]
    public void EditorMode_BeeMovementFunctionsTest_ExceedsHeightBounds_UnhappyPath()
    {
        BeeSwimming beeSwimmingScript = new GameObject().AddComponent<BeeSwimming>();

        Vector3 middlePoint = Vector3.zero;
        float myYPosition = 7;
        float height = 10;

        // Use the Assert class to test conditions
        Assert.IsTrue(beeSwimmingScript.ExceedsHeightBounds(myYPosition, middlePoint.y, height));
    }

    [Test]
    public void EditorMode_BeeMovementFunctionsTest_ExceedsVerticalAngleBound_HappyPath()
    {
        BeeSwimming beeSwimmingScript = new GameObject().AddComponent<BeeSwimming>();

        float updatedAngle = 3;
        float verticalRotationBound = 10;

        // Use the Assert class to test conditions
        Assert.IsFalse(beeSwimmingScript.ExceedsVerticalAngleBound(updatedAngle, verticalRotationBound));
    }

    [Test]
    public void EditorMode_BeeMovementFunctionsTest_ExceedsVerticalAngleBound_UnhappyPath()
    {
        BeeSwimming beeSwimmingScript = new GameObject().AddComponent<BeeSwimming>();

        float updatedAngle = 11;
        float verticalRotationBound = 10;

        // Use the Assert class to test conditions
        Assert.IsTrue(beeSwimmingScript.ExceedsVerticalAngleBound(updatedAngle, verticalRotationBound));
    }
}
