using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour {

    public GameObject TextHolder;

    private Text text;

    private void Start()
    {
        text = TextHolder.GetComponent<Text>();
    }

    public void UpdateScore() {
        float score = GameController.Instance.Score;
        text.text = "" + (int)score;
    }

}

