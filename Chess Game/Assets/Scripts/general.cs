using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class general : base_soldier {
    public general(int x, int y, bool isPlayer, GameObject unitModel)
    {
        //STATS
        //health = 2;
        //strength = 2;
        health = 1;
        strength = 1;
        defense = 0;
        experience = 0;
        name = "General";
        isGeneral = true;

        //MOVEMENT ARRAY
        bool[,] generalMovement = {
            { true,true,true},
            { true,false,true},
            { true,true,true}
        };
        movementarray = generalMovement;

        xLocation = x;
        yLocation = y;

        isPlayerOne = isPlayer;

        unitmodel = unitModel;
    }
}
