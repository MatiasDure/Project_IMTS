using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class BeeMovementFunctionsTest
{
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
