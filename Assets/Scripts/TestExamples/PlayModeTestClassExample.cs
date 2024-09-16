using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayModeTestClassExample : MonoBehaviour
{
    private readonly Vector3 North = Vector3.up;
    private readonly Vector3 South = Vector3.down;

    internal void PrivateMoveUpTest()
    {
        transform.position += North;
    }

    public void PublicMoveDownTest()
    {
        transform.position += South;
    }
}
