using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour 
{
	public Timer ColoringTimer = null;
	private Image bar = null;
    private Vector3 originalScale;

    void Start () 
	{
		bar = GetComponent<Image>();
		originalScale = bar.gameObject.transform.localScale;
	}
	
	void Update () 
	{
		if(ColoringTimer.IsCountingDown)
		{
			float scale = ColoringTimer.PercentLeft;
			bar.gameObject.transform.localScale = new Vector3(scale * originalScale.x, originalScale.y, originalScale.z); 
		}
		else
		{
			bar.gameObject.transform.localScale = 0f * originalScale;
		}
	}
}
