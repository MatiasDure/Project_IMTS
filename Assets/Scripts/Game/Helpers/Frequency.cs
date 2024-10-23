using System;
using UnityEngine;

[Serializable]
public class Frequency
{
    public float FrequencyAmount;

	public void DecreaseFrequency()
	{
		FrequencyAmount--;
	}

	public bool IsFrequencyOver() => FrequencyAmount == 0;
}
