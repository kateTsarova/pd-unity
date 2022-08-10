using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddLoremText : MonoBehaviour
{
    public int len;
    public Color color = Color.red;
    public bool bold = false;
    const string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. ";

    void Start()
    {
        System.Random rand = new System.Random(0);
        if(bold)
            this.GetComponent<UnityEngine.UI.Text>().text += "<b>";
        this.GetComponent<UnityEngine.UI.Text>().text += lorem.Substring(0, len);
        if(bold)
            this.GetComponent<UnityEngine.UI.Text>().text += "</b>";
        color.a = 255;
        this.gameObject.GetComponent<UnityEngine.UI.Text>().color = color;
    }
}
