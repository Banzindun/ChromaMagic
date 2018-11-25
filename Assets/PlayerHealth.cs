using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

    public GameObject PlayerHealthObject;

    public GameObject HealthBarPrefab;

    public Color[] HealthColors;

    private Stack<GameObject> HealthBars;

    public int BaseHealth = 4;

    public int HealthOffset = 5;

    // Use this for initialization
    void Start() {
        HealthBars = new Stack<GameObject>();
        float yOffset = 0f;

        for (int i = 0; i < BaseHealth; i++)
        {
            GameObject healthBar = GameObject.Instantiate(HealthBarPrefab);
            healthBar.transform.parent = PlayerHealthObject.transform;
            healthBar.transform.localPosition = new Vector3(0, yOffset, 0);

            healthBar.GetComponent<Image>().color = HealthColors[i]; // Set the color

            HealthBars.Push(healthBar);
            yOffset += HealthOffset;
        }
    }

    public void LooseHealth() {
        SoundManager.Instance.PlaySound("lostHealth");
        Destroy(HealthBars.Pop());
    }

    public bool IsDead() {
        return HealthBars.Count == 0;
    }
}
