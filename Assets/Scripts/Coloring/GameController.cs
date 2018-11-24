using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour 
{
	public Timer coloringTimer = null;
	public float CurrentDifficultyModifier = 1f;
	public bool IsDoingColoring = false;
	public ColorWheel colorWheelPrefab = null;
	private bool alreadySetupBeforeColoring = false;
	private Colorable currentColorable = null;
	private ColorWheel colorWheel = null;


	void Start () 
	{
		colorWheel = GameObject.Find("ColorWheel").GetComponent<ColorWheel>();
		colorWheel.Deactivate();		
	}
	
	void Update () 
	{
		if(IsDoingColoring)
		{
			if(alreadySetupBeforeColoring == false)
				SetStateBeforeColoring();
			UpdateWhenColoring();
		}
		else
		{

		}
	}

    private void SetStateBeforeColoring()
    {
		alreadySetupBeforeColoring = true;
		//coloringTimer.StartCountdown(currentColorable.BaseTimer * CurrentDifficultyModifier);
		coloringTimer.StartCountdown(5f);
		colorWheel.Activate();
    }

    private void UpdateWhenColoring()
    {
		coloringTimer.Update();
		if(coloringTimer.IsNoTimeLeft)
		{
			SetStateAfterColoring();
		}
    }

    private void SetStateAfterColoring()
    {
		IsDoingColoring = false;
		alreadySetupBeforeColoring = false;
		colorWheel.Deactivate();
    }
}
