using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FurnaceBar : MonoBehaviour {

	public RectTransform furnaceFill;
    float barStartValue = 0f;
	private float curValue;
    private float acceleration;
    private float speed = 0.03f;
    private float minTargetValue = 0.5f;
    private float maxTargetValue = 0.95f;
	private float fillAmount = 0;

	public RectTransform parentRectTransform;
    public RectTransform targetMarker;

    public UnityEvent Success;
    public UnityEvent Failed;

    bool isTriggered;

    public enum FurnaceHeatType
    {
        TooMuch,
        JustRight,
        TooLow
    }

	public void Awake () 
	{
		SetFurnaceBar(new FurnaceSettings(0.1f, 0.05f, 0.8f));
	}

	void Update () {
        if (isTriggered == false)
        {
            if (Input.GetKey(KeyCode.Space))
            {
				speed += acceleration;
				fillAmount += Time.deltaTime * speed;
				furnaceFill.anchoredPosition = new Vector2(furnaceFill.anchoredPosition.x,
				                                           parentRectTransform.sizeDelta.y * fillAmount);

                if (fillAmount >= 1)
                {
                    ReleaseFurnace();
                    return;
                }
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                ReleaseFurnace();
            }
        }
    }

	public void SetFurnaceBar(FurnaceSettings furnaceSettings)
    {
        fillAmount = 0;
		furnaceFill.anchoredPosition = new Vector2(furnaceFill.anchoredPosition.x, 0);
		this.speed = furnaceSettings.fillSpeed;
		this.acceleration = furnaceSettings.fillAcceleration;
		this.minTargetValue = furnaceSettings.minTargetValue;

        targetMarker.offsetMin = new Vector2(0, parentRectTransform.sizeDelta.y * minTargetValue);
		isTriggered = false;
    }
    public void ReleaseFurnace()
    {
        FurnaceHeatType result = HeatStatus(fillAmount);
        Debug.Log(result);
        switch (result)
        {
            case FurnaceHeatType.TooMuch:
                Failed.Invoke();
                break;
            case FurnaceHeatType.JustRight:
                Success.Invoke();
                break;
            case FurnaceHeatType.TooLow:
                Failed.Invoke();
                break;
        }
        isTriggered = true;
    }
    
    public FurnaceHeatType HeatStatus(float amount)
    {
        if (amount > maxTargetValue)
            return FurnaceHeatType.TooMuch;
        else if (amount > minTargetValue && amount < maxTargetValue)
            return FurnaceHeatType.JustRight;
        else 
            return FurnaceHeatType.TooLow;
    }
}
