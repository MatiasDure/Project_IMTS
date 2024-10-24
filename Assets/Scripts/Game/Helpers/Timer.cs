using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer
{
	public float something;
	public float EndTime;

	public void DecreaseTime(float time)
	{
		EndTime -= time;
	}
}
