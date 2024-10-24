using System;

[Serializable]
public class Frequency
{
    public uint FrequencyAmount;

	public void DecreaseFrequency()
	{
		FrequencyAmount--;
	}

	public bool IsFrequencyOver() => FrequencyAmount == 0;
}
