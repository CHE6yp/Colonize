using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevCards : MonoBehaviour {

    //14 рыцарей, 5 победных очков, 2 карты строительства двух дорог, 2 карты взятия любого ресурса, 2 карты монополии на ресурс

    public enum DevCardType { Knight, WinPoint, TwoRoads, AnyResource, Monopoly}
    public List<DevCardType> devCards = new List<DevCardType>();


     // Use this for initialization
     void Awake () {
        FillStack();
	}

    void FillStack()
    {
        devCards.Clear();

        for (int i = 0; i < 14; i++)
        {
            DevCardType card = DevCardType.Knight;
            devCards.Add(card);
        }

        for (int i = 0; i < 5; i++)
        {
            DevCardType card = DevCardType.WinPoint;
            devCards.Add(card);
        }

        for (int i = 0; i < 2; i++)
        {
            DevCardType card = DevCardType.TwoRoads;
            devCards.Add(card);
        }

        for (int i = 0; i < 2; i++)
        {
            DevCardType card = DevCardType.AnyResource;
            devCards.Add(card);
        }

        for (int i = 0; i < 2; i++)
        {
            DevCardType card = DevCardType.Monopoly;
            devCards.Add(card);
        }
    }
	

    public static void PlayKnight(Player player)
    {
        player.playerPhase = Player.PlayerPhase.MoveRobber; //надо наверно новую фазу, чтобы не грабить людей если сыграл рыцарем
    }
    
}
