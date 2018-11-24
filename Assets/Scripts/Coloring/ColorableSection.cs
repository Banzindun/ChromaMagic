using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorableSections.asset", menuName = " Colorables / Section")]
public class ColorableSection : ScriptableObject {

    public Sprite ColorLayer;

    public Sprite SelectionLayer;

    [Tooltip("Final colors that this object can have.")]
    public Color[] FinalColors;

    public Color GetRandomFinalColor()
    {
        int index = Random.Range(0, FinalColors.Length);
        return FinalColors[index];
    }



}
