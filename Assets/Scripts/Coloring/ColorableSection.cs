using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorableSections.asset", menuName = " Colorables / Section")]
public class ColorableSection : ScriptableObject {

    public Sprite ColorLayer;

    // Random colors generatin
    // TODO Jakub should fill this out

    [Tooltip("Base colors that this object can have.")]
    public Color[] BaseColors;

    [Tooltip("Final colors that this object can have.")]
    public Color[] FinalColors;


    public Color GetRandomBaseColor() {
        int index = Random.Range(0, BaseColors.Length);
        return BaseColors[index];
    }

    public Color GetRandomFinalColor()
    {
        int index = Random.Range(0, FinalColors.Length);
        return FinalColors[index];200
    }

}
