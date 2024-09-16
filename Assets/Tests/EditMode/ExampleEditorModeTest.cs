using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ExampleEditorModeTest
{
    EditorModeTestClassExample testClass = new EditorModeTestClassExample();
    // A Test behaves as an ordinary method
    [Test]
    public void EditorMode_TestClassExample_AddTest()
    {
        int expectedValue = 4;
        // Use the Assert class to test conditions
        Assert.AreEqual(expectedValue, testClass.AddPublicMethod(2,2));
    }

    [Test]
    public void EditorMode_TestClassExample_SubtractTest()
    {
        int expectedValue = 3;

        Assert.IsTrue(expectedValue == testClass.SubPrivateMethod(9, 6));
    }
}
