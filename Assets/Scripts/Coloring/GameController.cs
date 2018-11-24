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
		public ScriptableColor currentlySelectedColor = null;

    private bool alreadySetupBeforeColoring = false;
    private ColorWheel colorWheel = null;
		private SectionSelector sectionSelector = null;
		private MonsterGenerator monsterGenerator = null;
		private GameState currentState = GameState.Generating;
		private ColorableSectionInstance currentlySelectedSection = null;
    private bool alreadySetupBeforePicking;

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
		//	Debug.Log(currentState);
				switch(currentState)
				{
						case GameState.Generating:
								Generate();
								break;
						case GameState.PickingSection:
								if(alreadySetupBeforePicking == false)
									SetStateBeforeSectionPicking();
								UpdateWhenPickingSection();
								break;
						case GameState.PickingColor:
            		if (alreadySetupBeforeColoring == false)
            		    SetStateBeforeColoring();
            		UpdateWhenColoring();
								break;
				}
    }

    private void SetStateBeforeSectionPicking()
    {
			alreadySetupBeforePicking = true;
			sectionSelector.StartSelecting();
    }

    private void Generate()
    {
				var colorableObject = monsterGenerator.createNewColorableMonster();
				currentState = GameState.PickingSection;
        timer.StartCountdown(20f);
    }
	
    private void UpdateWhenPickingSection()
    {
			  if(sectionSelector.IsFinishedSelecting)
				{
						currentlySelectedSection = sectionSelector.SelectedColorableSectionInstance;
						currentState = GameState.PickingColor;
				}
    }

    private void SetStateBeforeColoring()
    {
        alreadySetupBeforeColoring = true;
        //coloringTimer.StartCountdown(currentColorable.BaseTimer * CurrentDifficultyModifier);
        colorWheel.Activate(currentlySelectedSection.transform.position);
    }

    private void UpdateWhenColoring()
    {
				if(colorWheel.IsFinishedSelecting)
				{
						if(currentlySelectedColor != null)
						{
							currentlySelectedSection.SelectedColor = currentlySelectedColor.ColorValue;
							currentlySelectedSection.UseSelectedColor();
						}
						alreadySetupBeforePicking = false;
						currentState = GameState.PickingSection;
						colorWheel.Deactivate();
				}
				if(currentlySelectedColor != null)
				{
					currentlySelectedSection.SelectedColor = currentlySelectedColor.ColorValue;
					currentlySelectedSection.UseSelectedColor();
				}

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
