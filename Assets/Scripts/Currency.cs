using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Currency  {

    public int _wood;
    public int _sheep;
    public int _wheat;
    public int _stone;
    public int _brick;




    public Currency(int wood, int sheep, int wheat, int stone, int brick)
    {
        _wood = wood;
        _sheep = sheep;
        _wheat = wheat;
        _stone = stone;
        _brick = brick;

    }

    

}
