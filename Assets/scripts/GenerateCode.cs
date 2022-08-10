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
        // string cmd = "D:\\again\\pix2code-master\\pix2code-master\\model\\sample.py";
        // string cmd = "C:\\Users\\Nata\\Desktop\\sample.py";
        string cmd = AddQuotesIfRequired(Directory.GetCurrentDirectory() + "\\Assets\\python\\sample.py");
        // string cmd = AddQuotesIfRequired(@"D:\\semestr 7\\test.py");
        // cmd = AddQuotesIfRequired(cmd);
        // string cmd = "C:\\Users\\Nata\\Desktop\\out\\test.py";
        Debug.Log(cmd);
        // string cmd = "C:\\Users\\Nata\\Desktop\\test.py";
        System.Diagnostics.ProcessStartInfo start = new System.Diagnostics.ProcessStartInfo();
        start.FileName = "python.exe";
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

            generate_btn.interactable = true;
        }
    }
}
