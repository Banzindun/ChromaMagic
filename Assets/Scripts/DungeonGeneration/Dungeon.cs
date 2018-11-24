using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dungeon {

    public float Difficulty;

    private Queue<GameObject> monsters;
        
    public Dungeon()
    {
        monsters = new Queue<GameObject>();
    }

    public bool IsAvailable() {
        return monsters.Count > 0;
    }

    public GameObject NextMonster() {
        return monsters.Dequeue();
    }

    public void AddMonster(GameObject monster) {
        monsters.Enqueue(monster);
    }



}
