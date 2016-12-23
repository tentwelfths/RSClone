using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayLog : MonoBehaviour {

    private static string[] Log = new string[10];
    private static string FinalLog;
    public UnityEngine.UI.Text LogDisplay;

    // Use this for initialization
    void Start () {
        for(int i = 0; i<Log.Length; i++)
        {
            Log[i] = " ";
        }
        LogMessage("Welcome to Prune's Scrape");
	}

    public static void LogMessage(string newMessage)
    {
        FinalLog = "";
        for(int i = 0; i<Log.Length-1; i++)
        {
            Log[i] = Log[i + 1];
        }
        Log[Log.Length-1] = newMessage;

        for(int i = 0; i < Log.Length; i++)
        {
            FinalLog += Log[i] + "\n";
        }
    }

    private void Update()
    {
        LogDisplay.text = FinalLog;
    }
}
