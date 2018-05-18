using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class base_soldier {

    public GameObject unitmodel;
    //STATS
    public int health;
    public int strength;
    public int defense;
    public int experience;
    public string name;
    public bool isGeneral = false;

    public bool isAlive = true;

    public int xLocation, yLocation;

    public bool atOtherEndOfBoard;

    public bool isPlayerOne;

    public bool[,] movementarray;

    public void TakeDamage(int power)
    {
        int damage = power - defense;
        if (damage <= 0) return;
        health -= damage;
        Debug.Log("Health " + health);
        if (health <= 0)
        {
            isAlive = false;
            Debug.Log("UNIT KILLED" + !isAlive);
            Debug.Log(xLocation + "  " + yLocation);
            battlefield.map[xLocation, yLocation] = null;
        }
    }

    // 0: Not Moveable, 1: Moveable, 2: Enemy
    public virtual int[,] FindPossibleMovements()
    {
        int[,] possibleMovements = new int[movementarray.GetLength(0), movementarray.GetLength(1)];
        for (int i = 0; i < movementarray.GetLength(0); i++)
            for (int j = 0; j < movementarray.GetLength(1); j++)
            {
                int selectedxCord = xLocation - ((int)movementarray.GetLength(0)/2) + i, selectedyCord = yLocation - (movementarray.GetLength(1) / 2) + j;
                if (selectedxCord < 0 || selectedyCord < 0 || selectedxCord >= battlefield.map.GetLength(0) || selectedyCord >= battlefield.map.GetLength(1)) continue;
                base_soldier soldierontile = battlefield.map[selectedxCord, selectedyCord];
                if (movementarray[i, j] && soldierontile == null)
                    possibleMovements[i, j] = 1;
                else if (movementarray[i, j] && soldierontile.isPlayerOne != isPlayerOne)
                    possibleMovements[i, j] = 2;
                else
                    possibleMovements[i, j] = 0;
            }
        return possibleMovements;
    }

    public virtual void TakeAction(int xMov,int yMov)
    {
        int[,] possibleMovements = FindPossibleMovements();
        int middleIndexX = movementarray.GetLength(0) / 2;
        int middleIndexY = movementarray.GetLength(1) / 2;
        if(name == "Pikeman")
        {
            //There's some fucking problem somewhere in here withh the values of the tiles.
            Debug.Log("XMov: " + xMov + "From " + xLocation);
            Debug.Log("YMov: " + yMov + "From " + yLocation);
        }

        if (possibleMovements[middleIndexX + xMov, middleIndexY + yMov] != 0)
        {
            if (possibleMovements[middleIndexX + xMov, middleIndexY + yMov] == 1)
            {
                battlefield.map[xLocation + xMov, yLocation + yMov] = this;
                battlefield.map[xLocation, yLocation] = null;
                xLocation += xMov;
                yLocation += yMov;
                isAtEndOfBoard();
            }
            else if (possibleMovements[middleIndexX + xMov, middleIndexY + yMov] == 2)
            {
                //ATTACK
                Debug.Log("ATTACKED");
                Attack(battlefield.map[xLocation + xMov, yLocation + yMov]);
                if(battlefield.map[xLocation + xMov, yLocation + yMov] == null)
                {
                    battlefield.map[xLocation + xMov, yLocation + yMov] = this;
                    battlefield.map[xLocation, yLocation] = null;
                    xLocation += xMov;
                    yLocation += yMov;
                    isAtEndOfBoard();
                }
            }
            else Debug.LogError("PossibleMovement Value TOO HIGH");
        }
        else
            Debug.LogError("Not Possible Movement");
    }

    public void Attack(base_soldier enemy)
    {
        enemy.TakeDamage(strength);
        //experience++;
    }

    public void isAtEndOfBoard()
    {
        int lastIndex;
        if (isPlayerOne) lastIndex = battlefield.height - 1;
        else lastIndex = 0;

        if (yLocation == lastIndex) atOtherEndOfBoard = true;
        else atOtherEndOfBoard = false;
        Debug.Log(atOtherEndOfBoard);
    }
}
