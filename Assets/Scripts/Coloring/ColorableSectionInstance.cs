using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorableSectionInstance : MonoBehaviour {

    public ColorableSection ColorableSection;

    public ColorSet MonsterColorSet;

    public Color SelectedColor;

    public Color FinalColor;

    public bool Colored;

    // Index from the Colorable instance
    public int index;

    // Reference to sprite renderer holding the sprite
    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UseSelectedColor() {
        ColorChanger colorChanger = spriteRenderer.GetComponent<ColorChanger>();
        if (SelectedColor == null)
        {
            Debug.Log("No color selected.");
            colorChanger.NewColor = Color.black;
        }
        else {
            colorChanger.NewColor = SelectedColor;
        }

        colorChanger.ChangeColor();

        Colored = true;
    }

    public void UseFinalColor()
    {
        ColorChanger colorChanger = spriteRenderer.GetComponent<ColorChanger>();
        if (SelectedColor == null)
        {
            Debug.Log("No color selected.");
            colorChanger.NewColor = Color.black;
        }
        else
        {
            colorChanger.NewColor = FinalColor;
        }

        colorChanger.ChangeColor();

        Colored = true;
    }
}
