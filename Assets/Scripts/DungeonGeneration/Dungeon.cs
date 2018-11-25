using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dungeon {

    public DungeonConstants DungeonConstants;

    public float ScoreMultiplier;

    public float Difficulty;

    private List<MonsterHolder> monsters;
    private int nextMonsterIndex = 0;
        
    public Dungeon()
    {
        monsters = new List<MonsterHolder>();
    }

    public bool IsAvailable() {
        return  nextMonsterIndex < monsters.Count;
    }

    public MonsterHolder NextMonster() {
        MonsterHolder monster = monsters[nextMonsterIndex];
        nextMonsterIndex++;
        return monster;
    }

    public void AddMonster(MonsterHolder monster) {
        monsters.Add(monster);
    }

    public MonsterHolder[] GetAllMonsters()
    {
        return monsters.ToArray();
    }
}
