using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Runtime.Serialization;

public class InterfaceMenu : MonoBehaviour
{
    public enum Paragraph_type
    {
        SMALL,
        MEDIUM,
        LARGE,
        TEXTFIELD
    }

    public enum Button_color
    {
        GREEN,
        ORANGE,
        RED
    }

    public class Header {
        public int button_count = 0;
        public bool radio_button = false;
        public bool checkbox = false;
        public bool radio_button_active = false;
        public bool checkbox_active = false;
    }

    public class Paragraph {
        public Paragraph_type type;
        public Button_color button_color;
        public Button button;

        public Paragraph(Paragraph_type par_type, Button_color par_color, Button par_button)
        {
            type = par_type;
            button_color = par_color;
            button = par_button;
        }

        public Paragraph(Paragraph_type par_type)
        {
            type = par_type;
        }
    }

    public class ParagraphRow {
        public List<Paragraph> paragraphs = new List<Paragraph>();
    }

    public class PageInterface {
        public Header header = new Header();
        public ParagraphRow[] rows = new ParagraphRow[]
        {
            new ParagraphRow(),
            new ParagraphRow(),
            new ParagraphRow()
        };

        public void show(){
            // Debug.Log("button_count: " + header.button_count);
            // Debug.Log("radio_button: " + header.radio_button);
            // Debug.Log("checkbox: " + header.checkbox);
            // Debug.Log("radio_button_active: " + header.radio_button_active);
            // Debug.Log("checkbox_active: " + header.checkbox_active);

            // Debug.Log("First row: " + string.Join(", ", rows[0].paragraphs));
            // Debug.Log("Second row: " + string.Join(", ", rows[1].paragraphs));
            // Debug.Log("Third row: " + string.Join(", ", rows[2].paragraphs));

            // Debug.Log("----------------------");
        }
    }

    public PageInterface pageInterface = new PageInterface();

    [System.Serializable]
    public class Row
    {
        public GameObject[] data;

        public int Count
        {
            get { return data.Length; }
        }
    }

    [System.Serializable]
    public class Button_Row
    {
        public Button[] data;

        public int Count
        {
            get { return data.Length; }
        }
    }

    public Button add_header_btn;
    public Button add_delete_radio_btn;
    public Button add_delete_checkbox;
    public Button delete_header_btn;
    public Button[] header_buttons;
    private int current_header_button = -1;

    private int max_par = 4;

    public Button radio_btn;
    public Text radio_text;
    private bool radio_btn_active = false; 

    public Button checkbox;
    private bool checkbox_active = false;
    // public Button delete_header_btn;

    public Button[] add_small_par_btn;
    public Button[] add_medium_par_btn;
    public Button[] add_large_par_btn;
    public Button[] add_textfield_btn;
    public Button[] delete_btn;

    public Row[] small_spaces;
    public Row[] medium_spaces;
    public Row[] large_spaces;
    public Button_Row[] btn;
    public Row[] textfiels;

    public Sprite green;
    public Sprite orange;
    public Sprite red;
    public Sprite black;
    public Sprite blue;
    public Sprite radio_off;
    public Sprite radio_on;
    public Sprite checkbox_off;
    public Sprite checkbox_on;

    private int[] row_count = new int[3];
    private int current_row = 0;
    Color light_blue = new Color(0, 153, 255, 255);
    // Button current_header_button_blue = null;

    public void add_header_button()
    {
        if(current_header_button + 1 < 3)
        {
            current_header_button++;
            header_buttons[current_header_button].gameObject.SetActive(true);
            if (current_header_button + 1 == header_buttons.Length)
                add_header_btn.interactable = false;
            delete_header_btn.interactable = true;
            pageInterface.header.button_count++;
        }
    }

    public void add_radio_button()
    {
        if(!radio_btn_active)
        {
            radio_btn_active = true;
            radio_btn.gameObject.SetActive(true);
            radio_text.gameObject.SetActive(true);
            add_delete_checkbox.interactable = false;
            add_delete_radio_btn.GetComponentInChildren<Text>().text = "Delete radio button";
            pageInterface.header.radio_button = true;
            // pageInterface.show();
        }
        else
        {
            radio_btn_active = false;
            radio_btn.gameObject.SetActive(false);
            radio_text.gameObject.SetActive(false);
            add_delete_checkbox.interactable = true;
            add_delete_radio_btn.GetComponentInChildren<Text>().text = "Add radio button";
            pageInterface.header.radio_button = false;
            // pageInterface.show();
        }
    }

    public void add_checkbox()
    {
        if(!checkbox_active)
        {
            checkbox_active = true;
            checkbox.gameObject.SetActive(true);
            radio_text.gameObject.SetActive(true);
            add_delete_radio_btn.interactable = false;
            add_delete_checkbox.GetComponentInChildren<Text>().text = "Delete checkbox";
            pageInterface.header.checkbox = true;
            // pageInterface.show();
        }
        else
        {
            checkbox_active = false;
            checkbox.gameObject.SetActive(false);
            radio_text.gameObject.SetActive(false);
            add_delete_radio_btn.interactable = true;
            add_delete_checkbox.GetComponentInChildren<Text>().text = "Add checkbox";
            pageInterface.header.checkbox = false;
            // pageInterface.show();
        }
    }

    public void delete_header_button()
    {
        if (current_header_button > -1)
        {
            header_buttons[current_header_button].gameObject.SetActive(false);
            current_header_button--;
            if (current_header_button == -1)
                delete_header_btn.interactable = false;
            add_header_btn.interactable = true;
            pageInterface.header.button_count--;
            // pageInterface.show();
        }
    }

    public void add_paragraph(int par_type)
    {
        delete_btn[current_row].interactable = true;

        if ((Paragraph_type)par_type == Paragraph_type.SMALL && row_count[current_row] < max_par)
        {
            small_spaces[current_row].data[row_count[current_row]].SetActive(true);
            btn[current_row].data[row_count[current_row]].gameObject.SetActive(true);
            pageInterface.rows[current_row].paragraphs.Add(new Paragraph(Paragraph_type.SMALL, Button_color.GREEN, btn[current_row].data[row_count[current_row]]));
            row_count[current_row]++;
            pageInterface.show();
        }
        else if ((Paragraph_type)par_type == Paragraph_type.MEDIUM && row_count[current_row] < max_par && row_count[current_row] < medium_spaces[current_row].Count)
        {
            medium_spaces[current_row].data[row_count[current_row]].SetActive(true);
            btn[current_row].data[row_count[current_row]].gameObject.SetActive(true);
            pageInterface.rows[current_row].paragraphs.Add(new Paragraph(Paragraph_type.MEDIUM, Button_color.GREEN, btn[current_row].data[row_count[current_row]]));
            row_count[current_row]+=2;
            pageInterface.show();
        }
        else if ((Paragraph_type)par_type == Paragraph_type.LARGE && row_count[current_row] < max_par && row_count[current_row] < large_spaces[current_row].Count)
        {
            large_spaces[current_row].data[row_count[current_row]].SetActive(true);
            btn[current_row].data[row_count[current_row]].gameObject.SetActive(true);
            pageInterface.rows[current_row].paragraphs.Add(new Paragraph(Paragraph_type.LARGE, Button_color.GREEN, btn[current_row].data[row_count[current_row]]));
            row_count[current_row]+=4;
            pageInterface.show();
        }
        else if ((Paragraph_type)par_type == Paragraph_type.TEXTFIELD && row_count[current_row] < max_par && row_count[current_row] < large_spaces[current_row].Count)
        {
            large_spaces[current_row].data[row_count[current_row]].SetActive(true);
            textfiels[current_row].data[row_count[current_row]].gameObject.SetActive(true);
            pageInterface.rows[current_row].paragraphs.Add(new Paragraph(Paragraph_type.TEXTFIELD));
            row_count[current_row]+=4;
            // pageInterface.show();
        }

        if (row_count[current_row] >= max_par)
            add_small_par_btn[current_row].interactable = false;
        if (row_count[current_row] + 1 >= max_par)
            add_medium_par_btn[current_row].interactable = false;
        if (row_count[current_row] + 4 >= max_par){
            add_large_par_btn[current_row].interactable = false;
            add_textfield_btn[current_row].interactable = false;
        }
    }

    public void delete_paragraph(GameObject row)
    {
        row_count[current_row] = 0;

        foreach (Transform child in row.transform)
        {
            child.gameObject.SetActive(false);
        }
        
        pageInterface.rows[current_row].paragraphs.Clear();
        pageInterface.show();

        if (row_count[current_row] < max_par)
            add_small_par_btn[current_row].interactable = true;
        if (row_count[current_row] < max_par + 2)
            add_medium_par_btn[current_row].interactable = true;
        if (row_count[current_row] < max_par + 4){
            add_large_par_btn[current_row].interactable = true;
            add_textfield_btn[current_row].interactable = true;
        }
        if (row_count[current_row] == 0)
            delete_btn[current_row].interactable = false;
    }

    public void change_row(int row)
    {
        current_row = row;
    }

    public void change_button_color(Button button)
    {
        Image image = button.targetGraphic as Image;
        Sprite sprite = image.sprite;

        if (sprite.name == "green_btn"){
            foreach(var row in pageInterface.rows){
                foreach(var paragraph in row.paragraphs){
                    if(GameObject.ReferenceEquals(button, paragraph.button)){
                        paragraph.button_color = Button_color.RED;
                    }
                }
            }
            button.image.sprite = red;
        }
        // else if (sprite.name == "orange_btn"){
        //     foreach(var row in pageInterface.rows){
        //         foreach(var paragraph in row.paragraphs){
        //             if(GameObject.ReferenceEquals(button, paragraph.button)){
        //                 paragraph.button_color = Button_color.RED;
        //             }
        //         }
        //     }
        //     button.image.sprite = red;
        // }
        else if (sprite.name == "red_btn"){
            foreach(var row in pageInterface.rows){
                foreach(var paragraph in row.paragraphs){
                    if(GameObject.ReferenceEquals(button, paragraph.button)){
                        paragraph.button_color = Button_color.GREEN;
                    }
                }
            }
            button.image.sprite = green;
        }
        // else if (sprite.name == "header_black_btn")
        // {
        //     if(current_header_button_blue != null){
        //         current_header_button_blue.GetComponentInChildren<UnityEngine.UI.Text>().Button_color = light_blue;
        //         current_header_button_blue.image.sprite = black;
        //     }
        //     current_header_button_blue = button;
        //     button.image.sprite = blue;
        //     light_blue = button.GetComponentInChildren<UnityEngine.UI.Text>().Button_color;
        //     button.GetComponentInChildren<UnityEngine.UI.Text>().Button_color = new Color(255, 255, 255, 255);
        // }
        // else if (sprite.name == "header_blue_btn")
        // {
        //     button.image.sprite = black;
        //     button.GetComponentInChildren<UnityEngine.UI.Text>().Button_color = light_blue;
        // }
        else if (sprite.name == "radio_btn_off")
        {
            button.image.sprite = radio_on;
            pageInterface.header.radio_button_active = true;
            // pageInterface.show();
        }
        else if (sprite.name == "radio_btn_on")
        {
            button.image.sprite = radio_off;
            pageInterface.header.radio_button_active = false;
            // pageInterface.show();
        }
        else if (sprite.name == "checkbox_off")
        {
            button.image.sprite = checkbox_on;
            pageInterface.header.checkbox_active = true;
            // pageInterface.show();
        }
        else if (sprite.name == "checkbox_on")
        {
            button.image.sprite = checkbox_off;
            pageInterface.header.checkbox_active = false;
            // pageInterface.show();
        }
    }

    public void export_png(Button button)
    {
        Image image = button.targetGraphic as Image;
        Sprite sprite = image.sprite;

        if (sprite.name == "green_btn")
            button.image.sprite = orange;
        else if (sprite.name == "orange_btn")
            button.image.sprite = red;
        else if (sprite.name == "red_btn")
            button.image.sprite = green;
        else if (sprite.name == "header_black_btn")
            button.image.sprite = blue;
        else if (sprite.name == "header_blue_btn")
            button.image.sprite = black;
    }
}
