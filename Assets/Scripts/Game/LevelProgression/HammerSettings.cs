using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HammerSettings
{
	public int minRequiredHits;
	public int maxRequiredHits;
	public float scrollSpeed;
	public float hammerbarWidth;
	public float startTargetPercentage;
	public float endTargetPercentage;
	public bool randomizeTargetRangePos;

	public HammerSettings(int minRequiredHits, int maxRequiredHits, float scrollSpeed, float hammerbarWidth, float startTargetPercentage, float endTargetPercentage, bool randomizeTargetRangePos)
	{
		this.minRequiredHits = minRequiredHits;
		this.maxRequiredHits = maxRequiredHits;
		this.scrollSpeed = scrollSpeed;
		this.hammerbarWidth = hammerbarWidth;
		this.startTargetPercentage = startTargetPercentage;
		this.endTargetPercentage = endTargetPercentage;
		this.randomizeTargetRangePos = randomizeTargetRangePos;
	}
}
