using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizeText : MonoBehaviour
{
    public int len;
    public Color color = Color.red;
    public bool bold = false;
    const string glyphs= "abcdefghijklmnopqrstuvwxyz";
    const string lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. ";

    void Start()
    {
        System.Random rand = new System.Random(0);
        if(bold)
            this.GetComponent<UnityEngine.UI.Text>().text += "<b>";
        for(int i=0; i<len; i++)
        {
            // this.GetComponent<UnityEngine.UI.Text>().text += glyphs[rand.Next(0, glyphs.Length)];
            this.GetComponent<UnityEngine.UI.Text>().text += lorem[i];
        }
        if(bold)
            this.GetComponent<UnityEngine.UI.Text>().text += "</b>";
        color.a = 255;
        this.gameObject.GetComponent<UnityEngine.UI.Text>().color = color;
    }
}
