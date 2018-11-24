﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {

    public Color NewColor;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            ChangeColor();
    }

    public void ChangeColor() {
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        NewColor.a = 255;
        //renderer.material.color = NewColor;
        renderer.color = NewColor;
    }
}