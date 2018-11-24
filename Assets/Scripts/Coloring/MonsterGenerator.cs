using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterGenerator : MonoBehaviour {

    public Colorable[] Colorables;

    public GameObject ColorablePrefab;

	// Use this for initialization
	void Start () {
        createNewColorableObject();
	}
	
	
	void Update () {
        
        

	}

    public GameObject createNewColorableObject() {
        GameObject colorableObject = GameObject.Instantiate(ColorablePrefab);

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
        colorableInstance.InitializeSectionHolders();

        GameObject[] colorLayersObjects = AddSectionColorLayers(colorableInstance, colorableObject, 10);
        colorableInstance.InitializeColorLayers(colorLayersObjects);
    }

    private GameObject[] AddLayers(ColorableInstance colorableInstance, GameObject originalObject, int startDepth)
    {
        Colorable colorable = colorableInstance.ColorableHolder.Colorable;
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
            SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = layers[i];
            gameObject.transform.parent = originalObject.transform;

            // Change the depth
            Vector3 newPosition = gameObject.transform.localPosition;
            newPosition.z = depth;
            depth--;
            gameObject.transform.localPosition = newPosition;

            layerObjects[i] = gameObject;

            // TODO maybe scale it 
            // TODO the prefab should be probably on the spot where everything should be drawed
        }

        return layerObjects;       
    }

    private GameObject[] AddSectionColorLayers(ColorableInstance colorableInstance, GameObject originalObject, int startDepth)
    {
        Sprite[] sectionColorLayers = GetSectionColorLayers(colorableInstance.ColorableHolder.Colorable);
        GameObject[] layerObjects = new GameObject[sectionColorLayers.Length];

        Colorable colorable = colorableInstance.ColorableHolder.Colorable;
        ColorableSectionHolder[] sectionHolders = colorableInstance.SectionHolders;
        
        // Z axis depth
        int depth = startDepth;

        for (int i = 0; i < sectionColorLayers.Length; i++)
        {
            if (sectionColorLayers[i] == null)
            {
                layerObjects[i] = null;
                continue;
            }

            GameObject gameObject = new GameObject();
            SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sectionColorLayers[i];
            gameObject.transform.parent = originalObject.transform;

            ColorChanger colorChanger = gameObject.AddComponent<ColorChanger>();
            colorChanger.NewColor = sectionHolders[i].BaseColor;
            colorChanger.ChangeColor();

            // Change the depth
            Vector3 newPosition = gameObject.transform.localPosition;
            newPosition.z = depth;
            depth--;
            gameObject.transform.localPosition = newPosition;

            layerObjects[i] = gameObject;

            // TODO maybe scale it 
            // TODO the prefab should be probably on the spot where everything should be drawed
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
        instance.ColorableHolder = CreateRandomColorableHolder();

        return instance;
    }

    public ColorableHolder CreateRandomColorableHolder() {
        int selected = Random.Range(0, Colorables.Length);

        ColorableHolder holder = new ColorableHolder();
        holder.Colorable = Colorables[selected];

        return holder;            
    }
}
