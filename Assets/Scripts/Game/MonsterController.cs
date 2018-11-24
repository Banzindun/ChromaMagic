using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour {
    private ColorableInstance colorableInstance;



    // Use this for initialization
    void Start() {
        colorableInstance = GetComponent<ColorableInstance>();


    }

    public void Color(int index, Color color) {
        ColorableSectionInstance holder = colorableInstance.SectionHolders[index];
        holder.SelectedColor = color;

        holder.UseSelectedColor();

        int score = holder.IsColorRealistic();
        // TODO do something with the color

        //
        colorableInstance.ColoredSections += 1;
    }
}
