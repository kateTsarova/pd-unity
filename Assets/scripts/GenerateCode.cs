using UnityEngine;
using System.IO;
using System;
using UnityEditor;
using UnityEngine.UI;

public class GenerateCode : MonoBehaviour
{
    private string path;
    public Button generate_btn;

    public string AddQuotesIfRequired(string path)
    {
        return !string.IsNullOrWhiteSpace(path) ? 
            path.Contains(" ") && (!path.StartsWith("\"") && !path.EndsWith("\"")) ? 
                "\"" + path + "\"" : path : 
                string.Empty;
    }

    public void ConnectWithPython()
    {
        string cmd = AddQuotesIfRequired(Directory.GetCurrentDirectory() + "\\python\\sample.py");
        // string cmd = "C:\\Users\\Nata\\Desktop\\python\\sample.py";
        System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo();
        start.FileName = "python.exe";
        start.Arguments = string.Format("{0} {1}", cmd, path);
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

            generate_btn.interactable = true;
        }
    }
}
