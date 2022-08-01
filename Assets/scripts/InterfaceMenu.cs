using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        RED
    }

    public class Header
    {
        public int button_count = 0;
        public bool radio_button = false;
        public bool checkbox = false;
        public bool radio_button_active = false;
        public bool checkbox_active = false;
    }

    public class Paragraph
    {
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

    public class ParagraphRow
    {
        public List<Paragraph> paragraphs = new List<Paragraph>();
    }

    public class PageInterface
    {
        public Header header = new Header();
        public ParagraphRow[] rows = new ParagraphRow[]
        {
            new ParagraphRow(),
            new ParagraphRow(),
            new ParagraphRow()
        };
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
    public Button generate_btn;
    private int current_header_button = -1;

    private int max_par = 4;

    public Button radio_btn;
    public Text radio_text;
    private bool radio_btn_active = false;

    public Button checkbox;
    private bool checkbox_active = false;

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
    private int max_button_count = 3;

    public void add_header_button()
    {
        if (current_header_button + 1 < max_button_count)
        {
            current_header_button++;
            header_buttons[current_header_button].gameObject.SetActive(true);
            if (current_header_button + 1 == header_buttons.Length)
                add_header_btn.interactable = false;
            delete_header_btn.interactable = true;
            pageInterface.header.button_count++;
        }
        generate_btn.interactable = false;
    }

    public void add_checkbox()
    {
        if (!checkbox_active)
        {
            checkbox_active = true;
            checkbox.gameObject.SetActive(true);
            radio_text.gameObject.SetActive(true);
            add_delete_radio_btn.interactable = false;
            add_delete_checkbox.GetComponentInChildren<Text>().text = "Delete checkbox";
            pageInterface.header.checkbox = true;
        }
        else
        {
            checkbox_active = false;
            checkbox.gameObject.SetActive(false);
            radio_text.gameObject.SetActive(false);
            add_delete_radio_btn.interactable = true;
            add_delete_checkbox.GetComponentInChildren<Text>().text = "Add checkbox";
            pageInterface.header.checkbox = false;
        }
        generate_btn.interactable = false;
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
        }
        generate_btn.interactable = false;
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
        }
        else if ((Paragraph_type)par_type == Paragraph_type.MEDIUM && row_count[current_row] < max_par && row_count[current_row] < medium_spaces[current_row].Count)
        {
            medium_spaces[current_row].data[row_count[current_row]].SetActive(true);
            btn[current_row].data[row_count[current_row]].gameObject.SetActive(true);
            pageInterface.rows[current_row].paragraphs.Add(new Paragraph(Paragraph_type.MEDIUM, Button_color.GREEN, btn[current_row].data[row_count[current_row]]));
            row_count[current_row] += 2;
        }
        else if ((Paragraph_type)par_type == Paragraph_type.LARGE && row_count[current_row] < max_par && row_count[current_row] < large_spaces[current_row].Count)
        {
            large_spaces[current_row].data[row_count[current_row]].SetActive(true);
            btn[current_row].data[row_count[current_row]].gameObject.SetActive(true);
            pageInterface.rows[current_row].paragraphs.Add(new Paragraph(Paragraph_type.LARGE, Button_color.GREEN, btn[current_row].data[row_count[current_row]]));
            row_count[current_row] += 4;
        }
        else if ((Paragraph_type)par_type == Paragraph_type.TEXTFIELD && row_count[current_row] < max_par && row_count[current_row] < large_spaces[current_row].Count)
        {
            large_spaces[current_row].data[row_count[current_row]].SetActive(true);
            textfiels[current_row].data[row_count[current_row]].gameObject.SetActive(true);
            pageInterface.rows[current_row].paragraphs.Add(new Paragraph(Paragraph_type.TEXTFIELD));
            row_count[current_row] += 4;
        }

        if (row_count[current_row] >= max_par)
            add_small_par_btn[current_row].interactable = false;
        if (row_count[current_row] + 1 >= max_par)
            add_medium_par_btn[current_row].interactable = false;
        if (row_count[current_row] + 4 >= max_par)
        {
            add_large_par_btn[current_row].interactable = false;
            add_textfield_btn[current_row].interactable = false;
        }
        generate_btn.interactable = false;
    }

    public void delete_paragraph(GameObject row)
    {
        row_count[current_row] = 0;

        foreach (Transform child in row.transform)
        {
            child.gameObject.SetActive(false);
        }

        pageInterface.rows[current_row].paragraphs.Clear();

        if (row_count[current_row] < max_par)
            add_small_par_btn[current_row].interactable = true;
        if (row_count[current_row] < max_par + 2)
            add_medium_par_btn[current_row].interactable = true;
        if (row_count[current_row] < max_par + 4)
        {
            add_large_par_btn[current_row].interactable = true;
            add_textfield_btn[current_row].interactable = true;
        }
        if (row_count[current_row] == 0)
            delete_btn[current_row].interactable = false;
        generate_btn.interactable = false;
    }

    public void change_row(int row)
    {
        current_row = row;
    }

    public void ChangeButtonColor(Button button)
    {
        Image image = button.targetGraphic as Image;
        Sprite sprite = image.sprite;

        if (sprite.name == "green_btn")
        {
            foreach (var row in pageInterface.rows)
            {
                foreach (var paragraph in row.paragraphs)
                {
                    if (GameObject.ReferenceEquals(button, paragraph.button))
                    {
                        paragraph.button_color = Button_color.RED;
                    }
                }
            }
            button.image.sprite = red;
        }
        else if (sprite.name == "red_btn")
        {
            foreach (var row in pageInterface.rows)
            {
                foreach (var paragraph in row.paragraphs)
                {
                    if (GameObject.ReferenceEquals(button, paragraph.button))
                    {
                        paragraph.button_color = Button_color.GREEN;
                    }
                }
            }
            button.image.sprite = green;
        }
        else if (sprite.name == "checkbox_off")
        {
            button.image.sprite = checkbox_on;
            pageInterface.header.checkbox_active = true;
        }
        else if (sprite.name == "checkbox_on")
        {
            button.image.sprite = checkbox_off;
            pageInterface.header.checkbox_active = false;
        }
        generate_btn.interactable = false;
    }
}
