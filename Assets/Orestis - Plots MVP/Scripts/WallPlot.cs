using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallPlot : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("BubblesBG2");
        FindObjectOfType<AudioManager>().Play("Melody");
        FindObjectOfType<AudioManager>().Play("Appearance");
    }
}
