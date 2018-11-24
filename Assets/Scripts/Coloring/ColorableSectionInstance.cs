using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorableSectionInstance : MonoBehaviour {

    public ColorableSection ColorableSection;

    public Color SelectedColor;

    public Color FinalColor;

    public bool Colored;

    // Reference to sprite renderer holding the sprite
    public SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (ColorableSection != null)
            FinalColor = ColorableSection.GetRandomFinalColor();
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

        Colored = true;
    }

    // Should return level of realism
    public int ColorRealistic()
    {
        return 0;
    }
}
