using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Town : MonoBehaviour {

    public Player owner;
    public int id = 0;

    public bool empty = true;
    [HideInInspector]
    public SpriteRenderer townRenderer;
    [HideInInspector]
    public SpriteRenderer cityRenderer;

    //public int hexCount = 3;
    public List<Hex> hexes = new List<Hex>();

    public bool isCity;

    public bool isPort;
    public int portRes;

    public bool available = true;
    
    public Sprite[] sprites;

    
    public List<Player> whoCanBuild = new List<Player>();
    //===
    public List<Road> roads = new List<Road>();


    // Use this for initialization
    void Start () {

        //подписались на ивент
        foreach (Hex h in hexes)
        {
            //h.scoreHex += ScoreTown;
            h.towns.Add(this);
        }

	}

    public bool PlaceTown(Player player)
    {
        if (!empty||!available || !whoCanBuild.Contains(player))
            return false;

        available = false;
        empty = false;
        owner = player;
        townRenderer.sprite = sprites[player.colorInt];
        townRenderer.gameObject.SetActive(true);

        //запрет на строительство впритык к поселению
        foreach (Road r in roads)
        {
            r.OtherTown(this).available = false;
        }
        AllowNearbyRoads(player);

        if (isPort)
            player.ports[portRes] = true;

        Log.Add(Log.Tag.Map, player.playerName + " placed a town!");
        return true;
        
    }

    public bool PlaceTownWithoutAllow(Player player)
    {
        if (!empty||!available )
            return false;

        whoCanBuild.Add(player);

        available = false;
        empty = false;
        owner = player;
        townRenderer.sprite = sprites[player.colorInt];
        townRenderer.gameObject.SetActive(true);

        //запрет на строительство впритык к поселению
        foreach (Road r in roads)
        {
            r.OtherTown(this).available = false;
        }
        AllowNearbyRoads(player);

        if (MatchManager.singleton.secondRoundSPH)
            foreach (Hex h in hexes)
            {
                player.wealth[h.resNumber]++;
            }

        if (isPort)
            player.ports[portRes] = true;

        Log.Add(Log.Tag.Map, player.playerName + " placed a town!");
        return true;

    }

    public bool TurnIntoCity()
    {
        isCity = true;
        Log.Add(Log.Tag.Map, owner.playerName + " placed a city!");
        cityRenderer.gameObject.SetActive(true);
        return true;
    }

    public void ScoreTown(int resNumber)
    {
        string gotSome = " got 1 {";
        owner.wealth[resNumber]++;
        if (isCity)
        {
            owner.wealth[resNumber]++;
            gotSome = " got 2 {";
        }
        owner.wealthChange();
        Log.Add(Log.Tag.Map, string.Format(owner.playerName + gotSome + resNumber.ToString() + "}!", "wood", "sheep", "wheat", "brick", "stone"));
    }

    public void AllowNearbyRoads(Player player)
    {
        foreach (Road r in roads)
        {
            if (!r.whoCanBuild.Contains(player))
                r.whoCanBuild.Add(player);
            
        }
    }

    private void OnMouseEnter()
    {
        if (empty)
            townRenderer.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (empty)
            townRenderer.gameObject.SetActive(false);
    }
}
/// <summary>
/// x - 0.7; y - 0.2;0.4;
/// start x = -0.7 ; y = 1.6;
/// </summary>