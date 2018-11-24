using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorableInstance : MonoBehaviour {

    public ColorableHolder ColorableHolder;

    public ColorableSectionHolder[] SectionHolders;

    public SpriteRenderer OutlineLayer;

    public SpriteRenderer DetailsLayer;

    public SpriteRenderer EffectsLayer;

    public SpriteRenderer[] AdditionalLayers;

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

    public void InitializeSectionHolders() {
        int size = ColorableHolder.Colorable.Sections.Length;

        ColorableSection[] sections = ColorableHolder.Colorable.Sections;

        SectionHolders = new ColorableSectionHolder[sections.Length];

        for (int i = 0; i < sections.Length; i++)
        {
            SectionHolders[i] = new ColorableSectionHolder();

            SectionHolders[i].ColorableSection = sections[i];

            // Initialize the default color of the holder
            SectionHolders[i].SelectRandomColors();
        }
    }

    public void InitializeColorLayers(GameObject[] gameObjects) {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            SectionHolders[i].spriteRenderer = GetSpriteRenderer(gameObjects[i]);
        }
    }
}
