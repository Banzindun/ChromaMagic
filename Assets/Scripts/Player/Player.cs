using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    // Player's colorable instance, wher the player health is represented
    public ColorableInstance ColorableInstance;

    public int CurrentHealth;

	// Use this for initialization
	void Start () {
        
        CurrentHealth = ColorableInstance.SectionHolders.Length;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
