using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[SelectionBase]
[System.Serializable]
public class Hex : MonoBehaviour {

    public int id = 0;

    public SpriteRenderer hexRend;
    [SerializeField]
    SpriteRenderer resRend;
    public Sprite[] resSprite = new Sprite[0];
    public int resNumber = 0;
    public bool desert;
    public bool hasRobbers;

    

    public int number = 0;
    [SerializeField]
    public SpriteRenderer numRend;
    public Sprite[] numSprite = new Sprite[0];

    public List<Town> towns = new List<Town>();

    //ивенты. когда выпадает число хекса
    public delegate void HexEvent();
    public HexEvent scoreHex;


    // Use this for initialization
    void Awake () {

	}

    public void ScoreHex()
    {
        if (!hasRobbers)

            foreach(Town t in towns)
            {
                if (!t.empty)
                {
                    t.ScoreTown(resNumber);
                }
            }
    }

    public void ClearHexSelection()
    {
        hexRend.color = Color.white;
    }

    public void SelectHex()
    {
        hexRend.color = Color.cyan;
    }

    public void SetupHex(int resNumT, int numT)
    {
        resNumber = resNumT;
        resRend.sprite = resSprite[resNumber];

        number = numT;
        if (number < 7)
            numRend.sprite = numSprite[number - 2];
        else
            numRend.sprite = numSprite[number - 3];
    }
}


