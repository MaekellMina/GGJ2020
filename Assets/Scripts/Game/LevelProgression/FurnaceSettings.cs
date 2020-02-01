using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnaceSettings
{
	public float fillSpeed;
	public float fillAcceleration;
	public float minTargetValue;

	public FurnaceSettings(float fillSpeed, float fillAcceleration, float minTargetValue)
	{
		this.fillSpeed = fillSpeed;
		this.fillAcceleration = fillAcceleration;
		this.minTargetValue = minTargetValue;
	}

}
