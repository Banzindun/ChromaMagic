using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorWheel : MonoBehaviour 
{
	public GameObject ColorWheelSelectionPrefab = null;
	public ScriptableColor CurrentSelectedColor = null;
	public List<Color> BaseColors = new List<Color>();
	public ColorPickable Selected = null;
	public GameObject CursorPrefab = null;
	public bool IsFinishedSelecting = false;
	private List<ColorPickable> pickables = new List<ColorPickable>();
	private Vector3 originalCursorLocation = new Vector3(1,1,1);
	private GameObject cursor = null;
	public RectTransform rect = null;
	
	void Start () 
	{
		if(BaseColors.Count == 0)
			throw new UnityException("ColorWheel does not have any colors set");
		rect = GetComponent<RectTransform>();
		Setup();
	}

	public void InitializeColorPalette(List<Color> colors)
	{
		BaseColors = colors;
		rect = GetComponent<RectTransform>();
		Setup();
	}

	public void Setup()
	{
		for(int i = transform.childCount - 1; i >= 0 ; --i)
			Destroy(transform.GetChild(i).gameObject);
		pickables.Clear();

		float currentAngle = 0.0f;
		float angleIncrement = 360f / (float) BaseColors.Count;
		foreach(Color color in BaseColors)
		{
			GameObject selection = GenerateColorWheelSelection(color);
			selection.transform.localPosition = new Vector3(0, rect.sizeDelta.x / 2f, 0);

			selection.transform.localPosition = Quaternion.Euler(0, 0, currentAngle) * selection.transform.localPosition;
			selection.transform.localScale = new Vector3(1f,1f,1f);
			currentAngle += angleIncrement;
		}	

		cursor = Instantiate(CursorPrefab, transform);
		cursor.transform.localPosition = new Vector3(0, rect.sizeDelta.x / 3f, 0);
	}

    private GameObject GenerateColorWheelSelection(Color color)
    {
		GameObject selection = Instantiate(ColorWheelSelectionPrefab, transform);
		ColorPickable pickable = selection.GetComponent<ColorPickable>();
		pickable.ColorValue = color;
		pickables.Add(pickable);
		
		return selection;
    }

	public void Activate(Vector3 originalMousePosition)
	{
        originalCursorLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
        IsFinishedSelecting = false;
		gameObject.SetActive(true);
	}

	public void Deactivate()
	{
		gameObject.SetActive(false);
	}

	void Update()
	{
        if (Input.GetMouseButtonUp(MouseButton.Right))
		{
			IsFinishedSelecting = true;
		}
		if(Input.GetMouseButton(MouseButton.Right))
		{
			SelectColor();
			//Cursor.visible = false;
		}
		else
		{
			//Cursor.visible = true;
			if(Selected != null)
			{
				Selected.Deselect();
				Selected = null;
			}
		}
	}

    private void SelectColor()
    {
        Vector3 mouseDirection = Vector3.Normalize(Camera.main.ScreenToWorldPoint(Input.mousePosition) - originalCursorLocation);
        
        //Vector3 mouseDirection = Vector3.Normalize(new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"), 0));
		float angle = AngleBetweenVectors(mouseDirection, Vector3.up);
		float from0To360 = angle < 0f ? 360f + angle : angle;
		cursor.transform.localPosition = Quaternion.Euler(0, 0, from0To360) * new Vector3(0, rect.sizeDelta.x / 3f, 0);;

		float angleIncrement = 360f / (float) BaseColors.Count;
		float cursorOffset = angleIncrement / 2f;
		float from0To360Offset = (angle + cursorOffset) < 0f ? 360f + (angle + cursorOffset) : (angle + cursorOffset);
		int position = (int)(from0To360Offset / angleIncrement);
		if(Selected != pickables[position])
		{
			if(Selected != null)
				Selected.Deselect();
			Selected = pickables[position];
			Selected.Select();
			CurrentSelectedColor.ColorValue = Selected.ColorValue;
		}
    }

    private float AngleBetweenVectors(Vector3 v1, Vector3 v2)
    {
		Vector3 v1Perpendiculiar = new Vector3(-v1.y, v1.x);
		float sign = Vector3.Dot(v1Perpendiculiar,v2) < 0 ? 1f : -1f;
		return Vector3.Angle(v1, v2) * sign;
    }
}
