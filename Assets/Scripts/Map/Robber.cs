﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robber : MonoBehaviour {

    public static Robber singleton;

    public static Hex currentHex;

    public static bool hasToMove;

	// Use this for initialization
	void Awake () {
        singleton = this;
	}
	
    public static bool MoveTo(Hex otherHex)
    {
        //нельзя двигать на ту же этсамую
        if (currentHex == otherHex)
            return false;

        Debug.Log("GopStop!");

        currentHex.hasRobbers = false;
        SetAt(otherHex);

        hasToMove = false;

        if (MatchManager.singleton.players[MatchManager.singleton.activePlayerId].rolledDice)
            MatchManager.singleton.players[MatchManager.singleton.activePlayerId].playerPhase = Player.PlayerPhase.TurnPhase;
        else
            MatchManager.singleton.players[MatchManager.singleton.activePlayerId].playerPhase = Player.PlayerPhase.RollDices;

        return true;
    }

    public static void SetAt(Hex hex)
    {
        currentHex = hex;
        currentHex.hasRobbers = true;

        singleton.transform.position = currentHex.transform.position + new Vector3(0,0,-.2f);
    }
}
