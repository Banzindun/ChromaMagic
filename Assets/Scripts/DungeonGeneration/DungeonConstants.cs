using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GenerationConstants.asset", menuName = "Generation / DungeonConstants")]
public class DungeonConstants : ScriptableObject {

    public int NumberOfMonsters;

    // Modifies time
    public float DifficultyModifier;

    public Colorable[] Colorables;

    public Sprite background;
}
