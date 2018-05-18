using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cavalry : base_soldier {

    public cavalry(int x, int y, bool isPlayer, GameObject unitModel)
    {
        //STATS
        //health = 4;
        //strength = 3;
        health = 1;
        strength = 1;
        defense = 0;
        experience = 0;
        name = "Cavalry";

        //MOVEMENT ARRAY
        bool[,] cavalryMovement = {
            { false,true,false,true,false},
            {true,false,false,false,true},
            {false,false,false,false,false},
            {true,false,false,false,true},
            {false,true,false,true,false}
        };
        movementarray = cavalryMovement;

        xLocation = x;
        yLocation = y;

        isPlayerOne = isPlayer;

        unitmodel = unitModel;
    }
}
