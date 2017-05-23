using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Dices : MonoBehaviour {

    public static Dices singleton;

    public int dice1 = 0;
    public int dice2 = 0;
    public int diceSum = 0;

    public SpriteRenderer dice1rend;
    public SpriteRenderer dice2rend;

    public Sprite[] diceSprites;
    // Use this for initialization
    void Start () {
        singleton = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int Roll()
    {
        dice1 = Random.Range(1, 7);
        dice2 = Random.Range(1, 7);
        diceSum = dice1 + dice2;

        if (diceSum != 7)
            MapManager.singleton.ScoreHexes(diceSum);

        DrawDice();

        return diceSum;
    }

    void DrawDice()
    {
        dice1rend.sprite = diceSprites[dice1-1];
        dice2rend.sprite = diceSprites[dice2-1];
    }
}
