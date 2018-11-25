using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LabelCreator : MonoBehaviour {

    public static LabelCreator Instance = null;

    private Text text;

    private void OnEnable()
    {
        Instance = this;
    }

    private void Update()
    {
        if (text.enabled && GameController.Instance.InterruptTime <= 0) {
            text.enabled = false;
        }
    }

    void Start() {
        text = gameObject.GetComponent<Text>();
    }

    public void CreateLabelEvent(string label, float time)
    {
        GameController.Instance.Interrupt(time);
        text.text = label;
        text.enabled = true;
    }

    public void CreateLabelEvent(string label, float time, Color color)
    {
        GameController.Instance.Interrupt(time);
        text.text = label;
        text.color = color;
        text.enabled = true;
    }
}

