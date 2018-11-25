﻿using System;
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

    public float YEnvironmentOffset = -11f;

    public float EnemiesOffset = 30;

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
        
        // Max size .. the middle should be 0
        float size = (monsterHolder.Length - 1) * EnemiesOffset;
        float currentXPosition = - size/2;
        
        for (int i = 0; i < monsterHolder.Length; i++)
        {
            ColorableInstance colorableInstance = monsterHolder[i].MonsterOutlined.GetComponent<ColorableInstance>();
            colorableInstance.MakeColoredModel();
            GameObject sceneMonster = monsterHolder[i].SceneMonster;

            sceneMonster.transform.parent = BackgroundSlot.transform;
            sceneMonster.transform.localScale = colorableInstance.Colorable.EnvironmentScale;
            sceneMonster.transform.localPosition = new Vector3(currentXPosition, colorableInstance.Colorable.YEnvironmentOffset, 0.1f);

            currentXPosition += EnemiesOffset; 
        }

        EnemiesAssignedToScene = true;
    }

}
