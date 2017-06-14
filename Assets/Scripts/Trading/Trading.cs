using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trading : MonoBehaviour {

    public static Trading singleton;
    public static TradingUI ui;
    public static Player localPlayer;

    //trade
    public List<Player> otherPlayers = new List<Player>();

    public bool isTrading;
    public int tradePartnerID;
    public string tradePartnerName;

    public int[] partnerOffer = new int[5];
    public int[] yourOffer = new int[5];

    public bool yourReady;
    public bool partnersReady;
    




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

        tradePartnerID = otherPlayers[ui.tradePlayersDropdown.value].colorInt;
        tradePartnerName = otherPlayers[ui.tradePlayersDropdown.value].playerName;

        localPlayer.TradeRequest(tradePartnerID);
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
        tradePartnerID = senderID;
        tradePartnerName = MatchManager.singleton.players[senderID].playerName;


        Log.Add(MatchManager.singleton.players[senderID].playerName + " хочет поторговаться");
    }

    //то же что и CancelTradeRequest?!
    public void DenyTradeRequest()
    {
        isTrading = false;
        localPlayer.DenyTradeRequest(tradePartnerID);
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
        //if (id == tradePartnerID) //неуверен насколько это условие нужно
        
            isTrading = false;
            ui.SwitchTradeStatus(2);
       
    }

    public void CancelTradeRequest()
    {
        if (!isTrading)
            return;

        isTrading = false;
        ui.TradeStatusHide();
        localPlayer.DenyTradeRequest(tradePartnerID);
    }

    public void AcceptTradeRequest()
    {
        localPlayer.AcceptTradeRequest(tradePartnerID);
        ui.SwitchTradeStatus(3);
        ClearOffers();
    }

    public void RecieveAcceptance()
    {
        ui.SwitchTradeStatus(3);
        ClearOffers();
    }

    public void ClearOffers()
    {
        for (int i = 0; i < 5; i++)
        {
            yourOffer[i] = 0;
            partnerOffer[i] = 0;
            ui.UpdateYourResourceOffer(i);
            ui.UpdatePartnersResourceOffer(i);
        }
    }

    public void IncreaseOfferAmount(int resource)
    {
        if (yourOffer[resource] < localPlayer.wealth[resource])
        {
            yourOffer[resource]++;
            UpdateYourResourceOffer(resource);
            //UpdateOffer
        }
    }

    public void DecreaseOfferAmount(int resource)
    {
        if (yourOffer[resource] > 0)
        {
            yourOffer[resource]--;
            UpdateYourResourceOffer(resource);
            //UpdateOffer
        }
    }

    public void UpdateYourResourceOffer(int resource)
    {
        ui.UpdateYourResourceOffer(resource);
        localPlayer.UpdateOffer(tradePartnerID, resource, yourOffer[resource]);
        DeclareReady(false);
    }

    public void UpdatePartnersResourceOffer(int resource, int amount)
    {
        partnerOffer[resource] = amount;
        ui.UpdatePartnersResourceOffer(resource);
        DeclareReady(false);
    }


    public void DeclareReady(bool yourToggle)
    {
        yourReady = yourToggle;
        localPlayer.UpdateReady(tradePartnerID, yourReady);
        if (!yourReady)
        {
            ui.readyToggle.isOn = false;

            
        }
        else
        {
            if (partnersReady)
                Exchange();
        }
        
    }

    public void PartnersReady(bool partnersToggle)
    {
        partnersReady = partnersToggle;
        ui.partnersReadyText.gameObject.SetActive(partnersReady);
        //if (yourReady && partnersReady)
        //    Exchange();
    }

    /// <summary>
    /// тут надо както делать рпс запросы, дабы у игроков которые не участвую в торговле тоже менялось количество ресурсов у обменявшихся игроков
    /// </summary>
    public void Exchange()
    {

        localPlayer.Exchange(tradePartnerID, partnerOffer, yourOffer);

        FinishExchange();

    }

    public void ExchangeOld()
    {
        Player partner = new Player();
        foreach (Player p in MatchManager.singleton.players)
            if (p.colorInt == tradePartnerID)
                partner = p;

        for (int i = 0; i < 5; i++)
        {
            localPlayer.wealth[i] -= yourOffer[i];
            partner.wealth[i] -= partnerOffer[i];

            localPlayer.wealth[i] += partnerOffer[i];
            partner.wealth[i] += yourOffer[i];

        }

        ui.SwitchTradeStatus(4);
        isTrading = false;

        localPlayer.wealthChange();
        partner.wealthChange();


    }

    /// <summary>
    /// When all the good shit is done and you just need to switch your ui and shit
    /// </summary>
    public void FinishExchange()
    {
        ui.SwitchTradeStatus(4);
        isTrading = false;
    }



}
