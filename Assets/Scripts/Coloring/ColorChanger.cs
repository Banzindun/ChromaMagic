using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {

    public Color NewColor;

    public void ChangeColor() {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        NewColor.a = 0.8f;
        //renderer.material.color = NewColor;
        renderer.color = NewColor;
    }
}
