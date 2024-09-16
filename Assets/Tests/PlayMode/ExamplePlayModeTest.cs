using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class NewTestScript
{ 
    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator PlayMode_TestClassExample_MoveUp()
    {
        var testClass = GetPlayModeTestClassExampleInstance();

        testClass.PrivateMoveUpTest();

        Vector3 expectedValue = new Vector3(0, 1, 0);

        // Use yield to skip a frame.
        yield return null;

        // Use the Assert class to test conditions.
        Assert.AreEqual(expectedValue, testClass.transform.position);
    }

    [UnityTest]
    public IEnumerator PlayMode_TestClassExample_MoveDown()
    {
        var testClass = GetPlayModeTestClassExampleInstance();

        testClass.PublicMoveDownTest();

        Vector3 expectedValue = new Vector3(0, -1, 0);

        // Use yield to skip a frame.
        yield return null;

        // Use the Assert class to test conditions.
        Assert.AreEqual(expectedValue, testClass.transform.position);
    }

    private PlayModeTestClassExample GetPlayModeTestClassExampleInstance() => new GameObject().AddComponent<PlayModeTestClassExample>();
}
