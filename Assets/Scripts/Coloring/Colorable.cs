using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Colorables.asset", menuName = " Colorables / Colorable")]
public class Colorable : ScriptableObject {

    public Sprite OutlineLayer;

    public Sprite DetailsLayer;

    public Sprite EffectsLayer;

    public Sprite[] AdditionalLayers;
    
    public float BaseTimer;

    public ColorableSection[] Sections;

    public int GetLayersCount() {
        int count = 0;

        if (OutlineLayer != null)
            count++;

        if (DetailsLayer != null)
            count++;

        if (EffectsLayer != null)
            count++;

        count += AdditionalLayers.Length;

        return count;
    }

    public Sprite[] GetAllLayers() {
        Sprite[] sprites = new Sprite[ 3 + AdditionalLayers.Length];

        sprites[0] = OutlineLayer;
        sprites[1] = DetailsLayer;
        sprites[2] = EffectsLayer;

        for (int i = 0; i < AdditionalLayers.Length; i++)
        {
            sprites[ 3 + i] = AdditionalLayers[i];
        }

        return sprites;
    }

    private bool InsertSprite(Sprite[] sprites, Sprite sprite, int index) {
        if (sprite != null)
        {
            sprites[index] = sprite;
            return true;
        }

        return false;
    }
}
