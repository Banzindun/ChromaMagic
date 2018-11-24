using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorableSectionHolder {

    public ColorableSection ColorableSection;

    public Color BaseColor;

    public Color FinalColor;

    // Reference to sprite renderer holding the sprite
    public SpriteRenderer spriteRenderer;

    public void SelectRandomColors() {
        BaseColor = ColorableSection.GetRandomBaseColor();
        FinalColor = ColorableSection.GetRandomFinalColor();
    }
	
}
