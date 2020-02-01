using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class HammerBar : MonoBehaviour
{
	public RectTransform timingIndicator;   //the UI that scrolls along the bar
	public RectTransform targetArea;    //for display purposes (will indicate which area is user's target to land on)
	public Text requiredHitsUI;

	private RectTransform rectTransform;
	private CameraShake cameraShake;
	private Animator animator;
	private float scrollSpeed = 1f;
	private float targetStartPercentage = 0.45f;    //percentage of the bar where target area starts (0.5 = 50%)
	private float targetEndPercentage = 0.55f;      //percentage of the bar where target area ends (0.5 = 50%)
	private float hammerbarWidth = 100;         //width of the UI of hammer bar that the indicator will scroll through
	private int requiredHits = 3;
	private int hitsMade = 0;

	private Coroutine moveTimingIndicator;

	public UnityEvent Success;
	public UnityEvent Failed;

	private void Start()
	{
		rectTransform = GetComponent<RectTransform>();
		cameraShake = GetComponent<CameraShake>();
		animator = GetComponent<Animator>();

		SetHammerBar(3, 5f, 250, 0.65f, 0.85f);
	}

	public void SetHammerBar(int requiredHits, float scrollSpeed, float hammerbarWidth, float targetStartPercentage, float targetEndPercentage)   
	{
		this.requiredHits = requiredHits;
		hitsMade = 0;
		UpdateRequiredHitsUI();

		this.scrollSpeed = scrollSpeed;
		this.hammerbarWidth = hammerbarWidth;
		this.targetStartPercentage = targetStartPercentage;
		this.targetEndPercentage = targetEndPercentage;

		rectTransform.sizeDelta = new Vector2(hammerbarWidth, rectTransform.sizeDelta.y);
		targetArea.offsetMin = new Vector2(hammerbarWidth * targetStartPercentage, 0);
		targetArea.offsetMax = new Vector2(-hammerbarWidth * (1 - targetEndPercentage), 0);

		//randomize timing indicator pos
		timingIndicator.anchoredPosition = new Vector2(Random.Range(0f, hammerbarWidth), 0);

		StartMoveTimingIndicator();
	}

    private void StartMoveTimingIndicator()
	{
		if (moveTimingIndicator != null)
			StopCoroutine(moveTimingIndicator);
		moveTimingIndicator = StartCoroutine(MoveTimingIndicator());
	}

    private IEnumerator MoveTimingIndicator()
	{
		int step = 1;   //determines the direction of movement of timing indicator (+1 = right, -1 = left)
		bool exit = false;

		while (!exit)
		{
			//input
            if(Input.GetKeyDown(KeyCode.F))
			{
				if(timingIndicator.anchoredPosition.x >= hammerbarWidth * targetStartPercentage && 
				   timingIndicator.anchoredPosition.x <= hammerbarWidth * targetEndPercentage)
				{
					Debug.Log("CORRECT");
					hitsMade++;
					UpdateRequiredHitsUI();
					if(hitsMade >= requiredHits)
					{
						exit = true;
						Success.Invoke();
					}
				}
				else
				{
					Debug.Log("WRONG");
					cameraShake.Shake(0.1f, 0.02f);
					animator.Play("Fail");
					Failed.Invoke();
				}
			}
			
			timingIndicator.anchoredPosition += new Vector2(scrollSpeed * step, 0); //move the indicator

            //check if change dir
			if(step > 0)
			{
				if (timingIndicator.anchoredPosition.x >= hammerbarWidth)
					step = -1;
			}
			else
			{
				if (timingIndicator.anchoredPosition.x <= 0)
					step = 1;
			}

			yield return null;
		}
	}

    private void UpdateRequiredHitsUI()
	{
		requiredHitsUI.GetComponent<Animator>().Play("Pop");
		requiredHitsUI.text = hitsMade + "/" + requiredHits;
		animator.Play("Success");

	}
}
