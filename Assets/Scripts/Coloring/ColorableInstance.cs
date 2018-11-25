using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorableInstance : MonoBehaviour {
    public Colorable Colorable;

    public ColorableSectionInstance[] SectionHolders;

    public SpriteRenderer OutlineLayer;

    public SpriteRenderer DetailsLayer;

    public SpriteRenderer EffectsLayer;

    public SpriteRenderer[] AdditionalLayers;

    public int ColoredSections = 0;

    void Start()
    {
        
    }


    public ColorSet GetRandomColorSet(){
        ColorSet[] colorSets = Colorable.ColorSets;
        int index = UnityEngine.Random.Range(0, colorSets.Length);
        return colorSets[index];
    }

    public void InitializeLayers(GameObject[] gameObjects) {
        OutlineLayer = GetSpriteRenderer(gameObjects[0]);
        DetailsLayer = GetSpriteRenderer(gameObjects[1]);
        EffectsLayer = GetSpriteRenderer(gameObjects[2]);

        for (int i = 0;  i < gameObjects.Length - 3; i++){
            AdditionalLayers[3 + i] = GetSpriteRenderer(gameObjects[i]);
        }        
    }

    private SpriteRenderer GetSpriteRenderer(GameObject gameObject) {
        if (gameObject == null)
            return null;

        return gameObject.GetComponent<SpriteRenderer>();
    }

    public void MakeModel()
    {
        OutlineLayer.enabled = true;
        DetailsLayer.enabled = false;
        EffectsLayer.enabled = false;

        for (int i = 0; i < SectionHolders.Length; i++)
        {
            SectionHolders[i].UseFinalColor();
        }

        ShowSectionSpriteRenderers();
    }


    public void MakeColoredModel()
    {
        OutlineLayer.enabled = true;
        DetailsLayer.enabled = false;
        EffectsLayer.enabled = false;
        HideSectionSpriteRenderers();
    }


    public void MakeExtremelyRealistic()
    {
        DetailsLayer.enabled = true;
        EffectsLayer.enabled = true;

        ShowSectionSpriteRenderers();
    }

    public void HideSectionSpriteRenderers()
    {
        for (int i = 0; i < SectionHolders.Length; i++)
        {
            SpriteRenderer spriteRenderer = SectionHolders[i].GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }
    }

    public void ShowSectionSpriteRenderers()
    {
        for (int i = 0; i < SectionHolders.Length; i++)
        {
            SpriteRenderer spriteRenderer = SectionHolders[i].GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = true;
        }
    }

    public void InitializeSectionInstances(ColorableSectionInstance[] colorableSectionInstance)
    {
        SectionHolders = colorableSectionInstance;

        // Get random color set and assign the colors to the sections
        ColorSet randomColorSet = GetRandomColorSet();
        for (int i = 0; i < SectionHolders.Length; i++)
        {
            SectionHolders[i].FinalColor = randomColorSet.colors[i];
        }
    }
}
