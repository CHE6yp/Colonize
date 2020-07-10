﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour {

    [SyncVar]
    public string playerName = "";
    public int colorInt = 0;

    MapManager mapManager;

    public bool turn;

    public enum PlayerPhase { Inactive, RollDices, MoveRobber, TurnPhase}
    public PlayerPhase playerPhase;
    

    public delegate void PlayerEvent();
    public PlayerEvent endTurn;
    public PlayerEvent wealthChange;


    public int winPoints = 0;

    public int towns = 2;
    public int roads = 2;
    public int cities = 2;

    //wood sheep wheat brick stone
    public int[] wealth = new int[5] { 0, 0, 0, 0, 0 };
    public bool[] ports = new bool[6];
    public bool rolledDice;


    // Use this for initialization
    void Start() {

        mapManager = MapManager.singleton;
    }


    public void StartGame()
    {
        Cmd_StartGame();
    }

    [Command]
    public void Cmd_StartGame()
    {
        MatchManager.singleton.Rpc_StartGame(System.Environment.TickCount);
    }

    public void RollDice()
    {
        if (playerPhase == PlayerPhase.RollDices)
            Cmd_RollDice();
    }

    [Command]
    public void Cmd_RollDice()
    {
        if (rolledDice || !turn)
            return;

        Rpc_RollDice(System.Environment.TickCount);

    }

    [ClientRpc]
    public void Rpc_RollDice(int seed)
    {
        Random.InitState(seed);

        if (Dices.singleton.Roll() == 7)
            playerPhase = PlayerPhase.MoveRobber;
        else
            playerPhase = PlayerPhase.TurnPhase;   

        rolledDice = true;

    }





    public void EndTurn()
    {
        if (playerPhase == PlayerPhase.TurnPhase)
            Cmd_EndTurn();
    }

    [Command]
    public void Cmd_EndTurn()
    {
        if (!rolledDice || !turn)
        {
            Debug.Log("Have to roll dices!");
            return;
        }

        Rpc_EndTurn();
    }

    [ClientRpc]
    public void Rpc_EndTurn()
    {
        EndTurnFinal();
    }

    public void EndTurnFinal()
    {
        turn = false;
        rolledDice = false;
        if (endTurn != null)
            endTurn();
    }



    //Robbers

    public void RobbingCheck(bool heRolledDice)
    {
        //if wealthE >7 then drop shit
        if (wealth[0]+ wealth[1]+ wealth[2]+ wealth[3]+ wealth[4] > 7)
        {
            //dropshit
        }
        else
        {
            //if ()
        }
    }
    



    [Command]
    public void Cmd_MoveRobbersTo(int hexId)
    {
        Rpc_MoveRobbersTo(hexId);
    }

    [ClientRpc]
    public void Rpc_MoveRobbersTo(int hexId)
    {
        if (playerPhase == PlayerPhase.MoveRobber)
            Robber.MoveTo(mapManager.hexes[hexId]);
    } 



    //Build

    public void ManageTown(int townId)
    {
        if (!rolledDice)
        {
            Log.Add("Have to roll dices!");
            return;
        }

        if (mapManager.towns[townId].empty)
        {
            if (mapManager.towns[townId].available && mapManager.towns[townId].whoCanBuild.Contains(this))
            {
                if (towns > 0)
                {
                    Cmd_BuildTown(townId);
                }
                else
                {
                    Log.Add("Нет поселений.");
                }
            }
            else
            {
                //You can't build here
                Log.Add("Вы не можете строить тут!");
            }
        }
        else
        {
            if (mapManager.towns[townId].owner==this)
            {
                if (cities > 0)
                {
                    //Build City. cities--
                    Cmd_TurnIntoCity(townId);
                }
                else
                {
                    //no cities
                    Log.Add("Нет городов.");
                }
            }
            else
            {
                string citown = (mapManager.towns[townId].isCity) ? "город!" : "поселение...";
                Log.Add(mapManager.towns[townId].owner.playerName + " там уже построил "+citown);
            }
        }
    }

    [Command]
    public void Cmd_BuildTown(int townId)
    {

        Rpc_BuildTown(townId);

        /*
        if (towns > 0 && mapManager.towns[townId].empty && 
            mapManager.towns[townId].available && mapManager.towns[townId].whoCanBuild.Contains(this))
        {
            Rpc_BuildTown(townId);
        }
        else if (towns <= 0)
            Debug.Log("No available towns - " + name);
        else
            Debug.Log("Can't build town there - " + name);
        */
    }

    [ClientRpc]
    public void Rpc_BuildTown(int townId)
    {

        mapManager.towns[townId].PlaceTown(this);

        towns--;
        winPoints++;
        wealthChange();
        
    }

    [Command]
    public void Cmd_TurnIntoCity(int townId)
    {
        Rpc_TurnIntoCity(townId);
    }

    [ClientRpc]
    public void Rpc_TurnIntoCity(int townId)
    {

        mapManager.towns[townId].TurnIntoCity();

        cities--;
        winPoints++;
        wealthChange();


    }



    [Command]
    public void Cmd_BuildRoad(int roadId)
    {
        if (!rolledDice)
        {
            Debug.Log("Have to roll dices!");
            return;
        }

        if (roads > 0 && mapManager.roads[roadId].empty && mapManager.roads[roadId].whoCanBuild.Contains(this))
        {
            Rpc_BuildRoad(roadId);
        }
        else if (roads <= 0)
            Debug.Log("No available roads - " + name);
        else
            Debug.Log("Can't build road there - " + name);

    }

    [ClientRpc]
    public void Rpc_BuildRoad(int roadId)
    {
        mapManager.roads[roadId].PlaceRoad(this);
        roads--;
        wealthChange();

    }


    //START PHASE-======================-=-=--=-=-=-=-=-
    [Command]
    public void Cmd_BuildTownWithoutAllow(int townId)
    {
        if (towns > 0 && mapManager.towns[townId].empty && mapManager.towns[townId].available)
        {
            Rpc_BuildTownWithoutAllow(townId);
            
        }
        else if (towns <= 0)
            Debug.Log("No available towns - " + name);
        else
            Debug.Log("Can't build town there - " + name);

    }

    [ClientRpc]
    public void Rpc_BuildTownWithoutAllow(int townId)
    {
        
        mapManager.towns[townId].PlaceTownWithoutAllow(this);
        towns--;
        winPoints++;
        wealthChange();
        GetComponent<PlayerController>().placedTown = true;
    }

    //основной? потом то будет ивент
    [Command]
    public void Cmd_BuildRoadBool(int roadId)
    {
        //return 
        if (roads > 0 && mapManager.roads[roadId].empty && mapManager.roads[roadId].whoCanBuild.Contains(this))
        {
            Rpc_BuildRoadBool(roadId);
        
            

            
        }
    
        else if (roads <= 0)
            Debug.Log("No available roads - " + name);
        else
            Debug.Log("Can't build road there - " + name);
           


    }

    [ClientRpc]
    public void Rpc_BuildRoadBool(int roadId)
    {
        Debug.Log("HEY");
        mapManager.roads[roadId].PlaceRoad(this);
        
        roads--;
        wealthChange();

        GetComponent<PlayerController>().placedTown = false;
        EndTurnFinal();
        if (GetComponent<PlayerController>().startClickCount == 1)
            GetComponent<PlayerController>().click = GetComponent<PlayerController>().Click;
        GetComponent<PlayerController>().startClickCount++;

    }

    #region netShit
    //==============================
    public override void OnStartClient()
    {
        base.OnStartServer();

        Debug.Log("CONNECTION!");
        
        StartCoroutine(DelayedRegistration());
    }

    //MatchManager.singleton почемуто появляется после того как появляются игроки. 
    //Потому дабы добавить хоста, нужно откладывать регистрацию пока не создаться синглтон
    private IEnumerator DelayedRegistration()
    {
        while (MatchManager.singleton == null)
        {
            yield return null;
        }
        MatchManager.singleton.players.Add(this);
        colorInt = MatchManager.singleton.playerCount;
        MatchManager.singleton.playerCount++;

        //wealthChange = DRAWTHEFUCKINGWEATH;
        //wealthChange = panel.UpdateWealth;

        
        yield return new WaitForSeconds(0.3f);
        if (isLocalPlayer)
        {
            
            Bank.singleton.localPlayer = this;
            MatchUI.localPlayer = this;
            Log.localPlayer = this;
            Trading.localPlayer = this;

            AssignDelegatesUI();
            Debug.Log("ASSINGDELEGALES " + colorInt);
            Cmd_SetName(PlayerPrefs.GetString("playerName", "Ararat"));
            //wealthChange = MatchUI.singleton.DrawWealth;

        }
        
        //CmdSwitchName(FindObjectOfType<Params>().playerName);
    }

    [Command]
    void Cmd_SetName(string name)
    {
        Rpc_SetName(name);
    }

    [ClientRpc]
    void Rpc_SetName(string name)
    {
        playerName = name;
        gameObject.name = playerName + " (Player)";
        Log.Add(Log.Tag.Other, playerName + " connected.");
    }


    void AssignDelegatesUI()
    {
        if (isServer)
        {
            MatchUI.singleton.startGameUI = StartGame;
        }
        MatchUI.singleton.endTurnUI = EndTurn;
        MatchUI.singleton.rollDiceUI = RollDice;

        MatchUI.singleton.buyButtonUI = BankBuy;
        
    }
    #endregion



    //==============BANK
    public void BankBuy()
    {
        int buy = Bank.singleton.buy;
        int sell = Bank.singleton.sell;

        if (!rolledDice)
            return;
        if (buy<5)
            Cmd_BankBuyResource(buy, sell);
        if (buy >= 5 && buy != 8)
            Cmd_BankBuyPiece(buy);
    }

    [Command]
    public void Cmd_BankBuyResource(int buy, int sell)
    {
        Debug.Log("CmdBuy " + buy + " " + sell);
        Bank.singleton.Rpc_BankBuyResource(buy, sell, colorInt);
    }

    [Command]
    public void Cmd_BankBuyPiece(int buy)
    {
        Debug.Log("CmdBuy " + buy);
        Bank.singleton.Rpc_BankBuyPiece(buy, colorInt);
    }

    //LOG
    public void SendChatMessage(string message)
    {
        Cmd_SendChatMessage(playerName + ": " + message);
    }

    [Command]
    public void Cmd_SendChatMessage(string message)
    {
        Rpc_SendChatMessage(message);
    }

    [ClientRpc]
    public void Rpc_SendChatMessage(string message)
    {
        Log.Add(Log.Tag.Chat, message);
    }



    //TRADING
    public void TradeRequest(int id)
    {
        Debug.Log("TradeRequestSending");
        Cmd_TradeRequest(colorInt, id);
    }

    [Command]
    public void Cmd_TradeRequest(int senderID, int recieverID)
    {
        Rpc_TradeRequest(senderID, recieverID);
    }

    [ClientRpc]
    public void Rpc_TradeRequest(int senderID, int recieverID)
    {
        Debug.Log(senderID +"sent request to "+recieverID+". You are "+colorInt);

        foreach (Player p in MatchManager.singleton.players)
        {
            if (p.colorInt == recieverID)
                p.RecieveTradeRequest(senderID);
        }

        
    }

    public void RecieveTradeRequest(int senderID)
    {
        if (!isLocalPlayer)
            return;
        Debug.Log("Got trade request form " + senderID);
        Trading.singleton.RecieveTradeRequest(senderID);
    }


    //отклонение предложения
    public void DenyTradeRequest(int senderID)
    {
        Cmd_DenyTradeRequest(senderID, colorInt);
    }

    [Command]
    public void Cmd_DenyTradeRequest(int senderID, int recieverID)
    {
        Rpc_DenyTradeRequest(senderID, recieverID);
    }

    [ClientRpc]
    public void Rpc_DenyTradeRequest(int senderID, int recieverID)
    {
        foreach (Player p in MatchManager.singleton.players)
        {
            if (p.colorInt == senderID)
                p.RecieveTradeRequestDenial(senderID);
        }
        
    }

    public void RecieveTradeRequestDenial(int senderID)
    {
        if (!isLocalPlayer)
            return;
        Debug.Log("Got trade request denied by " + senderID);
        Trading.singleton.RecieveDenial(senderID);
    }

    //принятие предложения
    public void AcceptTradeRequest(int partnerId)
    {
        Cmd_AcceptTradeRequest(partnerId, colorInt);
    }

    [Command]
    public void Cmd_AcceptTradeRequest(int senderID, int recieverID)
    {
        Rpc_AcceptTradeRequest(senderID, recieverID);
    }

    [ClientRpc]
    public void Rpc_AcceptTradeRequest(int senderID, int recieverID)
    {
        foreach (Player p in MatchManager.singleton.players)
        {
            if (p.colorInt == senderID)
                p.RecieveTradeRequestAcceptance(senderID);
        }
    }

    public void RecieveTradeRequestAcceptance(int senderID)
    {
        if (!isLocalPlayer)
            return;
        Debug.Log("Got trade request accepted by " + senderID);
        //Trading.singleton.RecieveAcceptance(senderID);
        Trading.singleton.RecieveAcceptance();
    }


    //offers

    public void UpdateOffer(int id, int res, int amount)
    {
        Cmd_UpdateOffer(id, res, amount);
    }

    [Command]
    public void Cmd_UpdateOffer(int id, int res, int amount)
    {
        Rpc_UpdateOffer(id, res, amount);
    }

    [ClientRpc]
    public void Rpc_UpdateOffer(int id, int res, int amount)
    {
        foreach (Player p in MatchManager.singleton.players)
        {
            if (p.colorInt == id)
                p.RecieveUpdatedOffer(res, amount);
        }
        
    }

    public void RecieveUpdatedOffer(int res, int amount)
    {
        if (isLocalPlayer)
            Trading.singleton.UpdatePartnersResourceOffer(res, amount);
    }

    //ready

    public void UpdateReady(int id, bool ready)
    {
        Cmd_UpdateReady(id, ready);
    }

    [Command]
    public void Cmd_UpdateReady(int id, bool ready)
    {
        Rpc_UpdateReady(id, ready);
    }

    [ClientRpc]
    public void Rpc_UpdateReady(int id, bool ready)
    {
        foreach (Player p in MatchManager.singleton.players)
        {
            if (p.colorInt == id)
                p.RecieveUpdateReady(ready);
        }

    }

    public void RecieveUpdateReady(bool ready)
    {
        if (isLocalPlayer)
            Trading.singleton.PartnersReady(ready);
    }


    public void Exchange(int tpID, int[] tpOffer, int[] yourOffer)
    {
        Cmd_Exchange( tpID,  tpOffer,  yourOffer);
    }

    [Command]
    public void Cmd_Exchange(int tpID, int[] tpOffer, int[] yourOffer)
    {
        Rpc_Exchange( tpID,  tpOffer,  yourOffer);
    }

    [ClientRpc]
    public void Rpc_Exchange(int tpID, int[] tpOffer, int[] yourOffer)
    {
        Player partner = new Player();
        foreach (Player p in MatchManager.singleton.players)
            if (p.colorInt == tpID)
                partner = p;

        for (int i = 0; i < 5; i++)
        {
            wealth[i] -= yourOffer[i];
            partner.wealth[i] -= tpOffer[i];

            wealth[i] += tpOffer[i];
            partner.wealth[i] += yourOffer[i];

        }

        wealthChange();
        partner.wealthChange();

        if (partner.isLocalPlayer)
        {
            Trading.singleton.FinishExchange();
        }
    }




    //DevCards
    [Command]
    public void Cmd_PlayKnight()
    {
        Rpc_PlayKnight();
    }

    [ClientRpc]
    public void Rpc_PlayKnight()
    {
        if (turn)
            DevCards.PlayKnight(this);
    }



}
