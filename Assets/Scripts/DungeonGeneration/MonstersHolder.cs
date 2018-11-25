using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHolder {
    public GameObject SceneMonster;

    public GameObject MonsterOutlined;

    public GameObject MonsterModel;

    public void Init(GameObject gameObject) {
        SceneMonster = gameObject;

        MonsterOutlined = GameObject.Instantiate(gameObject);
        MonsterModel = GameObject.Instantiate(gameObject);
    }

    public void Deactivate() {
        MonsterOutlined.SetActive(false);
        MonsterModel.SetActive(false);
    }

    public void Activate()
    {
        MonsterOutlined.SetActive(true);
        MonsterModel.SetActive(true);
    }
}
