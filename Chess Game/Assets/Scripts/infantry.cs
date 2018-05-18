using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infantry : base_soldier {

    public infantry(int x, int y, bool isPlayer, GameObject unitModel)
    {
        //STATS
        //health = 2;
        //strength = 2;
        health = 1;
        strength = 1;
        defense = 0;
        experience = 0;
        name = "Infantry";

        //MOVEMENT ARRAY
        bool[,] infantryMovement = {
            { false,false,false},
            { true,false,true},
            { false,false,false}
        };
        movementarray = infantryMovement;

        xLocation = x;
        yLocation = y;

        isPlayerOne = isPlayer;

        unitmodel = unitModel;
    }

    

}
