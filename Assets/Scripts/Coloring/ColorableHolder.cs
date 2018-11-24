using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorableHolder{

    public Colorable Colorable;

    [Tooltip("Index to the base color from the Colorable.")]
    public int BaseColorIndex;

    [Tooltip("Index to the correct color from the Colorable.")]
    public int CorrectColorIndex;



}
