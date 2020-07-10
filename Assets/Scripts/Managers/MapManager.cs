using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MapManager : NetworkBehaviour {

    public static MapManager singleton;

    public Hex[] hexes = new Hex[19];
    public Town[] towns = new Town[56];
    public Road[] roads = new Road[72];
    public Port[] ports = new Port[9];

    public List<int> resList = new List<int>() { 0, 0, 0, 0, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 4, 4, 4 };
    public List<int> numList = new List<int>() { 2, 3, 3, 4, 4, 5, 5, 6, 6, 8, 8, 9, 9, 10, 10, 11, 11, 12 };
    public List<int> portList = new List<int>() { 0, 1, 2, 3, 4, 5, 5, 5, 5 };

    // Use this for initialization
    void Start () {
        singleton = this;

        AssignIds();
        MatchManager.singleton.newTurn += ClearHexes;
        //RandomMap();
	}

    void AssignIds()
    {
        int id = 0;
        foreach (Hex h in hexes)
        {
            h.id = id;
            id++;
        }

        id = 0;
        foreach (Town t in towns)
        {
            t.id = id;
            id++;
        }

        id = 0;
        foreach (Road r in roads)
        {
            r.id = id;
            id++;
        }


    }

    public void RandomMap(int seed)
    {
        Random.InitState(seed);
        foreach (Hex hex in hexes)
        {
            if (!hex.desert)
            {
                int tempR = Random.Range(0, resList.Count);
                int tempN = Random.Range(0, numList.Count);

                hex.SetupHex(resList[tempR], numList[tempN]);

                resList.RemoveAt(tempR);
                numList.RemoveAt(tempN);
            }
            else
            {
                hex.SetDesert();
                Robber.SetAt(hex);

            }
                
        }

        foreach (Port p in ports)
        {
            //Debug.Log(portList.Count);
            int t = Random.Range(0, portList.Count);
            //Debug.Log(portList.Count + " " + portList[t]);
            p.SetupPort(portList[t]);
            portList.RemoveAt(t);
        }
    }

    //можно переписать на евенты, но пока похуй впринципе
    public void ScoreHexes(int num)
    {
        foreach (Hex h in hexes)
        {
            if (h.number == num)
            {
                h.ScoreHex();
                h.SelectHex();
            }
        }
    }

    public void ClearHexes()
    {
        foreach (Hex h in hexes)
            h.ClearHexSelection();
    }
}
