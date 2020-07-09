using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogUI : MonoBehaviour {

    public Text logText;

    public InputField chatInput;
    public CanvasRenderer filterPanel;
    public List<Toggle> filterToggle = new List<Toggle>();
    public GameObject visible;
    //public Button chatButton;

    // Use this for initialization
    void Start () {
        Log.ui = this;
        int i = 0;
        foreach (Toggle t in filterToggle)
        {
            int s = i;
            t.onValueChanged.AddListener((value) => { Log.ChangeFilter((Log.Tag)s, value); });
            i++;
        }
    }

    public void SendChatMessage()
    {
        if (chatInput.text != "")
            Log.localPlayer.SendChatMessage(chatInput.text);
        chatInput.text = "";
    }

    public void SwitchFilterPanel()
    {
        filterPanel.gameObject.SetActive(!filterPanel.gameObject.activeSelf);
    }

    public void ToggleLog()
    {
        visible.SetActive(!visible.activeSelf);
    }
}
