﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DungeonGenerator{  

    private Dungeon dungeon;

    private DungeonConstants dungeonConstants;

    public Dungeon GenerateDungeon(DungeonConstants dungeonConstants)
    {
        this.dungeonConstants = dungeonConstants;
        
        dungeon = new Dungeon();
        dungeon.DungeonConstants = dungeonConstants;
        dungeon.Difficulty = dungeonConstants.DifficultyModifier;
        dungeon.ScoreMultiplier = dungeonConstants.ScoreMultiplier;
        GenerateMonsters();

        return dungeon;
    }


    private void GenerateMonsters() {
        int enemies = dungeonConstants.NumberOfMonsters;

        MonsterGenerator monsterGenerator = new MonsterGenerator(dungeonConstants);

        for (int i = 0; i < enemies; i++)
        {
            MonsterHolder monsterHolder = monsterGenerator.CreateMonsters();
            monsterHolder.Deactivate();
            
            dungeon.AddMonster(monsterHolder);            
        }        
    }

}
