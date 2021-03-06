﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterGenerator {

    // Constants to generate stuff from.
    public DungeonConstants dungeonConstants;

    public MonsterGenerator(DungeonConstants dungeonConstants){
        this.dungeonConstants = dungeonConstants;
    }

    public MonsterHolder CreateMonsters() {
        MonsterHolder monsterHolder = new MonsterHolder();
        monsterHolder.Init(createNewMonster());

        ColorableInstance monsterOutlinedColorable = monsterHolder.MonsterOutlined.GetComponent<ColorableInstance>();
        ColorableInstance SceneMonsterColorable = monsterHolder.SceneMonster.GetComponent<ColorableInstance>();
        monsterOutlinedColorable.Sibling = SceneMonsterColorable;

        SceneMonsterColorable.DisableColliders();
        monsterHolder.MonsterModel.GetComponent<ColorableInstance>().DisableColliders();

        return monsterHolder;
    }
    
    // Creates new monster from dungeonConstants
    private GameObject createNewMonster() {
        GameObject colorableObject = new GameObject();

        // Get a random colorable instance
        ColorableInstance colorableInstance = AddRandomColorableInstance(colorableObject);
       
        InitializeBaseLayers(colorableInstance, colorableObject);
        InitializeSections(colorableInstance, colorableObject);

        return colorableObject;
    }

    private void InitializeBaseLayers(ColorableInstance colorableInstance, GameObject colorableObject)
    {
        GameObject[] layerObjects = AddLayers(colorableInstance, colorableObject, 0);
        colorableInstance.InitializeLayers(layerObjects);
    }

    private void InitializeSections(ColorableInstance colorableInstance, GameObject colorableObject)
    {
        int depth = 10;
        
        Sprite[] sectionColorLayers = GetSectionColorLayers(colorableInstance.Colorable);
        GameObject[] layerObjects = new GameObject[sectionColorLayers.Length];

        Colorable colorable = colorableInstance.Colorable;

        List<ColorableSectionInstance> sectionInstances = new List<ColorableSectionInstance>();

        SectionSelector sectionSelector = null;

        for (int i = 0; i < sectionColorLayers.Length; i++)
        {
            if (sectionColorLayers[i] == null)
            {
                layerObjects[i] = null;
                continue;
            }

            GameObject gameObject = new GameObject();
            gameObject.name = "Section: " + i;

            gameObject.AddComponent<ColorChanger>();

            // Create the 2D box collider, add it to Collision Sprite Renderer
            /*if (i == 0)
            {
                sectionSelector = gameObject.AddComponent<SectionSelector>();
            }*/

            // Create sprite renderer
            SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sectionColorLayers[i];
            gameObject.transform.parent = colorableObject.transform;
            
            ColorableSectionInstance sectionInstance = gameObject.AddComponent<ColorableSectionInstance>();
            sectionInstance.ColorableSection = colorableInstance.Colorable.Sections[i];
            sectionInstances.Add(sectionInstance);

            // Collision object

            GameObject collisionGameObject = new GameObject();
            collisionGameObject.name = "Collision: " + i;

            // Create sprite renderer
            SpriteRenderer collisionSpriteRenderer = collisionGameObject.AddComponent<SpriteRenderer>();
            collisionSpriteRenderer.sprite = colorableInstance.Colorable.Sections[i].SelectionLayer;
            collisionGameObject.transform.parent = gameObject.transform;
            collisionSpriteRenderer.enabled = false;
            
            BoxCollider2D meshCollider = collisionGameObject.AddComponent<BoxCollider2D>();

            // Change the depth
            Vector3 newPosition = gameObject.transform.localPosition;
            newPosition.z = depth;
            depth--;
            gameObject.transform.localPosition = newPosition;

            layerObjects[i] = gameObject;
        }
        
        colorableInstance.InitializeSectionInstances(sectionInstances.ToArray());
    }

    private GameObject[] AddLayers(ColorableInstance colorableInstance, GameObject originalObject, int startDepth)
    {
        Colorable colorable = colorableInstance.Colorable;
        // Get all layers
        Sprite[] layers = colorable.GetAllLayers();
        GameObject[] layerObjects = new GameObject[layers.Length];

        // Z axis depth
        int depth = startDepth;

        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i] == null)
            {
                layerObjects[i] = null;
                continue;
            }

            GameObject gameObject = new GameObject();
            gameObject.name = "Layer" + i;

            // Add the sprite renderer
            SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = layers[i];
            gameObject.transform.parent = originalObject.transform;

            // Change the depth
            Vector3 newPosition = gameObject.transform.localPosition;
            newPosition.z = depth;
            depth--;
            gameObject.transform.localPosition = newPosition;

            layerObjects[i] = gameObject;
        }

        return layerObjects;       
    }


    private Sprite[] GetSectionColorLayers(Colorable colorable)
    {
        ColorableSection[] sections = colorable.Sections;
        Sprite[] sprites = new Sprite[sections.Length];

        for (int i = 0; i < sections.Length; i++)
        {
            sprites[i] = sections[i].ColorLayer;
        }

        return sprites;
    }

    public ColorableInstance AddRandomColorableInstance(GameObject obj) {
        ColorableInstance instance = obj.AddComponent<ColorableInstance>();
        instance.Colorable = GetRandmColorable();

        return instance;
    }

    public ColorableInstance AddColorableInstance(GameObject obj, Colorable colorable)
    {
        ColorableInstance instance = obj.AddComponent<ColorableInstance>();
        instance.Colorable = colorable;

        return instance;
    }

    public Colorable GetRandmColorable() {
        int selected = Random.Range(0, dungeonConstants.Colorables.Length);

        return dungeonConstants.Colorables[selected];
    }
}
