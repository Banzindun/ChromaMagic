using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SectionSelector : MonoBehaviour 
{
    public bool IsFinishedSelecting = false;
    public ColorableSectionInstance SelectedColorableSectionInstance = null;
    public float raycastCooldown = 0.8f;

    private float lastRaycast;

    public void StartSelecting()
    {
        IsFinishedSelecting = false;
    }

    public void StopSelecting()
    {
        IsFinishedSelecting = true;
    }

    private void Update()
    {
        if(IsFinishedSelecting == false)
        {
            DoRaycast();
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
        if(Input.GetMouseButton(MouseButton.Right))
        {
            IsFinishedSelecting = true;
            SelectedColorableSectionInstance = colorableInstance;
        }
    }
}
