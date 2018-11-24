using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
		private enum GameState 
		{
				Generating,
				PickingSection,
				PickingColor,
		}

    public Timer timer = null;
    public float CurrentDifficultyModifier = 1f;
    public bool IsDoingColoring = false;
    private bool alreadySetupBeforeColoring = false;
    private ColorWheel colorWheel = null;


		
		private SectionSelector sectionSelector = null;
		private MonsterGenerator monsterGenerator = null;
		private GameState currentState = GameState.Generating;


    void Start()
    {
				sectionSelector = GetComponent<SectionSelector>();
				monsterGenerator = GetComponent<MonsterGenerator>();
				timer.Reset();
				currentState = GameState.Generating;
        colorWheel = GameObject.Find("ColorWheel").GetComponent<ColorWheel>();
        colorWheel.Deactivate();
    }

    void Update()
    {
				switch(currentState)
				{
						case GameState.Generating:
								Generate();
								break;
						case GameState.PickingSection:
								UpdateWhenPickingSection();
								break;
						case GameState.PickingColor:
            		if (alreadySetupBeforeColoring == false)
            		    SetStateBeforeColoring();
            		UpdateWhenColoring();
								break;
				}
    }

    private void Generate()
    {
				var colorableObject = monsterGenerator.createNewColorableMonster();
				currentState = GameState.PickingSection;
    }

    private void UpdateWhenPickingSection()
    {
			
    }

    private void SetStateBeforeColoring()
    {
        alreadySetupBeforeColoring = true;
        //coloringTimer.StartCountdown(currentColorable.BaseTimer * CurrentDifficultyModifier);
        timer.StartCountdown(20f);
				
        colorWheel.Activate();
    }

    private void UpdateWhenColoring()
    {

        timer.Update();
        if (timer.IsNoTimeLeft)
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
