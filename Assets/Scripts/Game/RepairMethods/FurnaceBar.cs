using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FurnaceBar : MonoBehaviour {

	public RectTransform furnaceFill;
	public GameObject tutorialText1;
	public GameObject tutorialText2;
    float barStartValue = 0f;
	private float curValue;
    private float acceleration;
    private float speed = 0.03f;
    private float minTargetValue = 0.5f;
    private float maxTargetValue = 0.95f;
	private float fillAmount = 0;

	private bool tutorialDone;

	public RectTransform parentRectTransform;
    public RectTransform targetMarker;

    public UnityEvent Success;
    public UnityEvent Failed;

    bool isTriggered;
	bool started;

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
		if(GameManager.instance.gameState == GameManager.GAMESTATES.GAMEOVER)
		{
			return;
		}

        if (isTriggered == false)
        {
			if (Input.GetKeyDown(KeyCode.Space)) 
			{
				started = true;
                int randomAudio = UnityEngine.Random.Range(23, 25);
                AudioManager.instance.PlayAudioClip(randomAudio);
            }
			if (started)
			{
				if (Input.GetKey(KeyCode.Space))
				{
					speed += acceleration;

					if (!tutorialDone &&
                (!tutorialText2.activeInHierarchy &&
					fillAmount > (minTargetValue + maxTargetValue) / 2))
                    {
						tutorialText1.SetActive(false);
                        tutorialText2.SetActive(true);
                    }

					if (!tutorialText2.activeInHierarchy)
					{
						fillAmount += Time.deltaTime * speed;
						furnaceFill.anchoredPosition = new Vector2(furnaceFill.anchoredPosition.x,
																   parentRectTransform.sizeDelta.y * fillAmount);
					}

					if (fillAmount >= 1)
					{
						ReleaseFurnace();
						return;
					}
				}

				if (Input.GetKeyUp(KeyCode.Space))
				{
					if(!tutorialDone)
					{
						tutorialDone = true;
						tutorialText1.SetActive(false);
						tutorialText2.SetActive(false);
					}
					ReleaseFurnace();
				}
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
		started = false;

		if (!tutorialDone)
			tutorialText1.SetActive(true);
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
