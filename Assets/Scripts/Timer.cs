using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Timer")]
public class Timer : ScriptableObject 
{	
	public bool IsCountingDown = false;
	public float CountdownTime = 10f;
	public float TimeLeft = 10f;
	public bool IsNoTimeLeft = false;
	public float PercentLeft = 1f;


	public void StartCountdown(float time)
	{
		CountdownTime = time;
		TimeLeft = time;
		IsNoTimeLeft = false;
		IsCountingDown = true;
	}

	public void Update () 
	{
		if(IsCountingDown)
		{
			TimeLeft -= Time.deltaTime;
			PercentLeft = TimeLeft / CountdownTime;
			if(TimeLeft <= 0f)
			{
				IsNoTimeLeft = true;
				IsCountingDown = false;
				TimeLeft = 0f;
			}
		}
	}
}
