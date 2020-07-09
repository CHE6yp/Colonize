using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankUI : MonoBehaviour {


    public static BankUI singleton;

    public Dropdown sellRes;
    public Text sellAmount;

    // Use this for initialization
    void Awake () {
        singleton = this;
	}

    public void SetBuy(int i)
    {
        Bank.singleton.buy = i;

        bool b = (i < 5) ? false : true;
        HideSellStuff(b);
    }

    public void SetSell(int i)
    {
        Bank.singleton.sell = i;

        SellCountUpdate(Bank.CountAmount(i));
    }

    public void BuyResourceFromBank()
    {
        MatchUI.localPlayer.BankBuy();
    }

    public void HideSellStuff(bool b)
    {
        sellAmount.gameObject.SetActive(!b);
        sellRes.gameObject.SetActive(!b);
    }

    public void SellCountUpdate(int i)
    {
        sellAmount.text = "за " + i.ToString();
    }
}
