using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SectionSelector : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    
    private float lastRaycast;

    private float raycastCooldown = 0.8f;

    [SerializeField]
    Color currentColor;

    void Start() {

        spriteRenderer = gameObject.GetComponentInParent<SpriteRenderer>();
        //Texture2D texture = spriteRenderer.sprite.texture;       
    }

    private void Update()
    {
        if (Time.time - lastRaycast > raycastCooldown) {
            DoRaycast();

            lastRaycast = Time.time;
        } 
    }

    private void DoRaycast() {

        RaycastHit2D[] hits;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        hits = Physics2D.GetRayIntersectionAll(ray, 100.0f);
        Debug.Log(hits.Length);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                Collider2D collider = hits[i].collider;
                // First check that the collider has not been selected/painted yet

                ColorableSectionInstance colorableSection = collider.GetComponentInParent<ColorableSectionInstance>();
                if (colorableSection == null)
                {
                    Debug.Log("No colorable section.");
                    continue;
                }

                if (!colorableSection.Colored) {
                    SpriteRenderer spriteRenderer = collider.GetComponent<SpriteRenderer>();
                    if (spriteRenderer == null)
                    {
                        Debug.Log("No sprite renderer.");
                        continue;
                    }


                    Texture2D currentTexture = spriteRenderer.sprite.texture;
                    
                    // Get current color
                    Vector2 uv;
                    uv.x = (hits[i].point.x - hits[i].collider.bounds.min.x) / hits[i].collider.bounds.size.x;
                    uv.y = (hits[i].point.y - hits[i].collider.bounds.min.y) / hits[i].collider.bounds.size.y;
                    
                    uv.x *= currentTexture.width;
                    uv.y *= currentTexture.height;

                    Color currentColor = currentTexture.GetPixel((int)(uv.x), (int)(uv.y));

                    if (currentColor.r == 0 && currentColor.g == 0 && currentColor.b == 0) {
                        OnSectionSelect(colorableSection);
                    }

                    //Debug.Log(uv);
                    //Debug.Log(currentColor);
                }
            }
        }
    }

    public void OnSectionSelect(ColorableSectionInstance colorableInstance)
    {
        Debug.Log(colorableInstance.name + " selected.");
    }
}
