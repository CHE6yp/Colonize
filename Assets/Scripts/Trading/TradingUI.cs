using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TradingUI : MonoBehaviour {


    public Dropdown tradePlayersDropdown;

    public Canvas pending;
    public Canvas request;
    public Canvas denied;

    public int currentTradeStatus = 0;
    public Canvas[] tradeStatusPanels = new Canvas[0];


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
    }

    public void TradeStatusHide()
    {
        tradeStatusPanels[currentTradeStatus].gameObject.SetActive(false);
    }


}
