using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PressableTextController : MonoBehaviour {

    public Color Color;

    private Color OldColor;

    private Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}

    public void OnMouseEnter()
    {
        OldColor = text.color;
        text.color = Color;
    }

    public void OnMouseExit()
    {
        text.color = OldColor;
    }
}
