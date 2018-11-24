using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPickable : MonoBehaviour 
{
	public Color ColorValue;
	public float SelectedScale = 1.2f;

	private Vector3 selectedScaleVector = new Vector3(1f,1f,1f);
	private bool selected = false;
	private float animationCounter = 0.0f;

	void Start()
	{
		selectedScaleVector = new Vector3(SelectedScale,SelectedScale,SelectedScale);
		UpdateColor();
	}

	void Update()
	{
		UpdateColor();
		if(selected)
		{
			Spin();
		}
	}

    private void Spin()
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

	public void Select()
	{
		selected = true;
		transform.localScale = selectedScaleVector;
	}

	public void Deselect()
	{
		selected = false;
		animationCounter = 0.0f;
		transform.localScale = Vector3.one;
		transform.rotation = Quaternion.Euler(0, 0, 0);
	}
}
