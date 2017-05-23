using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trading : MonoBehaviour {

    public static Trading singleton;
    public static TradingUI ui;
    public static Player localPlayer;

    public bool isTrading;
    public int traderID;


    //trade
    public List<Player> otherPlayers = new List<Player>();




    private void Awake()
    {
        singleton = this;
    }



    //TRADING
    public void UpdateTradeOptions()
    {
        otherPlayers.Clear();
        

        foreach (Player p in MatchManager.singleton.players)
        {
            if (p != localPlayer)
            {
               
                if (localPlayer.turn || p.turn)
                {
                    otherPlayers.Add(p);
                }
            }
        }

        ui.UpdateTradeOptions(otherPlayers);
    }

    public void SendTradeRequest()
    {
        if (isTrading)
            return;

        localPlayer.TradeRequest(otherPlayers[ui.tradePlayersDropdown.value].colorInt);
        isTrading = true;
        ui.SwitchTradeStatus(0);
    }

    public void RecieveTradeRequest(int senderID)
    {
        if (isTrading)
        {
            DenyTradeRequest(senderID);
            return;
        }

        ui.SwitchTradeStatus(1);
        isTrading = true;
        traderID = senderID;
        Log.Add(MatchManager.singleton.players[senderID].playerName + " хочет поторговаться");
    }

    public void DenyTradeRequest()
    {
        isTrading = false;
        localPlayer.DenyTradeRequest(traderID);
        ui.TradeStatusHide();
    }

    public void DenyTradeRequest(int senderID)
    {
        //isTrading = false;
        localPlayer.DenyTradeRequest(senderID);
        //ui.TradeStatusHide();
    }

    public void RecieveDenial(int id)
    {
        isTrading = false;
        ui.SwitchTradeStatus(2);
    }
}
