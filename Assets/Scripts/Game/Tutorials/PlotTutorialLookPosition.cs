using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlotTutorialLookPosition : MonoBehaviour
{
	public bool IsDiscovered => _isDiscovered;
	
	private bool _isDiscovered = false;
	
	public void DiscoverPosition()
	{
		_isDiscovered = true;
	}
}
