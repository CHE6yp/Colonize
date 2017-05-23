using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchUI : MonoBehaviour {

    public static MatchUI singleton;

    public Player localPlayer;

    public Transform otherPlayersCanvas;
    public GameObject otherPlayerPanelPref;
    public List<Canvas> otherPlayerPanels = new List<Canvas>();
    public PlayerPanel localPlayerPanel;
    

    

    public Canvas[] canvases = new Canvas[0];
    int currentCanvasI = 0;

    //bank and log
    public Dropdown sellRes;
    public Text sellAmount;




    

    //buttonEvents
    public Player.PlayerEvent endTurnUI;
    public Player.PlayerEvent rollDiceUI;
    public Player.PlayerEvent startGameUI;
    public Player.PlayerEvent buyButtonUI;

    // Use this for initialization
    void Start () {
        singleton = this;

    }

    public void SwitchCanvas(int i)
    {
        canvases[currentCanvasI].gameObject.SetActive(false);
        canvases[i].gameObject.SetActive(true);
        currentCanvasI = i;
    }
	





    //buttons
    public void EndTurn()
    {
        //endTurnUI();
        localPlayer.EndTurn();
        Trading.singleton.UpdateTradeOptions();
    }

    public void RollDice()
    {
        rollDiceUI();
    }

    public void StartGame()
    {
        if (startGameUI != null)
            startGameUI();
        SwitchCanvas(1);
        //UpdateTradeOptions();

        

    }


    public void CreatePlayerPanels()
    {
        foreach (Player p in MatchManager.singleton.players)
        {
            if (p != localPlayer)
            {
                GameObject op = Instantiate(otherPlayerPanelPref);
                op.transform.parent = otherPlayersCanvas;

                op.GetComponent<PlayerPanel>().SetPanel(p);
            }
            else
            {
                localPlayerPanel.player = p;
                localPlayerPanel.SetPanel(p);
            }
        }
    }



    //BANK AND LOG STUFF
    public void BuyResourceFromBank()
    {
        buyButtonUI();
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
