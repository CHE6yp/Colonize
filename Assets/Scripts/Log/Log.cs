using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Log : MonoBehaviour {


    public static LogUI ui;

    public static Player localPlayer;

    public static string logString;
    

    public class LogMessage
    {
        public Tag tag;
        public string message;

        public LogMessage(Tag t, string m)
        {
            tag = t;
            message = m;
        }

    }
    public enum Tag { Chat, Map, Bank, Trade, Other };
    public static List<LogMessage> logList = new List<LogMessage>();
    public static List<Tag> filterTags = new List<Tag>() { Tag.Chat, Tag.Map, Tag.Bank, Tag.Trade, Tag.Other };


    // Use this for initialization
    void Start ()
    {
        ClearLog();
	}

    /// <summary>
    /// По умолчанию сообщения идут в other
    /// </summary>
    /// <param name="message"></param>
    public static void Add(string message)
    {
        LogMessage msg = new LogMessage(Tag.Other, message);
        logList.Add(msg);
        UpdateLog();
    }

    public static void Add(Tag tag, string message)
    {
        LogMessage msg = new LogMessage(tag, message);
        logList.Add(msg);
        UpdateLog();  
    }

    public static void UpdateLog()
    {
        logString = "";

        foreach (LogMessage lm in logList)
        {
            if (filterTags.Contains(lm.tag))
                logString+= '\n' + lm.message;
        }
        ui.logText.text = logString;

    }

    public void ClearLog()
    {
        logList.Clear();
        logString = "";
        ui.logText.text = "";
    }

    public static void ChangeFilter(Tag tag, bool set)
    {
        if (set)
            filterTags.Add(tag);
        else
            filterTags.Remove(tag);

        Debug.Log(tag.ToString() + " " + set.ToString());
        UpdateLog();
    }

}




