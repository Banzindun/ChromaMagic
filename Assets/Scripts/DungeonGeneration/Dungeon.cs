using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dungeon {

    public DungeonConstants DungeonConstants;

    public float Difficulty;

    private List<MonsterHolder> monsters;
        
    public Dungeon()
    {
        monsters = new List<MonsterHolder>();
    }

    public bool IsAvailable() {
        return monsters.Count > 0;
    }

    public MonsterHolder NextMonster() {
        MonsterHolder monster = monsters[0];
        monsters.RemoveAt(0);
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
