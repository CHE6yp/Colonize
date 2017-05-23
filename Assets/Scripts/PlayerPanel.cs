using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// UI panel that displays players and their wealth;
/// </summary>
public class PlayerPanel : MonoBehaviour {

    public Player player;
    public int id;

    public Image panelImage;
    public Text playerName;

    public Text wealthText;

    

    public Color[] playerColors = new Color[4];
    public Color[] turnColors = new Color[2];



    public void SetPanel(Player p)
    {
        player = p;
        
        player.wealthChange = UpdateWealth;
        //player.endTurn += EndTurn;
        MatchManager.singleton.newTurn +=NewTurn;

        id = player.colorInt;
        playerName.text = player.playerName;
        playerName.color = playerColors[player.colorInt];

        UpdateWealth();
    }

    /// WEALTH:
    /// wood, sheep, wheat, stone, brick
    /// 
    /// roads towns?
    /// 
    /// cards
    /// 
    /// knights
    /// other cards?
    /// 
    /// biggest road?
    /// 
    /// win points
    /// 
    public void UpdateWealth()
    {
        wealthText.text = string.Format("Wood = {0}\nSheep = {1}\nWheat = {2}\nBrick = {3}\nStone = {4}",
            player.wealth[0], player.wealth[1], player.wealth[2], player.wealth[3], player.wealth[4]);

        //playerWealth.text = string.Format("Wood = {0}\nSheep = {1}\nWheat = {2}\nBrick = {3}\nStone = {4}\n\nRoads = {5}\nTowns = {6}\n\nWin Points = {7}\n",
            //activePlayer.wealth[0], activePlayer.wealth[1], activePlayer.wealth[2], activePlayer.wealth[3], activePlayer.wealth[4], activePlayer.roads, activePlayer.towns, activePlayer.winPoints);
    }


    public void NewTurn()
    {
        if (player.turn)
            panelImage.color = turnColors[0];
        else
            panelImage.color = turnColors[1];
    }




}
