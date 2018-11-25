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
    public ScriptableColor currentlySelectedColor = null;

    private bool alreadySetupBeforeColoring = false;
    private ColorWheel colorWheel = null;
    private SectionSelector sectionSelector = null;
    private MonsterGenerator monsterGenerator = null;
    private ColorableSectionInstance currentlySelectedSection = null;
    private bool alreadySetupBeforePicking;

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
        if(timer.IsCountingDown)
            timer.Update();
            
        switch (currentState)
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
                if (alreadySetupBeforePicking == false)
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
        timer.StartCountdown(60f);
    }

    private void Generate()
    {
        if (currentDungeonIndex == MaxLevels)
        {
            // The player has won 
            YouHaveWon();
        }

        DungeonGenerator dungeonGenerator = new DungeonGenerator();
        Debug.Log(currentDungeonIndex);
        currentDungeon = dungeonGenerator.GenerateDungeon(Dungeons[currentDungeonIndex]);
        currentDungeonIndex++;

        currentState = GameState.PickingMonster;
    }

    public void YouHaveWon()
    {


    }

    private void PickMonster()
    {
        if (!currentDungeon.IsAvailable())
        {
            currentState = GameState.Generating;
            return;
        }

        currentEnemy = currentDungeon.NextMonster();
        currentState = GameState.CreatingScene;
    }

    private void CreateScene()
    {
        // TODO

        currentEnemy.SetActive(true);
        currentState = GameState.PickingSection;
        alreadySetupBeforePicking = false;
    }

    private void UpdateWhenPickingSection()
    {
        if (sectionSelector.IsFinishedSelecting)
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
        if (colorWheel.IsFinishedSelecting)
        {
            if (currentlySelectedColor != null)
            {
                currentlySelectedSection.SelectedColor = currentlySelectedColor.ColorValue;
                currentlySelectedSection.UseSelectedColor();
            }
            alreadySetupBeforePicking = false;
            currentState = GameState.PickingSection;
            SetStateAfterColoring();
        }
        if (currentlySelectedColor != null)
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
