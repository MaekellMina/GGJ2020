using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FurnaceBar : MonoBehaviour {

	public Image furnaceImg;
    float barStartValue = 0f;
	private float curValue;
    private float acceleration;
    private float speed = 0.03f;
    private float minTargetValue = 0.5f;
    private float maxTargetValue = 0.6f;

    private RectTransform rectTransform;
    public RectTransform targetMarker;

    public UnityEvent Success;
    public UnityEvent Failed;

    bool isTriggred;

    public enum FurnaceHeatType
    {
        TooMuch,
        JustRight,
        TooLow
    }

	// Use this for initialization
	void Start () {
        furnaceImg.fillAmount = barStartValue;
        rectTransform = GetComponent<RectTransform>();
        SetFurnaceBar(0.1f,0.05f,0.95f,0.8f);
        isTriggred = false;
	}

	// Update is called once per frame
	void Update () {
        if (isTriggred == false)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                speed += acceleration;
                curValue += Time.deltaTime * speed;
                furnaceImg.fillAmount = curValue;

                if (furnaceImg.fillAmount >= 1)
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

    public void SetFurnaceBar(float speed, float acceleration, float maxTargetValue, float minTargetValue)
    {
        furnaceImg.fillAmount = 0;
        this.speed = speed;
        this.acceleration = acceleration;
        this.maxTargetValue = maxTargetValue;
        this.minTargetValue = minTargetValue;

        targetMarker.offsetMin = new Vector2(0, rectTransform.sizeDelta.y * minTargetValue);
    }
    public void ReleaseFurnace()
    {
        FurnaceHeatType result = HeatStatus(furnaceImg.fillAmount);
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
        isTriggred = true;
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
