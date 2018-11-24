using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorableInstance : MonoBehaviour {

    public ColorableHolder ColorableHolder;

    public ColorableSectionInstance[] SectionHolders;

    public SpriteRenderer OutlineLayer;

    public SpriteRenderer DetailsLayer;

    public SpriteRenderer EffectsLayer;

    public SpriteRenderer[] AdditionalLayers;

    public int ColoredSections = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //if (Input.GetMouseButtonDown(1)) {
        //    ColorChanger colorChanger = GetComponent<ColorChanger>();
        //    colorChanger.ChangeColor();
        //}
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
    }

    public void MakeColoredModel()
    {
        OutlineLayer.enabled = true;

        for (int i = 0; i < SectionHolders.Length; i++)
        {
            SectionHolders[i].UseSelectedColor();
        }
    }

    public void MakeExtremelyRealistic()
    {
        DetailsLayer.enabled = true;
        EffectsLayer.enabled = true;
    }
}
