using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradingUI : MonoBehaviour {


    public Dropdown tradePlayersDropdown;


    public int currentTradeStatus = 0;
    public Canvas[] tradeStatusPanels = new Canvas[0];

    public Text[] tradePanelTexts = new Text[0];

    //offers
    public Text[] yourOffers = new Text[0];
    public Text[] partnerOffers = new Text[0];


    public Toggle readyToggle;
    public Text partnersReadyText;


    private void Awake()
    {
        Trading.ui = this;
    }

    public void UpdateTradeOptions(List<Player> otherPlayers)
    {
        tradePlayersDropdown.ClearOptions();

        foreach (Player p in otherPlayers)
        {
            Dropdown.OptionData op = new Dropdown.OptionData();
            op.text = p.playerName;
            tradePlayersDropdown.options.Add(op);
        }

        tradePlayersDropdown.RefreshShownValue();
    }

    public void SwitchTradeStatus(int i)
    {
        tradeStatusPanels[currentTradeStatus].gameObject.SetActive(false);
        currentTradeStatus = i;
        tradeStatusPanels[currentTradeStatus].gameObject.SetActive(true);

        TradeTextUpdate();
    }

    public void TradeTextUpdate()
    {
        tradePanelTexts[0].text = "Ожидание ответа от " + Trading.singleton.tradePartnerName + "...";
        tradePanelTexts[1].text = Trading.singleton.tradePartnerName + " предлагает вам обмен!";
        tradePanelTexts[2].text = Trading.singleton.tradePartnerName + " отказался от торговли.";
        if (Random.Range(0, 6) < 1)
            tradePanelTexts[2].text += " Как черт!";
        tradePanelTexts[3].text = Trading.singleton.tradePartnerName;
        

    }

    public void TradeStatusHide()
    {
        tradeStatusPanels[currentTradeStatus].gameObject.SetActive(false);
    }

    public void UpdateYourResourceOffer(int res)
    {
        yourOffers[res].text = Trading.singleton.yourOffer[res].ToString();
        
        
    }

    public void UpdatePartnersResourceOffer(int res)
    {
        partnerOffers[res].text = Trading.singleton.partnerOffer[res].ToString();
    }

}
