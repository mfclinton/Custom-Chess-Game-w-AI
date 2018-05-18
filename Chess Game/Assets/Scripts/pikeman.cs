using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pikeman : base_soldier
{

    public pikeman(int x, int y, bool isPlayer, GameObject unitModel)
    {
        //STATS
        //health = 2;
        //strength = 3;
        //defense = 2;
        health = 1;
        strength = 1;
        defense = 0;
        experience = 0;
        name = "Pikeman";

        //MOVEMENT ARRAY Only works for 8 x 8 board
        bool[,] pikemanMovement = {
            {true,false,true },
            {false,false,false },
            {true,false,true}
        };
        movementarray = pikemanMovement;

        xLocation = x;
        yLocation = y;

        isPlayerOne = isPlayer;

        unitmodel = unitModel;
    }

    public override int[,] FindPossibleMovements()
    {
        //This +1 fix should be fixed later for an odd sized map
        int[,] possibleMovements = new int[9,9];
        for (int h = 0; h < battlefield.width/2; h++) 
            for (int i = 0 - h - 1; i < h + 2; i++)
                for (int j = 0 - h - 1; j < h + 2; j++)
                {
                    int xCord = (possibleMovements.GetLength(0)/2) + i;
                    int yCord = (possibleMovements.GetLength(1) / 2) + j;

                    //Checks if out of bounds
                    if (xLocation + i < 0 || yLocation + j < 0 || xLocation + i >= battlefield.width || yLocation + j >= battlefield.height || xCord < 0 || yCord < 0 || xCord >= possibleMovements.GetLength(0) || yCord >= possibleMovements.GetLength(1))
                    {
                        possibleMovements[xCord,yCord] = 0;
                        continue;
                    }

                    //Avoid inside of loop
                    if (!((j != h + 1 && i != 0 - h - 1) || (j != 0 - h - 1 && i != h + 1) || (j != h + 1 && i != h + 1) || (j != 0 - h - 1 && i != 0 - h - 1)))
                    {
                        continue;
                    }

                    //evaluate if can travel
                    //Top Left
                    else if (j == h + 1 && i == 0 - h - 1)
                    {
                        if((battlefield.map[xLocation + i, yLocation + j] == null && possibleMovements[xCord + 1, yCord - 1] == 1) || (h == 0 && battlefield.map[xLocation + i, yLocation + j] == null))
                        {
                            possibleMovements[xCord, yCord] = 1;
                        }
                        else if ((battlefield.map[xLocation + i, yLocation + j] != null && battlefield.map[xLocation + i, yLocation + j].isPlayerOne != isPlayerOne && possibleMovements[xCord + 1, yCord - 1] == 1) || (h==0 && battlefield.map[xLocation + i, yLocation + j].isPlayerOne != isPlayerOne))
                        {
                            possibleMovements[xCord, yCord] = 2;
                        }
                        else
                        {
                            possibleMovements[xCord, yCord] = 0;
                        }
                    }
                    //Top Right
                    else if (j == h + 1 && i == h + 1)
                    {
                        if ((battlefield.map[xLocation + i, yLocation + j] == null && possibleMovements[xCord - 1, yCord - 1] == 1) || (h == 0 && battlefield.map[xLocation + i, yLocation + j] == null))
                        {
                            possibleMovements[xCord, yCord] = 1;
                        }
                        else if((battlefield.map[xLocation + i, yLocation + j] != null && battlefield.map[xLocation + i, yLocation + j].isPlayerOne != isPlayerOne && possibleMovements[xCord - 1, yCord - 1] == 1) || (h == 0 && battlefield.map[xLocation + i, yLocation + j].isPlayerOne != isPlayerOne))
                        {
                            possibleMovements[xCord, yCord] = 2;
                        }
                        else
                        {
                            possibleMovements[xCord, yCord] = 0;
                        }

                    }
                    //Bottom Left
                    else if(j == 0 - h - 1 && i == 0 - h - 1 )
                    {
                        if ((battlefield.map[xLocation + i, yLocation + j] == null && possibleMovements[xCord + 1, yCord + 1] == 1) || (h == 0 && battlefield.map[xLocation + i, yLocation + j] == null))
                        {
                            possibleMovements[xCord, yCord] = 1;
                        }
                        else if ((battlefield.map[xLocation + i, yLocation + j] != null && battlefield.map[xLocation + i, yLocation + j].isPlayerOne != isPlayerOne && possibleMovements[xCord + 1, yCord + 1] == 1) || (h == 0 && battlefield.map[xLocation + i, yLocation + j].isPlayerOne != isPlayerOne))
                        {
                            possibleMovements[xCord, yCord] = 2;
                        }
                        else
                        {
                            possibleMovements[xCord, yCord] = 0;
                        }
                    }
                    //Bottom Right
                    else if(j == 0 - h - 1 && i == h + 1)
                    {
                        if ((battlefield.map[xLocation + i, yLocation + j] == null && possibleMovements[xCord - 1, yCord + 1] == 1) || (h == 0 && battlefield.map[xLocation + i, yLocation + j] == null))
                        {
                            possibleMovements[xCord, yCord] = 1;
                        }
                        else if ((battlefield.map[xLocation + i, yLocation + j] != null && battlefield.map[xLocation + i, yLocation + j].isPlayerOne != isPlayerOne && possibleMovements[xCord - 1, yCord + 1] == 1) || (h == 0 && battlefield.map[xLocation + i, yLocation + j].isPlayerOne != isPlayerOne))
                        {
                            possibleMovements[xCord, yCord] = 2;
                        }
                        else
                        {
                            possibleMovements[xCord, yCord] = 0;
                        }
                    }
                }
        return possibleMovements;
    }

    public override void TakeAction(int xMov, int yMov)
    {
        int[,] possibleMovements = FindPossibleMovements();
        int middleIndexX = possibleMovements.GetLength(0) / 2;
        int middleIndexY = possibleMovements.GetLength(1) / 2;
        if (name == "Pikeman")
        {
            //There's some fucking problem somewhere in here withh the values of the tiles.

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
                    if (battlefield.map[xLocation + xMov, yLocation + yMov] == null)
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
    }
}

