using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour {

    public UnityEngine.UI.InputField nameField;

	// Use this for initialization
	void Start () {
        nameField.text = PlayerPrefs.GetString("playerName", "Ararat");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetName(string name)
    {
        PlayerPrefs.SetString("playerName", name);
    }
}
