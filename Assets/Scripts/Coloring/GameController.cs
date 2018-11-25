using System;
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
        TimeIsUp,
        Dead,
        Won
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
    private int coloredSections = 0;

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


    private ColorableInstance currentColorableInstance = null;

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
                UpdateWhenTimeIsUp();
                break;
            case GameState.Dead:
                UpdateWhenDead();
                break;
            case GameState.Won:
                PlayerHasWon();
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
        if (CurrentDungeonIndex == MaxLevels)
        {
            // The player has won 
            currentState = GameState.Won;

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

    public void PlayerHasWon()
    {
        // display some gui saying the number of enemies and/or score and/or health left
    }

    private void PickMonster()
    {
        if (!currentDungeon.IsAvailable())
        {
            currentState = GameState.Generating;
            timer.Reset();
            colorWheel.Deactivate();
            return;
        }

        currentMonsterHolder = currentDungeon.NextMonster();
        currentColorableInstance = currentMonsterHolder.MonsterOutlined.GetComponent<ColorableInstance>();
        coloredSections = 0;

        currentState = GameState.CreatingScene;
    }

    private void CreateScene()
    {
        SceneGenerator.InsertMonsters(currentMonsterHolder);

        currentState = GameState.PickingSection;
        alreadySetupBeforePicking = false;
        timer.StartCountdown(currentColorableInstance.Colorable.BaseTimer * currentDungeon.Difficulty);
    }

    private void UpdateWhenPickingSection()
    {
        if(timer.IsNoTimeLeft)
        {
            sectionSelector.StopSelecting();
            currentState = GameState.TimeIsUp;
            return;
        }
        if (sectionSelector.IsFinishedSelecting)
        {
            currentlySelectedSection = sectionSelector.SelectedColorableSectionInstance;
            currentState = GameState.PickingColor;
        }
    }

    private void SetStateBeforeColoring()
    {
        alreadySetupBeforeColoring = true;
        colorWheel.Activate(currentlySelectedSection.transform.position);
        List<Color> palette = GenerateColorPalleteFromColorSet(currentColorableInstance.InstanceColorSet.colors);
        colorWheel.InitializeColorPalette(palette);
    }

    private List<Color> GenerateColorPalleteFromColorSet(Color[] colors)
    {
        List<Color> palette = new List<Color>();
        for(int i = 0; i < colors.Length; ++i)
        {
            palette.Add(colors[i]);
            palette.Add(MakeSimiliarColor(colors[i], 1));
            palette.Add(MakeSimiliarColor(colors[i], -1));
        }

        ShuffleColorPalette(palette);
        return palette;
    }

    private Color MakeSimiliarColor(Color color, int sign)
    {
        float h, s, v;
        
        Color.RGBToHSV(color, out h, out s, out v);
        float sample = UnityEngine.Random.Range(0f, 0.1f) + 0.10f;
        if(s < 0.5f || v < 0.5f)
        {
            v += sign * sample;
            s += sign * sample;
        }
        else
        {
            h += sign * sample;
            s += sign * sample;
        }

        h = Mathf.Clamp(h,0f,1f);
        s = Mathf.Clamp(s,0f,1f);
        v = Mathf.Clamp(v,0f,1f);
        return Color.HSVToRGB(h,s,v);
    }

    private void ShuffleColorPalette(List<Color> palette)
    {
        for(int i = 0; i < palette.Count; ++i)
        {
            Color tmp = palette[i];
            int randomIndex = UnityEngine.Random.Range(0, palette.Count);
            palette[i] = palette[randomIndex];
            palette[randomIndex] = tmp;
        }
    }

    private void UpdateWhenColoring()
    {
        if (timer.IsNoTimeLeft)
        {
            SetStateAfterColoring();
            currentState = GameState.TimeIsUp;
        }

        if (colorWheel.IsFinishedSelecting)
        {
            SetStateAfterColoring();
            if (currentlySelectedColor != null)
            {
                currentColorableInstance.SetColor(currentlySelectedSection.index, currentlySelectedColor.ColorValue);
                currentColorableInstance.TurnOnLayer(ColorableInstance.LAYER_TYPE.COLORS, currentlySelectedSection.index);
            }

            ++coloredSections;
            Debug.Log(currentColorableInstance.SectionHolders.Length);
            if(coloredSections == currentColorableInstance.SectionHolders.Length)
            {
                SetStateAfterFinishedMonster();
                currentState = GameState.PickingMonster;
                return;
            }
            alreadySetupBeforePicking = false;
            currentState = GameState.PickingSection;
            return;
        }

        if (currentlySelectedColor != null)
        {
            currentlySelectedSection.GetComponentInParent<ColorableInstance>();
            currentColorableInstance.SetColor(currentlySelectedSection.index, currentlySelectedColor.ColorValue);
            currentColorableInstance.TurnOnLayer(ColorableInstance.LAYER_TYPE.COLORS, currentlySelectedSection.index);
        }
        
    }

    private void SetStateAfterFinishedMonster()
    {
        currentColorableInstance.CalculateScore();
        Destroy(currentMonsterHolder.MonsterModel);
        Destroy(currentMonsterHolder.MonsterOutlined);
    }

    private void UpdateWhenTimeIsUp()
    {
        SetStateAfterFinishedMonster();
        currentState = GameState.PickingMonster;
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
        playerHealth.LooseHealth();

        if (playerHealth.IsDead())
        {
            currentState = GameState.Dead;
            timer.Reset();
            colorWheel.Deactivate();
        }
    }

    private void UpdateWhenDead()
    {
        // show some UI, if time maybe a button to retry
    }
}
