using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWheel : MonoBehaviour 
{

	public GameObject ColorWheelSelection = null;
	public List<Color> BaseColors = new List<Color>();
	
	void Start () 
	{
		for(int i = transform.childCount - 1; i >= 0 ; --i)
			Destroy(transform.GetChild(i).gameObject);

		RectTransform rect = GetComponent<RectTransform>();
		float currentAngle = 0.0f;
		float angleIncrement = 360f / (float)BaseColors.Count;
		foreach(Color color in BaseColors)
		{
			GameObject selection = GenerateColorWheelSelection(color);
			selection.transform.localPosition = new Vector3(0, rect.sizeDelta.x / 2f, 0);

			selection.transform.localPosition = Quaternion.Euler(0, 0, currentAngle) * selection.transform.localPosition;

			currentAngle += angleIncrement;
		}	
	}

    private GameObject GenerateColorWheelSelection(Color color)
    {
		GameObject selection = Instantiate(ColorWheelSelection, transform);
		ColorPickable pickable = selection.GetComponent<ColorPickable>();
		pickable.ColorValue = color;
		
		return selection;
    }
}
