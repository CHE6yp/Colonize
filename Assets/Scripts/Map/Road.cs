using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Road : MonoBehaviour {

    public int id = 0;

    public bool empty = true;
    [HideInInspector]
    public SpriteRenderer roadRenderer;

    public Town[] towns;

    public Player owner;

    public Sprite[] sprites;

    public List<Player> whoCanBuild = new List<Player>();
    

    // Use this for initialization
    void Start () {
		foreach (Town town in towns)
        {
            town.roads.Add(this);
        }
	}

    public bool PlaceRoad(Player player)
    {

        empty = false;
        owner = player;
        roadRenderer.sprite = sprites[player.colorInt];
        roadRenderer.gameObject.SetActive(true);

        foreach (Town t in towns)
        {
            if (!t.whoCanBuild.Contains(player))
                t.whoCanBuild.Add(player);
            t.AllowNearbyRoads(player);
        }


        Log.Add(Log.Tag.Map, player.playerName + " placed a road!");
        return true;
    }

    public Town OtherTown(Town thisTown)
    {
        return (thisTown == towns[0]) ? towns[1] : towns[0];
    }

    private void OnMouseEnter()
    {
        if (empty)
            roadRenderer.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (empty)
            roadRenderer.gameObject.SetActive(false);
    }
}
