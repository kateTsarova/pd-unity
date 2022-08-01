using System.Collections;
// using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using UnityEngine.UI;
using System;
using UnityEditor;
// using System.Drawing.Image;
// using System.Drawing.Rectangle;
// using System.Drawing.Bitmap;

// using System;
// using System.IO;
// using System.Threading.Tasks;
// using System.Windows.Forms;
// namespace WindowsFormsApplication1

public class CreateDatabase : MonoBehaviour
{
    public enum Header_states
    {
        EMPTY,
        // RADIO_ON,
        // RADIO_OFF,
        CHECKBOX_ON,
        CHECKBOX_OFF
    }

    public class Header {
        public static int max_button_count = 3;
        public int button_count = 0;
        public Header_states header_state = Header_states.EMPTY;

        public Header(int count, Header_states state)
        {
            button_count = count;
            header_state = state;
        }
    }

    public enum Button_color
    {
        GREEN,
        // ORANGE,
        RED
    }

    public enum Paragraph_type
    {
        SMALL,
        MEDIUM,
        LARGE, 
        TEXTFIELD
    }

    public class Paragraph {
        public Paragraph_type type;
        public Button_color color;

        private bool empty = true;

        public Paragraph(Paragraph_type t, Button_color c)
        {
            type = t;
            color = c;
            empty = false;
        }

        public Paragraph()
        {
            empty = true;
        }
    }

    public class Row {
        public List<Paragraph> paragraphs = new List<Paragraph>();
    }

    [System.Serializable]
    public class WebRow {
        public GameObject[] smallParagraph;
        public GameObject[] mediumParagraph;
        public GameObject largeParagraph;
        public GameObject textbox;
        public Button[] buttons;
    }

    public class WebInterface {
        public Header header;
        public List<Row> rows = new List<Row>();

        public WebInterface(Header h, Row f, Row s, Row t)
        {
            header = h;
            rows.Add(f);
            rows.Add(s);
            rows.Add(t);
        }
    }

    [System.Serializable]
    public class InterfaceText {
        public Text text;
        public int len;
        public Color color;
        public bool bold = false;
    }

    public Camera myCamera;
    public Canvas canvas;
    private bool takeScreenshotOnNextFrame;
    public Button b;

    public Button[] headerButtons;
    public Button radioButton;
    public Button checkbox;
    public Text radioText;
    
    public Sprite radio_off;
    public Sprite radio_on;
    public Sprite checkbox_off;
    public Sprite checkbox_on;
    public Sprite green;
    public Sprite orange;
    public Sprite red;

    public WebRow firstRow;
    public WebRow secondRow;
    public WebRow thirdRow;

    public InterfaceText[] interfaceTexts;

    private void Update() {
        if(takeScreenshotOnNextFrame) {
            takeScreenshotOnNextFrame = false;
            RenderTexture renderTexture = myCamera.targetTexture;
            Debug.Log("renderTexture.width" + renderTexture.width + " --- " + renderTexture.height);
            // Debug.Log(renderTexture.width*(float)0.4413 + " ***** " + renderTexture.height*(float)0.1515);

            Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(500, 105, renderTexture.width, renderTexture.height);
            // Rect rect = new Rect(-51, 105, renderTexture.width, renderTexture.height);
            // Rect rect = new Rect(renderTexture.width*(float)0.4413, renderTexture.height*(float)0.1515, renderTexture.width, renderTexture.height);
            renderResult.ReadPixels(rect, 0, 0);
            //Graphics.CopyTexture(renderTexture, renderResult);

            byte[] byteArray = renderResult.EncodeToPNG();
            System.IO.File.WriteAllBytes("C:\\Users\\Nata\\Desktop\\res\\CameraScreenshot.png", byteArray);
            Debug.Log("Screenshot taken!");

            RenderTexture.ReleaseTemporary(renderTexture);
            myCamera.targetTexture = null;

            // MakeGuiForDatabase();
        }
    }

    public void TakeScreenshot() { //int width, int height
        // int width = 1140;
        // int height = 650;
        // // int width = (int)(myCamera.pixelWidth * 0.83);
        // // int height = myCamera.pixelHeight;
        // //int width = myCamera.pixelWidth;
        // //int height = myCamera.pixelHeight;
        // Debug.Log("myCamera.pixelWidth: " + myCamera.pixelWidth + " - " + myCamera.pixelHeight);
        // // Debug.Log("Main window display: " + Screen.mainWindowDisplayInfo);
        // myCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
        // takeScreenshotOnNextFrame = true;
        // // Debug.Log(Screen.width);
        // // Debug.Log(Screen.height);

        // Debug.Log("button: " + myCamera.WorldToScreenPoint(b.transform.position));

        // GetAllHeaders();

        // GetOneRow();

        // getInterfaces();

        StartCoroutine(showInterface());

        // ScreenCapture.CaptureScreenshot("C:\\Users\\Nata\\Desktop\\res\\CameraScreenshot.png");
        
        //int x= 10, y=20, width=200, height=100;
        // Bitmap source = new Bitmap("C:\\Users\\Nata\\Desktop\\res\\CameraScreenshot.png");
        // Bitmap CroppedImage = source.Clone(new System.Drawing.Rectangle(x, y, width, height), source.PixelFormat);
    }

    public void MakeGuiFromUser() {
        string guiText = "";
        
        if(canvas.GetComponent<InterfaceMenu>().pageInterface.header.button_count != 0 || canvas.GetComponent<InterfaceMenu>().pageInterface.header.checkbox)
        {
            guiText = "header {\n";

            for(int i = 0; i < canvas.GetComponent<InterfaceMenu>().pageInterface.header.button_count; i++){
                guiText += "btn-header";
                if(i == canvas.GetComponent<InterfaceMenu>().pageInterface.header.button_count - 1 && !canvas.GetComponent<InterfaceMenu>().pageInterface.header.checkbox)
                    guiText += "\n";
                else
                    guiText += ", ";
            }

            // if(canvas.GetComponent<InterfaceMenu>().pageInterface.header.radio_button_active)
            //     guiText += "radio_btn_active";
            if(canvas.GetComponent<InterfaceMenu>().pageInterface.header.checkbox_active)
                guiText += "checkbox_active";
            // else if(canvas.GetComponent<InterfaceMenu>().pageInterface.header.radio_button)
            //     guiText += "radio_btn_inactive";
            else if(canvas.GetComponent<InterfaceMenu>().pageInterface.header.checkbox)
                guiText += "checkbox_inactive";

            guiText += "\n}\n";

            if(canvas.GetComponent<InterfaceMenu>().pageInterface.rows[0].paragraphs.Count > 0){
                guiText += "row {\n";

                foreach(var p in canvas.GetComponent<InterfaceMenu>().pageInterface.rows[0].paragraphs){
                    switch(p.type){
                        case InterfaceMenu.Paragraph_type.SMALL:
                            guiText += "quadruple {\nsmall-title, ";
                            break;
                        case InterfaceMenu.Paragraph_type.MEDIUM:
                            guiText += "double {\nsmall-title, ";
                            break;
                        default:
                            guiText += "single {\nsmall-title, ";
                            break;
                    }

                    if(p.type == InterfaceMenu.Paragraph_type.TEXTFIELD)
                        guiText += "textfield";
                    else{
                        guiText += "text, ";

                        switch(p.button_color){
                            case InterfaceMenu.Button_color.GREEN:
                                guiText += "btn-green";
                                break;
                            // case InterfaceMenu.Button_color.ORANGE:
                            //     guiText += "btn-orange";
                            //     break;
                            case InterfaceMenu.Button_color.RED:
                                guiText += "btn-red";
                                break;
                        }
                    }
                    guiText += "\n}\n";
                }
            }

            guiText += "}\n";
        }

        if(canvas.GetComponent<InterfaceMenu>().pageInterface.rows[1].paragraphs.Count > 0){
            guiText += "row {\n";

            foreach(var p in canvas.GetComponent<InterfaceMenu>().pageInterface.rows[1].paragraphs){
                switch(p.type){
                    case InterfaceMenu.Paragraph_type.SMALL:
                        guiText += "quadruple {\nsmall-title, ";
                        break;
                    case InterfaceMenu.Paragraph_type.MEDIUM:
                        guiText += "double {\nsmall-title, ";
                        break;
                    default:
                        guiText += "single {\nsmall-title, ";
                        break;
                }

                if(p.type == InterfaceMenu.Paragraph_type.TEXTFIELD)
                    guiText += "textfield";
                else{
                    guiText += "text, ";

                    switch(p.button_color){
                        case InterfaceMenu.Button_color.GREEN:
                            guiText += "btn-green";
                            break;
                        // case InterfaceMenu.Button_color.ORANGE:
                        //     guiText += "btn-orange";
                        //     break;
                        case InterfaceMenu.Button_color.RED:
                            guiText += "btn-red";
                            break;
                    }
                }
                guiText += "\n}\n";
            }

            guiText += "}\n";
        }

        if(canvas.GetComponent<InterfaceMenu>().pageInterface.rows[2].paragraphs.Count > 0){
            guiText += "row {\n";

            foreach(var p in canvas.GetComponent<InterfaceMenu>().pageInterface.rows[2].paragraphs){
                switch(p.type){
                    case InterfaceMenu.Paragraph_type.SMALL:
                        guiText += "quadruple {\nsmall-title, ";
                        break;
                    case InterfaceMenu.Paragraph_type.MEDIUM:
                        guiText += "double {\nsmall-title, ";
                        break;
                    default:
                        guiText += "single {\nsmall-title, ";
                        break;
                }

                if(p.type == InterfaceMenu.Paragraph_type.TEXTFIELD)
                    guiText += "textfield";
                else{
                    guiText += "text, ";

                    switch(p.button_color){
                        case InterfaceMenu.Button_color.GREEN:
                            guiText += "btn-green";
                            break;
                        // case InterfaceMenu.Button_color.ORANGE:
                        //     guiText += "btn-orange";
                        //     break;
                        case InterfaceMenu.Button_color.RED:
                            guiText += "btn-red";
                            break;
                    }
                }
                guiText += "\n}\n";
            }

            guiText += "}\n";
        }

        Debug.Log("guiText: " + guiText);

        string filename = "C:\\Users\\Nata\\Desktop\\res\\CameraScreenshot.gui";

        if (File.Exists(filename))
        {    
            File.Delete(filename);
        }
        
        // Create a new file     
        using (FileStream fs = File.Create(filename))
        {
            // Add some text to file    
            byte[] info = new UTF8Encoding(true).GetBytes(guiText);
            fs.Write(info, 0, info.Length);
        }
    }

    public void MakeGuiForDatabase(WebInterface webInterface, string path) {

        System.Random rand = new System.Random();
        // int num = rand.Next(0, 4);

        int num = 2;

        if(num != 1 && webInterface.header.button_count != 0){
            string guiText = "";
            
            if(webInterface.header.button_count != 0 || webInterface.header.header_state != Header_states.EMPTY)
            {
                guiText = "header {\n";

                for(int i = 0; i < webInterface.header.button_count; i++){
                    guiText += "btn-header";
                    if(i == webInterface.header.button_count - 1 && webInterface.header.header_state == Header_states.EMPTY)
                        guiText += "";
                    else
                        guiText += ", ";
                }

                // if(webInterface.header.header_state == Header_states.RADIO_ON)
                //     guiText += "radio_btn_active";
                if(webInterface.header.header_state == Header_states.CHECKBOX_ON)
                    guiText += "checkbox_active";
                // else if(webInterface.header.header_state == Header_states.RADIO_OFF)
                //     guiText += "radio_btn_inactive";
                else if(webInterface.header.header_state == Header_states.CHECKBOX_OFF)
                    guiText += "checkbox_inactive";

                guiText += "\n}";
            }
            
            if(webInterface.rows[0].paragraphs.Count > 0){
                guiText += "\nrow {\n";

                foreach(var p in webInterface.rows[0].paragraphs){
                    switch(p.type){
                        case Paragraph_type.SMALL:
                            guiText += "quadruple {\nsmall-title, ";
                            break;
                        case Paragraph_type.MEDIUM:
                            guiText += "double {\nsmall-title, ";
                            break;
                        default:
                            guiText += "single {\nsmall-title, ";
                            break;
                    }

                    if(p.type == Paragraph_type.TEXTFIELD)
                        guiText += "textfield";
                        // continue;
                    else{
                    guiText += "text, ";

                    switch(p.color){
                        case Button_color.GREEN:
                            guiText += "btn-green";
                            break;
                        // case Button_color.ORANGE:
                        //     guiText += "btn-orange";
                        //     break;
                        case Button_color.RED:
                            guiText += "btn-red";
                            break;
                    }
                    }
                    guiText += "\n}\n";
                }
                guiText += "}";
            }

            if(webInterface.rows[1].paragraphs.Count > 0){
                guiText += "\nrow {\n";

                foreach(var p in webInterface.rows[1].paragraphs){
                    switch(p.type){
                        case Paragraph_type.SMALL:
                            guiText += "quadruple {\nsmall-title, ";
                            break;
                        case Paragraph_type.MEDIUM:
                            guiText += "double {\nsmall-title, ";
                            break;
                        default:
                            guiText += "single {\nsmall-title, ";
                            break;
                    }

                    if(p.type == Paragraph_type.TEXTFIELD)
                        guiText += "textfield";
                        // continue;
                    else{
                    guiText += "text, ";

                    switch(p.color){
                        case Button_color.GREEN:
                            guiText += "btn-green";
                            break;
                        // case Button_color.ORANGE:
                        //     guiText += "btn-orange";
                        //     break;
                        case Button_color.RED:
                            guiText += "btn-red";
                            break;
                    }
                    }
                    guiText += "\n}\n";
                }
                guiText += "}";
            }

            // if(webInterface.rows[2].paragraphs.Count > 0){
            //     guiText += "\nrow {\n";

            //     foreach(var p in webInterface.rows[2].paragraphs){
            //         switch(p.type){
            //             case Paragraph_type.SMALL:
            //                 guiText += "quadruple {\nsmall-title, ";
            //                 break;
            //             case Paragraph_type.MEDIUM:
            //                 guiText += "double {\nsmall-title, ";
            //                 break;
            //             default:
            //                 guiText += "single {\nsmall-title, ";
            //                 break;
            //         }

            //         if(p.type == Paragraph_type.TEXTFIELD)
            //             guiText += "textfield";
            //         else{
            //         guiText += "text, ";

            //         switch(p.color){
            //             case Button_color.GREEN:
            //                 guiText += "btn-green";
            //                 break;
            //             // case Button_color.ORANGE:
            //             //     guiText += "btn-orange";
            //             //     break;
            //             case Button_color.RED:
            //                 guiText += "btn-red";
            //                 break;
            //         }
            //         }
            //         guiText += "\n}\n";
            //     }
            //     guiText += "}";
            // }

            // Debug.Log("guiText: " + guiText);

            string filename = path + ".gui";
            Debug.Log(filename);

            if (File.Exists(filename))
            {    
                File.Delete(filename);
            }
            
            // Create a new file     
            using (FileStream fs = File.Create(filename))
            {
                // Add some text to file    
                byte[] info = new UTF8Encoding(true).GetBytes(guiText);
                fs.Write(info, 0, info.Length);
            }
            
            ScreenCapture.CaptureScreenshot(path + ".png");
        }
    }

    public List<Header> GetAllHeaders() {
        List<Header> allHeaders = new List<Header>();
        // Debug.Log(Header.max_button_count);
        for(int btn = 0; btn <= Header.max_button_count; btn++){
            foreach (Header_states i in Enum.GetValues(typeof(Header_states)))  
            {
                // Debug.Log(btn + "   " + i);
                allHeaders.Add(new Header(btn, i));
            }
        }
        // Debug.Log("allHeaders.Count: " + allHeaders.Count);
        return allHeaders;
    }

    static IEnumerable<String> CombinationsWithRepetition(IEnumerable<int> input, int length)
    {
        if (length <= 0)
            yield return "";
        else
        {
            foreach(var i in input)
                foreach(var c in CombinationsWithRepetition(input, length-1))
                    yield return i.ToString() + c;
        }
    }

    public List<Row> GetOneRow() {
        int[][] allCombos = {
            new int[] {}, 
            // new int[] {0}, 
            // new int[] {0, 0}, 
            // new int[] {0, 0, 0}, 
            new int[] {0, 0, 0, 0}, 
            // new int[] {1},
            // new int[] {1, 0}, 
            new int[] {1, 0, 0}, 
            // new int[] {0, 1}, 
            new int[] {0, 1, 0}, 
            new int[] {0, 0, 1}, 
            new int[] {1, 1}, 
            new int[] {2},
            new int[] {3}
        };

        List<Row> rows = new List<Row>();

        // Row row = new Row();
        // rows.Add(row);
        System.Random rand = new System.Random();

        foreach(var combo in allCombos){
            Row r = new Row();

            for(int i = 0; i<combo.Length; i++){
                int num = rand.Next(0, 3);
                r.paragraphs.Add(new Paragraph((Paragraph_type)combo[i], (Button_color)num));
            }

            // foreach(var c in CombinationsWithRepetition(new int[]{0, 1, 2}, combo.Length)){
            //     r.paragraphs.Clear();
            //     List<int> ints = new List<int>();
            //     for(int i = 0; i < c.Length; i++)
            //         ints.Add(Int32.Parse(c[i].ToString()));
            //     string s = "";
            //     for(int i = 0; i < ints.Count; i++){
            //         s += ints[i] + " ";
            //     }
            //     // Debug.Log(c + "  " + ints + ":   " + ints.Count + " = " + combo.Length);
            //     // Debug.Log(c + " " + " " + c.Length);
            //     // Debug.Log();
            //     // if(ints.Count > 4)
            //     //     Debug.Log("JHFRKJGHRKHGKGR.HRKGHKER.GKERGEGRints: " + ints.Count);
            //     for(int i = 0; i < ints.Count; i++){
            //         // Debug.Log("1" + " " + (Button_color)ints[i]);
            //         r.paragraphs.Add(new Paragraph((Paragraph_type)combo[i], (Button_color)ints[i]));
            //         // Debug.Log("2" + " " + r.paragraphs[i].color);
            //         // if(i!=0)
            //         //     Debug.Log("3" + " " + r.paragraphs[i-1].color);
            //     }
                
            rows.Add(r);
                // Debug.Log("r.paragraphs.Count: " + r.paragraphs.Count);
            // }
            // for(int j = 0; j < 3; j++){
            //     string ss = "";
            //     for(int i = 0; i<r.paragraphs.Count; i++){
            //         // ss += r.paragraphs[i].color + " ";
            //         if(r.paragraphs[i].color != Button_color.RED)
            //             Debug.Log("yjfhfthfftjxfjfxjtfjfjyjygj" + r.paragraphs[i].color);

            //     }
            //     Debug.Log(ss);
            // }
        }

        // Debug.Log("rows.Count: " + rows.Count);
        for(int i = 0; i<rows.Count; i++){
            string s = "";
            for(int j = 0; j<rows[i].paragraphs.Count; j++){
                s += rows[i].paragraphs[j].type + " ";
            }
            // Debug.Log(i + ": " + s);
        }

        // for(int j = 0; j < 3; j++){
        //     string s = "";
        //     for(int i = 0; i<rows[j].paragraphs.Count; i++){
        //         s += rows[j].paragraphs[i].color + " ";
        //         if(rows[j].paragraphs[i].color != Button_color.RED)
        //             Debug.Log("yjfhfthfftjxfjfxjtfjfjyjygj" + rows[j].paragraphs[i].color);

        //     }
        //     Debug.Log(s);
        // }

        return rows;
    }

    public List<WebInterface> getInterfaces() {
        List<Header> headers = GetAllHeaders();
        List<Row> rows = GetOneRow();
        List<WebInterface> interfaces = new List<WebInterface>();

        foreach(var header in headers){
            foreach(var firstrow in rows){
                foreach(var secondrow in rows){
                    // foreach(var thirdrow in rows){
                        // if((thirdrow.paragraphs.Count != 0 && secondrow.paragraphs.Count == 0) || 
                        //     (secondrow.paragraphs.Count != 0 && firstrow.paragraphs.Count == 0) ||
                        //     (secondrow.paragraphs.Count != 0 && thirdrow.paragraphs.Count != 0 && firstrow.paragraphs.Count == 0)){
                        //         continue;
                        //     }
                        if((secondrow.paragraphs.Count != 0 && firstrow.paragraphs.Count == 0)){
                                continue;
                            }
                        interfaces.Add(new WebInterface(header, firstrow, secondrow, firstrow));
                    // }
                }
            }
        }

        // Debug.Log(interfaces.Count);

        return interfaces;
    }

    public void changeText(){
        const string glyphs= "abcdefghijklmnopqrstuvwxyz";

        foreach(var interfaceText in interfaceTexts){
            // interfaceText.text.text = "";
            string s = "";
            for(int i=0; i<interfaceText.len; i++)
            {
                s += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
            }
            interfaceText.text.text = s;
            Debug.Log(interfaceText.len + "  " + s);
            interfaceText.text.color = interfaceText.color;
        }
    }

    public void clearInterface(){
        // changeText();
        
        WebRow[] webRows = {firstRow, secondRow, thirdRow};

        for(int i = 0; i<headerButtons.Length; i++)
        {
            headerButtons[i].gameObject.SetActive(false);
        }

        radioButton.gameObject.SetActive(false);
        radioText.gameObject.SetActive(false);
        radioButton.gameObject.SetActive(false);
        radioText.gameObject.SetActive(false);
        checkbox.gameObject.SetActive(false);
        radioText.gameObject.SetActive(false);
        checkbox.gameObject.SetActive(false);
        radioText.gameObject.SetActive(false);

        for(int i = 0; i<3; i++){
            foreach(var par in webRows[i].buttons){
                par.gameObject.SetActive(false);
            }
            foreach(var par in webRows[i].smallParagraph){
                par.gameObject.SetActive(false);
            }
            foreach(var par in webRows[i].mediumParagraph){
                par.gameObject.SetActive(false);
            }
            webRows[i].largeParagraph.gameObject.SetActive(false);
            webRows[i].textbox.gameObject.SetActive(false);
        }
    }

    public IEnumerator showInterface(){
        Debug.Log("here");
        WebRow[] webRows = {firstRow, secondRow, thirdRow};

        List<WebInterface> interfaces = getInterfaces();

        for(int k = 0; k < interfaces.Count; k++){ //interfaces.Count
            clearInterface();

            WebInterface webInterface = interfaces[k];

            // Debug.Log(webInterface.header.button_count);
            // Debug.Log(webInterface.header.header_state);

            // for(int k = 0; k < interfaces.Count; k++){
            //     for(int j = 0; j < 3; j++){
            //         // string s = "";
            //         for(int i = 0; i<interfaces[k].rows[j].paragraphs.Count; i++){
            //             // s += webInterface.rows[j].paragraphs[i].color + " ";
            //             if(interfaces[k].rows[j].paragraphs[i].color != Button_color.RED)
            //                 Debug.Log(k);
            //         }
            //         // Debug.Log(s);
            //     }
            // }
            // Debug.Log(webInterface.rows[0].paragraphs[0].type);
            // Debug.Log(webInterface.rows[0].paragraphs[0].color);

            // Button_color a = Button_color.RED;
            // Button_color b = Button_color.RED;
            // Button_color c = Button_color.ORANGE;

            // Debug.Log(a == b);
            // Debug.Log(a == c);

            // for(int k = 0; k<interfaces.Count; k++){
            //     string show = k.ToString();
            //     bool check = false;
            //     Button_color bc = Button_color.RED;
            //     for(int j = 0; j<3; j++){
            //         for(int i = 0; i<interfaces[k].rows[j].paragraphs.Count; i++){
            //             if(j==0)
            //                 bc = interfaces[k].rows[j].paragraphs[i].color;
            //             else{
            //                 if(bc != interfaces[k].rows[j].paragraphs[i].color)
            //                     check = true;
            //             }
            //             show += interfaces[k].rows[j].paragraphs[i].color + " ";
            //         }
            //         // Debug.Log("row: " + j);
            //         // Debug.Log("count: " + interfaces[100000].rows[j].paragraphs.Count);
            //         // for(int i = 0; i<interfaces[100000].rows[j].paragraphs.Count; i++){
            //         //     Debug.Log(interfaces[100000].rows[j].paragraphs[i].type + "-" + interfaces[100000].rows[j].paragraphs[i].color + " ");
            //         // }
            //     }
            //     if(check)
            //         Debug.Log(show);
            // }

            // Header h = new Header(5, Header_states.RADIO_OFF);

            // Row f = new Row();
            // f.paragraphs.Add(new Paragraph(Paragraph_type.TEXTFIELD, Button_color.ORANGE));
            // Row s = new Row();
            // s.paragraphs.Add(new Paragraph(Paragraph_type.MEDIUM, Button_color.ORANGE));
            // s.paragraphs.Add(new Paragraph(Paragraph_type.SMALL, Button_color.GREEN));
            // s.paragraphs.Add(new Paragraph(Paragraph_type.SMALL, Button_color.ORANGE));
            // Row t = new Row();
            // t.paragraphs.Add(new Paragraph(Paragraph_type.SMALL, Button_color.GREEN));
            // t.paragraphs.Add(new Paragraph(Paragraph_type.MEDIUM, Button_color.ORANGE));
            // t.paragraphs.Add(new Paragraph(Paragraph_type.SMALL, Button_color.RED));

            // WebInterface webInterface = new WebInterface(h, f, s, t);

            System.Random rand = new System.Random();

            for(int i = 0; i<webInterface.header.button_count; i++)
            {
                headerButtons[i].gameObject.SetActive(true);
            }

            switch(webInterface.header.header_state){
                // case Header_states.RADIO_ON:
                // radioButton.gameObject.SetActive(true);
                // radioText.gameObject.SetActive(true);
                // radioButton.image.sprite = radio_on;
                // break;
                // case Header_states.RADIO_OFF:
                // radioButton.gameObject.SetActive(true);
                // radioText.gameObject.SetActive(true);
                // radioButton.image.sprite = radio_off;
                // break;
                case Header_states.CHECKBOX_ON:
                checkbox.gameObject.SetActive(true);
                radioText.gameObject.SetActive(true);
                checkbox.image.sprite = checkbox_on;
                break;
                case Header_states.CHECKBOX_OFF:
                checkbox.gameObject.SetActive(true);
                radioText.gameObject.SetActive(true);
                checkbox.image.sprite = checkbox_off;
                break;
            }

            for(int i = 0; i < 2; i++){
                int count = 0;
                // Debug.Log(i + " webInterface.rows[i].paragraphs.Count " + webInterface.rows[i].paragraphs.Count);
                for(int j = 0; j < webInterface.rows[i].paragraphs.Count; j++){                
                    if(webInterface.rows[i].paragraphs[j].type != Paragraph_type.TEXTFIELD){
                        Debug.Log(count);
                        webRows[i].buttons[count].gameObject.SetActive(true);
                        int num = rand.Next(0, 2);
                        Button_color randColor = (Button_color)num;
                        switch(randColor){
                            case Button_color.GREEN:
                            webInterface.rows[i].paragraphs[j].color = Button_color.GREEN;
                            webRows[i].buttons[count].image.sprite = green;
                            break;
                            // case Button_color.ORANGE:
                            // webInterface.rows[i].paragraphs[j].color = Button_color.ORANGE;
                            // webRows[i].buttons[count].image.sprite = orange;
                            // break;
                            case Button_color.RED:
                            webInterface.rows[i].paragraphs[j].color = Button_color.RED;
                            webRows[i].buttons[count].image.sprite = red;
                            break;
                        }
                    }
                    switch(webInterface.rows[i].paragraphs[j].type){
                        case Paragraph_type.SMALL:
                        webRows[i].smallParagraph[count].SetActive(true);
                        count++;
                        break;
                        case Paragraph_type.MEDIUM:
                        webRows[i].mediumParagraph[count].SetActive(true);
                        count+=2;
                        break;
                        case Paragraph_type.LARGE:
                        webRows[i].largeParagraph.SetActive(true);
                        count+=4;
                        break;
                        case Paragraph_type.TEXTFIELD:
                        webRows[i].largeParagraph.SetActive(true);
                        webRows[i].textbox.SetActive(true);
                        count+=4;
                        break;
                    }
                }
            }

            string glyphs = "abcdefghijklmnopqrstuvwxyz";
            string filename = "";
            for(int i=0; i<6; i++)
            {
                filename += glyphs[UnityEngine.Random.Range(0, glyphs.Length)];
                // this.GetComponent<UnityEngine.UI.Text>().text += lorem[i];
            }

            string path = "D:\\web_database\\final\\" + filename;

            MakeGuiForDatabase(webInterface, path);
            // ScreenCapture.CaptureScreenshot("D:\\web_database\\1.png");
            // ScreenCapture.CaptureScreenshot("C:\\Users\\Nata\\Desktop\\res\\CameraScreenshot.png");

            if(k % 100 == 0){
                Debug.Log(k);
            }

            // Debug.Log(k);

            yield return new WaitForSeconds(0.01F);
        }
    }

    // public void connectWithPython(){
    //     Debug.Log("connecting...");
    //     try
    //     {
    //         System.Diagnostics.Process.Start("D:\\again\\pix2code-master\\pix2code-master\\test.py");
    //     }
    //     catch (Exception ex)
    //     {
    //         Debug.Log(ex.Message);
    //     }
    //     // System.Diagnostics.Process.Start("D:\\again\\pix2code-master\\pix2code-master\\test.py");
    // }

    // private void connectWithPython()
    //     {
    //     Debug.Log("connecting...");

    //         string fileName = "D:\\again\\pix2code-master\\pix2code-master\\test.py";

    //         System.Diagnostics.Process p = new System.Diagnostics.Process();
    //         p.StartInfo = new System.Diagnostics.ProcessStartInfo("C:\\Program Files\\Unity\\Hub\\Editor\\2020.3.23f1\\Editor\\Data\\PlaybackEngines\\AndroidPlayer\\NDK\\prebuilt\\windows-x86_64\\bin\\python.exe", fileName)
    //         {
    //             RedirectStandardOutput = true,
    //             UseShellExecute = false,
    //             CreateNoWindow = true
    //         };
    //         p.Start();

    //         string output = p.StandardOutput.ReadToEnd();
    //         p.WaitForExit();

    //         Console.WriteLine(output);

    //         Console.ReadLine();

    //     }

    private void connectWithPython()
    {
        Debug.Log("hjxhtx");
        string path = EditorUtility.OpenFolderPanel("Select Directory", "", "");
        if(path.Length > 0){
            ScreenCapture.CaptureScreenshot(path + "/out.png");
            string cmd = "D:\\again\\pix2code-master\\pix2code-master\\model\\sample.py";
            System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo();
            start.FileName = "C:\\Users\\Nata\\PycharmProjects\\praca_dyplomowa\\venv\\Scripts\\python.exe";
            start.Arguments = string.Format("{0}", cmd);
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            using(System.Diagnostics.Process process = System.Diagnostics.Process.Start(start))
            {
                using(StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.Write(result);
                }
            }
        }
    }
}
