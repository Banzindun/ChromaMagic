using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SceneGenerator : MonoBehaviour {

    public Image BackgroundSlot;

    public GameObject ModelSlot;

    public GameObject OutlinedModelSlot;


    public void Start()
    {
        GameController.Instance.SceneGenerator = this;
    }

    public void AssignDungeonBackground(Sprite background) {
        BackgroundSlot.sprite = background;
        // resize??
    }


}
