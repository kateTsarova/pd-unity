using UnityEngine;
using System.IO;
using System;
using UnityEditor;
using UnityEngine.UI;

public class GenerateCode : MonoBehaviour
{
    private string path;
    public Button generate_btn;

    public void ConnectWithPython()
    {
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
    public void SavePNG()
    {
        string path = EditorUtility.OpenFolderPanel("Select Directory", "", "");
        if(path.Length > 0){
            ScreenCapture.CaptureScreenshot(path + "/out.png");
        }

        generate_btn.interactable = true;
    }
}
