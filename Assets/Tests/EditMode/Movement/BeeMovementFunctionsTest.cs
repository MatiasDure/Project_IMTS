using NUnit.Framework;
using UnityEngine;

public class BeeMovementFunctionsTest
{
    [Test]
    public void EditorMode_BeeMovementFunctionsTest_ExceedsVerticalAngleBound_HappyPath()
    {
        BeeIdleSwimming beeSwimmingScript = new GameObject().AddComponent<BeeIdleSwimming>();

        float updatedAngle = 3;
        float verticalRotationBound = 10;

        Assert.IsFalse(beeSwimmingScript.ExceedsVerticalAngleBound(updatedAngle, verticalRotationBound));
    }

    [Test]
    public void EditorMode_BeeMovementFunctionsTest_ExceedsVerticalAngleBound_UnhappyPath()
    {
        BeeIdleSwimming beeSwimmingScript = new GameObject().AddComponent<BeeIdleSwimming>();

        float updatedAngle = 11;
        float verticalRotationBound = 10;

        Assert.IsTrue(beeSwimmingScript.ExceedsVerticalAngleBound(updatedAngle, verticalRotationBound));
    }
}
