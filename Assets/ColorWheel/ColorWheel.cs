using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWheel : MonoBehaviour 
{
	public GameObject ColorWheelSelectionPrefab = null;
	public ScriptableColor CurrentSelectedColor = null;
	public List<Color> BaseColors = new List<Color>();
	public ColorPickable selected = null;
	private List<ColorPickable> pickables = new List<ColorPickable>();
	
	void Start () 
	{
		Show();
	}

	public void Show()
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
		GameObject selection = Instantiate(ColorWheelSelectionPrefab, transform);
		ColorPickable pickable = selection.GetComponent<ColorPickable>();
		pickable.ColorValue = color;
		pickables.Add(pickable);
		
		return selection;
    }

	void Update()
	{
		if(Input.GetMouseButton(MouseButton.Right))
		{
			SelectColor();
		}
		else
		{
			if(selected != null)
			{
				selected.Deselect();
				selected = null;
			}
		}
	}

    private void SelectColor()
    {
		selected = pickables[0];
		CurrentSelectedColor.ColorValue = selected.ColorValue;
		selected.Select();
    }
}
