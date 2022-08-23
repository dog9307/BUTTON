using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class ScreenShot : MonoBehaviour
{
    [SerializeField]
    private string _screenShotDirectoryPath = "";
    private string _standardPath = "Resources";

    private bool _isDone = true;

    void Start()
    {
        _isDone = true;
    }

    void Update()
    {
        if (!_isDone) return;

        if (Input.GetKeyDown(KeyCode.F9))
        {
            _isDone = false;
            StartCoroutine(CaptureScreen());
        }

        if (Input.GetKeyDown(KeyCode.F10))
            PrintImage();
    }

    public IEnumerator CaptureScreen()
    {
        // Wait till the last possible moment before screen rendering to hide the UI
        yield return null;

        VideoPlayer video = FindObjectOfType<VideoPlayer>();
        video.Pause();
        video.enabled = false;

        // Wait for screen rendering to complete
        yield return new WaitForEndOfFrame();

        // Find Directory
        string path = Application.dataPath + "/" + _standardPath + "/" + _screenShotDirectoryPath;
        DirectoryInfo dirInfo = new DirectoryInfo(path);
        if (!dirInfo.Exists)
            Directory.CreateDirectory(path);

        // Take screenshot
        ScreenCapture.CaptureScreenshot(path + "/" + "screenshot.png");
        Texture2D tex = ScreenCapture.CaptureScreenshotAsTexture();
        SetTexture(tex);

        // Show UI after we're done
        video.enabled = true;
        video.Play();

        yield return new WaitForEndOfFrame();

        _isDone = true;
    }

    public void SetTexture()
    {
        Theme4RewardHelper reward = FindObjectOfType<Theme4RewardHelper>();
        if (reward)
        {
            Texture tex = Resources.Load<Texture>(_screenShotDirectoryPath + "/" + "screenshot");
            reward.SetMainTex(tex);
        }
    }

    public void SetTexture(Texture tex)
    {
        Theme4RewardHelper reward = FindObjectOfType<Theme4RewardHelper>();
        if (reward)
            reward.SetMainTex(tex);
    }

    void PrintImage()
    {
        string printerName = "Brother PT-P700";
        string path = Application.dataPath + "/" + _standardPath + "/" + _screenShotDirectoryPath + "/" + "screenshot";
        string fullCommand = "rundll32 C:\\WINDOWS\\system32\\shimgvw.dll,ImageView_PrintTo " + "\"" + path + "\"" + " " + "\"" + printerName + "\"";
        PrintImage(fullCommand);
    }

    void PrintImage(string _cmd)
    {
        try
        {
            Process myProcess = new Process();
            //myProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            myProcess.StartInfo.CreateNoWindow = true;
            myProcess.StartInfo.UseShellExecute = false;
            myProcess.StartInfo.FileName = "cmd.exe";
            myProcess.StartInfo.Arguments = "/c " + _cmd;
            myProcess.EnableRaisingEvents = true;
            myProcess.Start();
            myProcess.WaitForExit();
        }
        catch (Exception e)
        {
            UnityEngine.Debug.Log(e);
        }
    }
}
