using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Bank : NetworkBehaviour {

    public static Bank singleton;
    public Player localPlayer;
    public int buy;
    public int sell;
    /// <summary>
    /// 0 - дерево, 1 - овца, 2 - зерно, 3 - кирпич, 4 - камень, 
    /// 5 - дорога, 6 - поселок, 7 будет город, 8 будет карта развития
    /// </summary>
	// Use this for initialization
	void Awake () {
        singleton = this;
	}

    
    [ClientRpc]
    public void Rpc_BankBuyResource(int buy, int sell, int pId)
    {
        Player player = MatchManager.singleton.players[pId];
        Debug.Log("RpcBuy " + buy + " " + sell);

        if (buy == sell)
        {
            Debug.Log("What?! Why?!");
            Log.Add(Log.Tag.Bank, string.Format(player.playerName + " пытался купить {" + buy + "} за {" + sell + "}...", "дерево", "овца", "зерно", "кирпич", "камень"));
            return;
        }

        int amount = CountAmount(sell);
        
        //проверка на нехватку ресурсов
        if (player.wealth[sell] >= amount)
        {
            player.wealth[buy]++;
            player.wealth[sell] -= amount;
            Log.Add(Log.Tag.Bank, string.Format(player.playerName + " купил {" + buy + "} за " + amount + " {" + sell + "}.", "дерево", "овца", "зерно", "кирпич", "камень"));
        }
        else
            Debug.Log("Not Enough");

        player.wealthChange();
    }

    [ClientRpc]
    public void Rpc_BankBuyPiece(int buy, int pId)
    {
        Player player = MatchManager.singleton.players[pId];
        //Debug.Log("RpcBuy " + buy + " " + sell);

        //road
        if (buy == 5)
        {
            if (player.wealth[0] > 0 && player.wealth[3] > 0)
            {
                player.roads++;
                player.wealth[0]--;
                player.wealth[3]--;

                string hehe = (Random.Range(0, 25) == 24) ? " Ну охуеть теперь." : "";
                
                Log.Add(Log.Tag.Bank, player.playerName + " купил дорогу!"+hehe);
            }
        }
        //town
        if (buy == 6)
        {

            if (player.wealth[0] > 0 && player.wealth[1] > 0 && player.wealth[2] > 0 && player.wealth[3] > 0)
            {
                player.towns++;
                player.wealth[0]--;
                player.wealth[1]--;
                player.wealth[2]--;
                player.wealth[3]--;

                Log.Add(Log.Tag.Bank, player.playerName + " купил поселок.");

            }
        }

        //city. 2 зерна и 3 камня
        if (buy == 7)
        {
            if (player.wealth[2] > 1 && player.wealth[4] > 2)
            {
                player.cities++;
                player.wealth[2]-=2;
                player.wealth[4]-=3;


                Log.Add(Log.Tag.Bank, player.playerName + " купил городок.");

            }
        }



        player.wealthChange();
    }

    [ClientRpc]
    public void Rpc_BankBuyCard(int buy, int sell, int pId)
    {
        Player player = MatchManager.singleton.players[pId];
        //Debug.Log("RpcBuyCard " + buy + " " + sell);



        player.wealthChange();
    }


    /// <summary>
    /// Counts how much resource would you have to sell to buy another resource
    /// </summary>
    /// <param name="sellRes">Which resource you want to sell</param>
    /// <returns></returns>
    public static int CountAmount(int sellRes)
    {

        int amount;


        if (MatchUI.localPlayer.ports[sellRes])//если есть спец порт
            amount = 2;
        else if (MatchUI.localPlayer.ports[5])//если общий порт
            amount = 3;
        else amount = 4; //если нет портов

        return amount;
    }

}
