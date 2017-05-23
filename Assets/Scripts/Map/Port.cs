using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Port : MonoBehaviour {

    public SpriteRenderer rend;
    public Town[] towns = new Town[0];
    public int resNum;
    public Sprite[] sprites = new Sprite[0];

	// Use this for initialization
	void Start () {
		
	}
	


    public void SetupPort(int i)
    {
        resNum = i;
        foreach (Town t in towns)
        {
            t.isPort = true;
            t.portRes = resNum;
        }
        

        rend.sprite = sprites[resNum];
    }

}
