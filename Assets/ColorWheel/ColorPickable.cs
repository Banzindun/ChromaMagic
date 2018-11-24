using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickable : MonoBehaviour 
{
	public ScriptableColor CurrentSelectedColor = null;
	public Color ColorValue;
	public float HoverScale = 1.2f;

	private Vector3 hoverScaleVector = new Vector3(1f,1f,1f);
	private bool hovered = false;
	private float animationCounter = 0.0f;

	void Start()
	{
		hoverScaleVector = new Vector3(HoverScale,HoverScale,HoverScale);
		UpdateColor();
	}

	void Update()
	{
		UpdateColor();
		if(hovered)
		{
			SpinAnimation();
		}
	}

    private void SpinAnimation()
    {
		animationCounter += Time.deltaTime;
		float rotation = Mathf.Rad2Deg * Mathf.Sin(animationCounter);
		transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    public void UpdateColor()
	{
		var selectableImage = GetComponent<Image>();
		if(selectableImage != null)
			selectableImage.color = ColorValue;
	}

	public void OnMouseEnter()
	{
		hovered = true;
		animationCounter = 0.0f;
		transform.localScale = hoverScaleVector;
	}

	public void OnMouseExit()
	{
		hovered = false;
		transform.localScale = Vector3.one;
	}

	public void PickColor()
	{
		CurrentSelectedColor.ColorValue = ColorValue;
	}
}
