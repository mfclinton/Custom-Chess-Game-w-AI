using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : MonoBehaviour {
    base_soldier[] AIUnits;
    base_soldier[] PlayerUnits;

    int[,] tileValues;
    int[] soldierPriority;

    public AI(base_soldier[] hisUnits, base_soldier[] playerUnits)
    {
        AIUnits = hisUnits;
        PlayerUnits = playerUnits;
    }


    void getTileValues()
    {
        tileValues = new int[battlefield.map.GetLength(0), battlefield.map.GetLength(1)];
        soldierPriority = new int[AIUnits.Length];

        //This For Loop Calculates Unit Priority Based On Which Units They Can Attack
        for (int i = 0; i < AIUnits.Length; i++)
        {
            base_soldier Soldier = AIUnits[i];
            if (Soldier.isAlive == false) continue;
            int[,] possibleMovements = Soldier.FindPossibleMovements();

            for (int m = 0; m < possibleMovements.GetLength(0); m++)
                for (int c = 0; c < possibleMovements.GetLength(1); c++)
                {
                    int currentXLocation = Soldier.xLocation - (possibleMovements.GetLength(0) / 2) + m;
                    int currentYLocation = Soldier.yLocation - (possibleMovements.GetLength(1) / 2) + c;

                    if(currentXLocation < 0 || currentXLocation >= battlefield.width || currentYLocation < 0 || currentYLocation >= battlefield.height)
                    {
                        continue;
                    }

                    //This Spot Is Attackable
                    if (possibleMovements[m, c] == 2)
                    {
                        tileValues[currentXLocation, currentYLocation] += 2; //Higher Val = Wants To Attack
                        soldierPriority[i]++; // Priority on units thaat want to attack
                    }

                    //This Spot Is Moveable
                    else if (possibleMovements[m, c] == 1)
                    {
                        float closestDistance = 1000000;
                        int closestX, closestY;
                        //Find Closest Enemy
                        for (int z = 0; z < PlayerUnits.Length; z++)
                        {
                            if(!PlayerUnits[i].isAlive)
                            {
                                continue;
                            }
                            float distance = Mathf.Sqrt(Mathf.Pow((Soldier.xLocation - PlayerUnits[i].xLocation), 2) + Mathf.Pow((Soldier.yLocation - PlayerUnits[i].yLocation), 2));
                            //finds closest unit
                            if(distance <= closestDistance)
                            {
                                closestDistance = distance;
                                closestX = PlayerUnits[i].xLocation;
                                closestY = PlayerUnits[i].yLocation;
                            }
                        }
                        //determines if move would place unit farther from enemy
                        float distanceofmove = Mathf.Sqrt(Mathf.Pow((currentXLocation - PlayerUnits[i].xLocation), 2) + Mathf.Pow((currentYLocation - PlayerUnits[i].yLocation), 2));
                        if(distanceofmove > closestDistance)
                        {
                            tileValues[currentXLocation, currentYLocation]--;
                        }

                    }

                    //Not either
                    else
                    {

                    }
                }
        }

        for (int l = 0; l < PlayerUnits.Length; l++)
        {
            base_soldier enemySoldier = PlayerUnits[l];
            if (enemySoldier.isAlive == false) continue;
            int[,] possibleEnemyMovements = enemySoldier.FindPossibleMovements();

            //This For Loop Calculates Tile Value Based on Where Units Can Attack
            // This For Loop Also Calculates Unit Priority Based On Imminent Danger
            for (int i = 0; i < possibleEnemyMovements.GetLength(0); i++)
                for (int k = 0; k < possibleEnemyMovements.GetLength(1); k++)
                {
                    int currentXLocation = enemySoldier.xLocation - (possibleEnemyMovements.GetLength(0) / 2) + i;
                    int currentYLocation = enemySoldier.yLocation - (possibleEnemyMovements.GetLength(1) / 2) + k;

                    //This Tile Can Be Attacked in Future
                    if (possibleEnemyMovements[i, k] == 1)
                    {
                        tileValues[currentXLocation, currentYLocation] -= 1;
                    }

                    //This Tile is in danger of being attacked already
                    else if (possibleEnemyMovements[i, k] == 2)
                    {
                        tileValues[enemySoldier.xLocation, enemySoldier.yLocation] += 3; // This will make the bot OP as fuck
                        base_soldier endageredSoldier = battlefield.map[currentXLocation, currentYLocation];
                        tileValues[endageredSoldier.xLocation, endageredSoldier.yLocation]--;
                        if (endageredSoldier.isAlive == false) continue;

                        int m = 0;
                        for (m = 0; m < AIUnits.Length; m++)
                        {
                            if (endageredSoldier == AIUnits[m])
                            {
                                soldierPriority[m] += 2;
                                break;
                            }
                        }
                    }

                    //Not Traversable
                    else
                    {

                    }
                }
        }
    }

        public base_soldier DecideOnSoldier()
        {
            base_soldier returnedSoldier;
            while (true)
            {
                int RandomNumber = Random.Range(0, soldierPriority.Length);
                float luck = Random.Range(0, 10);
                returnedSoldier = AIUnits[RandomNumber];
                if (returnedSoldier.isAlive == false) continue;
                if (soldierPriority[RandomNumber] == 0 && luck <= 4)
                {
                    break;
                }
                else if (soldierPriority[RandomNumber] == 1 && luck <= 6)
                {
                    break;
                }
                else if (soldierPriority[RandomNumber] > 1 && luck <= 9)
                {
                    break;
                }
                else if (soldierPriority[RandomNumber] == -1 && luck < 2)
                {
                    break;
                }
                else if (soldierPriority[RandomNumber] < -1 && luck < 1)
                {
                    break;
                }
            }
            return returnedSoldier;
        }

    public int[] DetermineAction(base_soldier soldier)
    {
        int[,] possibleMovements = soldier.FindPossibleMovements();
        bool movementFound = false;
        int[] movement = new int[2];
        while (!movementFound)
        {
            for (int i = 0; i < possibleMovements.GetLength(0); i++)
            {
                for (int k = 0; k < possibleMovements.GetLength(1); k++)
                {
                    if (!movementFound && possibleMovements[i,k] != 0)
                    {
                        int currentXLocation = soldier.xLocation - (possibleMovements.GetLength(0) / 2) + i;
                        int currentYLocation = soldier.yLocation - (possibleMovements.GetLength(1) / 2) + k;

                        if (currentXLocation < 0 || currentXLocation >= battlefield.width || currentYLocation < 0 || currentYLocation >= battlefield.height) continue;

                        float luck = Random.Range(0, 10);
                        movement[0] = currentXLocation - soldier.xLocation;
                        movement[1] = currentYLocation - soldier.yLocation;

                        if (tileValues[currentXLocation, currentYLocation] == 0 && luck <= 4)
                        {

                            movementFound = true;
                        }

                        else if (tileValues[currentXLocation, currentYLocation] == 1 && luck <= 6)
                        {

                            movementFound = true;
                        }

                        else if (tileValues[currentXLocation, currentYLocation] > 1 && luck <= 9)
                        {

                            movementFound = true;
                        }

                        else if (tileValues[currentXLocation, currentYLocation] == -1 && luck < 2)
                        {

                            movementFound = true;
                        }

                        else if (tileValues[currentXLocation, currentYLocation] < -1 && luck < 1)
                        {

                            movementFound = true;
                        }
                    }
                }
            }
        }
        //Debug.Log("XPOS: " + soldier.xLocation + "    YPOS: " + soldier.yLocation);
        //Debug.Log("XMOV: " + movement[0] + "    YMOV: " + movement[1]);
        return movement;
    }

    public void takeActionEndTurn()
    {
        if (!AllAIDead())
        {
            getTileValues();
            base_soldier selected = DecideOnSoldier();
            battlefield theBattle = FindObjectOfType<battlefield>();
            theBattle.setSelected(selected);
            int[] action = DetermineAction(selected);
            selected.TakeAction(action[0], action[1]);
            battlefield.isPlayerOnesTurn = true;
        }
    }

    public bool AllAIDead()
    {
        for (int i = 0; i < AIUnits.Length; i++)
        {
            if (AIUnits[i].isAlive) return false;
        }
        return true;
    }
}
