using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPlot : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Appearance");
        FindObjectOfType<AudioManager>().Play("ForestRiverBG");
    }
}
