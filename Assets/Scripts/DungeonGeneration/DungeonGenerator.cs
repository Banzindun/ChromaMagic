using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DungeonGenerator : MonoBehaviour{  

    private Dungeon dungeon;

    private DungeonConstants dungeonConstants;

    public Dungeon GenerateDungeon(DungeonConstants dungeonConstants)
    {
        this.dungeonConstants = dungeonConstants;
        
        dungeon = new Dungeon();
        dungeon.Difficulty = dungeonConstants.DifficultyModifier;

        GenerateMonsters();

        return dungeon;
    }


    private void GenerateMonsters() {
        int enemies = dungeonConstants.NumberOfMonsters;

        MonsterGenerator monsterGenerator = new MonsterGenerator(dungeonConstants);

        for (int i = 0; i < enemies; i++)
        {
            GameObject monster = monsterGenerator.createNewMonster();
            monster.SetActive(false);
            dungeon.AddMonster(monster);            
        }        
    }

}
