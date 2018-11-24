using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterGenerator : MonoBehaviour {

    public Colorable[] Colorables;

    public GameObject ColorablePrefab;

    public Mesh MeshColliderMesh;

    private void Start()
    {
        createNewColorableMonster();
    }

    public GameObject createNewColorableObject(ColorableHolder holder) {
        GameObject colorableObject = GameObject.Instantiate(ColorablePrefab);

        // Get a random colorable instance
        ColorableInstance colorableInstance = AddColorableInstance(colorableObject, holder);

        InitializeBaseLayers(colorableInstance, colorableObject);
        InitializeSections(colorableInstance, colorableObject);

        return colorableObject;
    }

    public GameObject createNewColorableMonster() {
        GameObject colorableObject = GameObject.Instantiate(ColorablePrefab);

        // Get a random colorable instance
        ColorableInstance colorableInstance = AddRandomColorableInstance(colorableObject);
        MonsterController monsterController = colorableObject.AddComponent<MonsterController>();
       
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
        
        Sprite[] sectionColorLayers = GetSectionColorLayers(colorableInstance.ColorableHolder.Colorable);
        GameObject[] layerObjects = new GameObject[sectionColorLayers.Length];

        Colorable colorable = colorableInstance.ColorableHolder.Colorable;

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

            // Create sprite renderer
            SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sectionColorLayers[i];
            gameObject.transform.parent = colorableObject.transform;
            
            ColorableSectionInstance sectionInstnace = gameObject.AddComponent<ColorableSectionInstance>();
            sectionInstnace.ColorableSection = colorableInstance.ColorableHolder.Colorable.Sections[i];

            // Collision object
            
            GameObject collisionGameObject = new GameObject();
            collisionGameObject.name = "Collision: " + i;

            // Create sprite renderer
            SpriteRenderer collisionSpriteRenderer = collisionGameObject.AddComponent<SpriteRenderer>();
            collisionSpriteRenderer.sprite = colorableInstance.ColorableHolder.Colorable.Sections[i].SelectionLayer;
            collisionGameObject.transform.parent = gameObject.transform;

            // Create the 2D box collider, add it to Collision Sprite Renderer
            if (i == 0)
            { 
                sectionSelector = gameObject.AddComponent<SectionSelector>();
            }

            BoxCollider2D meshCollider = collisionGameObject.AddComponent<BoxCollider2D>();

            MonsterController monsterController = gameObject.AddComponent<MonsterController>();

            // Change the depth
            Vector3 newPosition = gameObject.transform.localPosition;
            newPosition.z = depth;
            depth--;
            gameObject.transform.localPosition = newPosition;

            layerObjects[i] = gameObject;

            // TODO maybe scale it 
            // TODO the prefab should be probably on the spot where everything should be drawed
        }

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

    public ColorableInstance AddColorableInstance(GameObject obj, ColorableHolder holder)
    {
        ColorableInstance instance = obj.AddComponent<ColorableInstance>();
        instance.ColorableHolder = holder;

        return instance;
    }

    public ColorableHolder CreateRandomColorableHolder() {
        int selected = Random.Range(0, Colorables.Length);

        ColorableHolder holder = new ColorableHolder();
        holder.Colorable = Colorables[selected];

        return holder;            
    }
}
