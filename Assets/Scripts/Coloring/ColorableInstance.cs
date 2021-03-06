﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorableInstance : MonoBehaviour {
    public enum LAYER_TYPE {
        OUTLINE,
        DETAILS,
        EFFECTS,
        COLORS
    }

    public Colorable Colorable;

    public ColorSet InstanceColorSet;

    public ColorableSectionInstance[] SectionHolders;

    public SpriteRenderer OutlineLayer;

    public SpriteRenderer DetailsLayer;

    public SpriteRenderer EffectsLayer;
    public Timer drawingTimer = null;

    public int ColoredSections = 0;

    public ColorableInstance Sibling;

    public ColorSet GetRandomColorSet() {
        ColorSet[] colorSets = Colorable.ColorSets;
        int index = UnityEngine.Random.Range(0, colorSets.Length);
        return colorSets[index];
    }

    public void InitializeLayers(GameObject[] gameObjects) {
        OutlineLayer = GetSpriteRenderer(gameObjects[0]);
        DetailsLayer = GetSpriteRenderer(gameObjects[1]);
        EffectsLayer = GetSpriteRenderer(gameObjects[2]);
    }

    public void DisableColliders()
    {
        ColorableSectionInstance[] instances = GetComponentsInChildren<ColorableSectionInstance>();
        
        for (int i = 0; i < instances.Length; i++)
        {
            BoxCollider2D[] colliders = instances[i].GetComponentsInChildren<BoxCollider2D>();
            for (int j = 0; j < colliders.Length; j++)
            {
                colliders[j].enabled = false;
            }
        }
    }

    private SpriteRenderer GetSpriteRenderer(GameObject gameObject) {
        if (gameObject == null)
            return null;

        return gameObject.GetComponent<SpriteRenderer>();
    }

    public void MakeModel()
    {
        TurnOnLayer(LAYER_TYPE.OUTLINE);
        //TurnOffLayer(LAYER_TYPE.DETAILS);
        //TurnOffLayer(LAYER_TYPE.EFFECTS);

        TurnOnLayer(LAYER_TYPE.DETAILS);
        TurnOnLayer(LAYER_TYPE.EFFECTS);

        for (int i = 0; i < SectionHolders.Length; i++)
        {
            SetColor(i, SectionHolders[i].FinalColor);
        }

        TurnOnLayer(LAYER_TYPE.COLORS);
    }


    public void MakeColoredModel()
    {
        TurnOnLayer(LAYER_TYPE.OUTLINE);
        //TurnOffLayer(LAYER_TYPE.DETAILS);
        //TurnOffLayer(LAYER_TYPE.EFFECTS);

        TurnOffLayer(LAYER_TYPE.DETAILS);
        TurnOffLayer(LAYER_TYPE.EFFECTS);

        TurnOffLayer(LAYER_TYPE.COLORS);
    }


    public void MakeExtremelyRealistic()
    {
        TurnOnLayer(LAYER_TYPE.DETAILS);
        TurnOnLayer(LAYER_TYPE.EFFECTS);

        TurnOnLayer(LAYER_TYPE.COLORS);
    }

    public void InitializeSectionInstances(ColorableSectionInstance[] colorableSectionInstance)
    {
        SectionHolders = colorableSectionInstance;

        // Get random color set and assign the colors to the sections
        ColorSet randomColorSet = GetRandomColorSet();
        InstanceColorSet = randomColorSet;
        for (int i = 0; i < SectionHolders.Length; i++)
        {
            SectionHolders[i].FinalColor = randomColorSet.colors[i];
            SectionHolders[i].index = i;
        }
    }

    // Set color
    public void SetColor(int index, Color color) {
        SectionHolders[index].SelectedColor = color;
        SectionHolders[index].UseSelectedColor();

        if (Sibling != null) {
            Sibling.SetColor(index, color);
        }
    }

    public void TurnOffLayer(LAYER_TYPE type) {
        SwitchLayer(type, false);
    }

    public void TurnOnLayer(LAYER_TYPE type) {
        SwitchLayer(type, true);
    }

    private void SwitchLayer(LAYER_TYPE type, bool bol) {
        switch (type) {
            case LAYER_TYPE.DETAILS:
                DetailsLayer.enabled = bol;
                break;
            case LAYER_TYPE.EFFECTS:
                EffectsLayer.enabled = bol;
                break;
            case LAYER_TYPE.OUTLINE:
                OutlineLayer.enabled = bol;
                break;
            case LAYER_TYPE.COLORS:
                for (int i = 0; i < SectionHolders.Length; i++)
                {
                    SectionHolders[i].spriteRenderer.enabled = bol;
                }
                break;
            default:
                break;
        }

        if (Sibling != null)
            Sibling.SwitchLayer(type, bol);
    }

    internal void TurnOnLayer(LAYER_TYPE type, int index)
    {
        if (type == LAYER_TYPE.COLORS) {
            SectionHolders[index].spriteRenderer.enabled = true;
        }
    }

    public void CalculateScore(int coloredSections) {
        int hits = getHits();

        GameController gameController = GameController.Instance;

        float scoreMultiplier = gameController.Dungeons[gameController.CurrentDungeonIndex - 1].ScoreMultiplier;

        if (hits == 0)
            GameController.Instance.LooseHealth();
        else 
        {
            if(coloredSections != SectionHolders.Length)
                GameController.Instance.LooseHealth();
            GameController.Instance.Score += (hits + GameController.Instance.timer.PercentLeft) * scoreMultiplier;
        }
    }

    private int getHits() {
        int hits = 0;

        for (int i = 0; i < SectionHolders.Length; i++)
        {
            ColorableSectionInstance colorableSectionInstance = SectionHolders[i];

            Color finalColor = colorableSectionInstance.FinalColor;
            Color selectedColor = colorableSectionInstance.SelectedColor;

            if (finalColor.r == selectedColor.r && finalColor.g == selectedColor.g && finalColor.b == selectedColor.b) {
                hits++;
            } else {
                // Maybe later
            }
        }

        return hits;
    }        
}
