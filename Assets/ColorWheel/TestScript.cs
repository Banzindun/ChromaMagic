using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestScript : MonoBehaviour 
{
	public ScriptableColor CurrentlySelectedColor = null;
	private Image image = null;

	void Start()
	{
		image = GetComponent<Image>();
	}

	void Update()
	{
		image.color = CurrentlySelectedColor.ColorValue;
	}
}
