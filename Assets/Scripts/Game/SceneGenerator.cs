using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SceneGenerator : MonoBehaviour {

    public Image BackgroundSlot;

    public GameObject ModelSlot;

    public GameObject OutlinedModelSlot;

    private bool EnemiesAssignedToScene = false;

    public float YEnvironmentOffset = -30f;

    public float MinXEnvironmentOffset = -30f;

    public float MaxXEnvironmentOffset = 30f;

    public void Start()
    {
        GameController.Instance.SceneGenerator = this;
    }

    public void AssignDungeonBackground(Sprite background) {
        BackgroundSlot.sprite = background;
        // resize??
    }

    public void AddMonstersToBackground(Dungeon dungeon) {
        //GameObject[] monsters = dungeon;
    }

    public void InsertMonsters(MonsterHolder holder) {
        GameObject monster = holder.MonsterModel;
        GameObject outline = holder.MonsterOutlined;
                
        // Enemy OUTLINE
        ColorableInstance colorableInstance = outline.GetComponent<ColorableInstance>();
        colorableInstance.MakeColoredModel();

        outline.SetActive(true);
        outline.transform.parent = OutlinedModelSlot.transform;
        outline.transform.localPosition = new Vector3(0, 0, outline.transform.localPosition.z);
        outline.transform.localScale = colorableInstance.Colorable.ModelScale;


        // ENEMY ColoredModel
        colorableInstance = monster.GetComponent<ColorableInstance>();
        colorableInstance.MakeModel();

        monster.SetActive(true);
        monster.transform.parent = ModelSlot.transform;
        monster.transform.localPosition = new Vector3(0, 0, monster.transform.localPosition.z);
        monster.transform.localScale = colorableInstance.Colorable.ModelScale;
    }

    public void Reset()
    {
        EnemiesAssignedToScene = false;
    }

    internal void TryInsertToEnvironment(Dungeon dungeon)
    {
        if (EnemiesAssignedToScene) {
            return;
        }

        AssignDungeonBackground(dungeon.DungeonConstants.background);

        MonsterHolder[] monsterHolder = dungeon.GetAllMonsters();

        for (int i = 0; i < monsterHolder.Length; i++)
        {
            monsterHolder[i].MonsterOutlined.GetComponent<ColorableInstance>().MakeColoredModel();
            GameObject sceneMonster = monsterHolder[i].SceneMonster;

            sceneMonster.transform.parent = BackgroundSlot.transform;

            float zOffset = UnityEngine.Random.Range(-1.5f, 1.5f);
            float xOffset = UnityEngine.Random.Range(MinXEnvironmentOffset, MaxXEnvironmentOffset);
            sceneMonster.transform.localPosition = new Vector3(xOffset, YEnvironmentOffset, zOffset);
        }

        EnemiesAssignedToScene = true;
    }

}
