using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance = null;
   

    private enum GameState
    {
        Generating,
        PickingMonster,
        CreatingScene,
        PickingSection,
        PickingColor,
        TimeIsUp
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
    public int CurrentDungeonIndex;
    public int MaxLevels;

    
    private MonsterHolder currentMonsterHolder;

    public SceneGenerator SceneGenerator;

    private ScoreController scoreController;

    private PlayerHealth playerHealth;

    private float score;

    public float Score {
        get
        {
            return score;

        }
        set
        {
            score = value;

            // Update the score
            scoreController.UpdateScore();
        }
    }

    public int Health{
        get;
        set;
    }



    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        scoreController = GetComponent<ScoreController>();
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
            case GameState.TimeIsUp:
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
        if (CurrentDungeonIndex == MaxLevels)
        {
            // The player has won 
            YouHaveWon();

            return; // EXIT the app
        }

        DungeonGenerator dungeonGenerator = new DungeonGenerator();

        currentDungeon = dungeonGenerator.GenerateDungeon(Dungeons[CurrentDungeonIndex]);
        CurrentDungeonIndex++;

        // Reset scene generator
        SceneGenerator.Reset();
        SceneGenerator.TryInsertToEnvironment(currentDungeon);

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

        currentMonsterHolder = currentDungeon.NextMonster();

        currentState = GameState.CreatingScene;
    }

    private void CreateScene()
    {
        SceneGenerator.InsertMonsters(currentMonsterHolder);

        currentState = GameState.PickingSection;
        alreadySetupBeforePicking = false;
    }

    private List<Color> GetRandomColorSet()
    {
        var instance = currentMonsterHolder.MonsterOutlined.GetComponent<ColorableInstance>();
        int index = UnityEngine.Random.Range(0, instance.Colorable.ColorSets.Length);
        return new List<Color>(instance.Colorable.ColorSets[index].colors);
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
        List<Color> palette = GetRandomColorSet();
        colorWheel.InitializeColorPallette(palette);
    }

    private void UpdateWhenColoring()
    {
        if (colorWheel.IsFinishedSelecting)
        {
            if (currentlySelectedColor != null)
            {
                ColorableInstance colorableInstance = currentlySelectedSection.GetComponentInParent<ColorableInstance>();
                colorableInstance.SetColor(currentlySelectedSection.index, currentlySelectedColor.ColorValue);
                colorableInstance.TurnOnLayer(ColorableInstance.LAYER_TYPE.COLORS, currentlySelectedSection.index);

            }
            alreadySetupBeforePicking = false;
            currentState = GameState.PickingSection;
            SetStateAfterColoring();
        }

        if (currentlySelectedColor != null)
        {
            ColorableInstance colorableInstance = currentlySelectedSection.GetComponentInParent<ColorableInstance>();
            colorableInstance.SetColor(currentlySelectedSection.index, currentlySelectedColor.ColorValue);
            colorableInstance.TurnOnLayer(ColorableInstance.LAYER_TYPE.COLORS, currentlySelectedSection.index);
        }

        timer.Update();
        if (timer.IsNoTimeLeft)
        {
            SetStateAfterColoring();
        currentState = GameState.TimeIsUp;
        }
    }

    private void SetStateAfterColoring()
    {
        IsDoingColoring = false;
        alreadySetupBeforeColoring = false;
        colorWheel.Deactivate();
    }

    // Called when the player should loose health
    public void LooseHealth()
    {
        Debug.Log("Player has lost health!!");
        playerHealth.LooseHealth();

        if (playerHealth.IsDead())
        {
            Debug.Log("I have lost the game. FUCK.");
        }
    }
}
