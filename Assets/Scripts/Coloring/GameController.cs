using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
	private enum GameState 
	{
			Generating,
            PickingMonster,
            CreatingScene,
			PickingSection,
			PickingColor,
	}

    public Timer timer = null;
    public float CurrentDifficultyModifier = 1f;
    public bool IsDoingColoring = false;
	public ScriptableColor currentlySelected = null;
		
    private bool alreadySetupBeforeColoring = false;
    private ColorWheel colorWheel = null;
	private SectionSelector sectionSelector = null;

    [SerializeField]
    private GameState currentState = GameState.Generating;

    private Dungeon currentDungeon = null;
    public DungeonConstants[] Dungeons;
    private int currentDungeonIndex;
    public int MaxLevels;

    private GameObject currentEnemy;


    void Start()
    {
		sectionSelector = GetComponent<SectionSelector>();
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
            case GameState.PickingMonster:
                PickMonster();
                break;
            case GameState.CreatingScene:
                CreateScene();
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
        if (currentDungeonIndex == MaxLevels) {
            // The player has won 
            YouHaveWon();
        }

        DungeonGenerator dungeonGenerator = new DungeonGenerator();
        currentDungeon = dungeonGenerator.GenerateDungeon(Dungeons[currentDungeonIndex]);
        currentDungeonIndex++;

        currentState = GameState.PickingMonster;
    }

    public void YouHaveWon() {


    }

    private void PickMonster() {
        if (!currentDungeon.IsAvailable()) {
            currentState = GameState.Generating;
            return;
        }

        currentEnemy = currentDungeon.NextMonster();
        currentState = GameState.CreatingScene;
    }

    private void CreateScene() {
        // TODO

        currentEnemy.SetActive(true);
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
