using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewScript : MonoBehaviour {

    private void Update()
    {
        RaycastHit2D[] hits;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        hits = Physics2D.GetRayIntersectionAll(ray, 100.0f);   
        Debug.Log(hits.Length);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                //Debug.Log(hits[i].point);

                Collider2D collider = hits[i].collider;
                SpriteRenderer spriteRenderer = collider.GetComponent<SpriteRenderer>();

                Texture2D currentTexture = spriteRenderer.sprite.texture;
                //Color currentColor = currentTexture.GetPixel((int) hits[i].point.x, (int) hits[i].point.y);

                Vector2 uv;
                uv.x = (hits[i].point.x - hits[i].collider.bounds.min.x) / hits[i].collider.bounds.size.x;
                uv.y = (hits[i].point.y - hits[i].collider.bounds.min.y) / hits[i].collider.bounds.size.y;
                // Paint it red
                uv.x *= currentTexture.width;
                uv.y *= currentTexture.height;
                Color currentColor = currentTexture.GetPixel((int)(uv.x), (int)(uv.y));

                if (currentColor.r == 0 && currentColor.g == 0 && currentColor.b == 0) {
                    Debug.Log("Hit: " + collider.name);
                }

                //Debug.Log(uv);
                //Debug.Log(currentColor);

               
            }

        }
    }

   
}
