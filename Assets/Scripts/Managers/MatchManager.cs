using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MatchManager : NetworkBehaviour {
    
    public static MatchManager singleton;

    public int playerCount = 0;

    public delegate void MatchEvent();
    
    public MatchEvent newTurn;
   

    public List<Player> players = new List<Player>();
    public int activePlayerId = 0;


    public bool secondRoundSPH;

    // Use this for initialization

    private void Awake()
    {
        singleton = this;
    }


    [ClientRpc]
    public void Rpc_StartGame(int seed)
    {
        MapManager.singleton.RandomMap(seed);
        PlayersEndTurnEvent(Rpc_SwitchTurnStartPhase);
        players[0].rolledDice = true;
        players[0].turn = true;

        //newTurn += MatchUI.singleton.NewTurn;
        
        //MatchUI.singleton.AssignDrawWealth();

        MatchUI.singleton.SwitchCanvas(1);
        MatchUI.singleton.CreatePlayerPanels();
        Trading.singleton.UpdateTradeOptions();

        newTurn();
    }

    [ClientRpc]
    public void Rpc_SwitchTurn()
    {
        //Debug.Log("SWTRN!");
        players[activePlayerId].turn = false;
        activePlayerId = (activePlayerId + 1 == players.Count) ? 0 : activePlayerId + 1;

        players[activePlayerId].turn = true;
        players[activePlayerId].rolledDice = false;

        if (newTurn != null)
            //newTurn(players[activePlayerId]);
            newTurn();


        Trading.singleton.UpdateTradeOptions();
    }

    [ClientRpc]
    public void Rpc_SwitchTurnStartPhase()
    {
        //Debug.Log(activePlayerId);
        bool endStartPhase = false;
        Debug.Log("SWTRN START PHASE");

        players[activePlayerId].turn = false;

        if (!secondRoundSPH)
        {
            if (activePlayerId + 1 == players.Count)
                secondRoundSPH = true;
            else
                activePlayerId++;

        }
        else
        {
            if (activePlayerId == 0)
            {

                PlayersEndTurnEvent(Rpc_SwitchTurn);
                Debug.Log("VSE");
                endStartPhase = true;
            }
            else
                activePlayerId--;
        }

        players[activePlayerId].rolledDice = !endStartPhase;
        players[activePlayerId].turn = true;
        if (newTurn != null)
            //newTurn(players[activePlayerId]);
            newTurn();

    }
    //void EndTurnSubcribe(MatchEvent

    public void PlayersEndTurnEvent(Player.PlayerEvent switchTurn)
    {
        foreach (Player p in players)
            p.endTurn = switchTurn;
    }
}
