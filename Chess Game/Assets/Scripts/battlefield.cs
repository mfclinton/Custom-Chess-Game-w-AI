using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class battlefield : MonoBehaviour {

    public static base_soldier[,] map;
    public static int width = 8, height = 8;

    int armysize = 3;

    base_soldier selectedUnit;
    Transform selectedTransform;
    public static bool isPlayerOnesTurn = true;

    AI enemyAI;

    //UNIT MODELS
    public GameObject infantryModel, enemyInfantryModel, cavalryModel, enemyCavalryModel, pikemanModel, enemyPikemanModel, moveableTile, attackableTile, imageReference, generalModel, enemyGeneralModel;
    public Text turnText, imageName,imageHealth,imageStrength,imageDefense;

    bool gamesOver = false;

    void isGameOver()
    {
        for (int i = 0; i < enemies.Length; i++)
        {
            if(enemies[i].isAlive)
            {
                return;
            }
        }
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].isAlive)
            {
                return;
            }
        }
        gamesOver = true;
    }
    public void setSelected(base_soldier newSelected)
    {
        selectedUnit = newSelected;
    }
    public void DisplayMap()
    {
        Debug.Log("DONE");
        while (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            child.parent = null;
            Destroy(child.gameObject);
        }

        for (int i = 0; i < width; i++)
            for (int j = 0; j < height; j++)
            {
                if(map[i,j] != null)
                {
                    Vector3 location = new Vector3(i, 0, j);
                    GameObject spawnedunit = Instantiate(map[i, j].unitmodel, location, Quaternion.identity);
                    spawnedunit.transform.parent = transform;
                }
            }
    }

    void DisplayPossibleMovements(base_soldier selectedunit)
    {
        DisplayMap();

        int[,] possibleMovements = selectedunit.FindPossibleMovements();

        for (int i = 0; i < possibleMovements.GetLength(0); i++)
            for (int j = 0; j < possibleMovements.GetLength(1); j++)
            {
                Vector3 location = new Vector3(selectedunit.xLocation - (possibleMovements.GetLength(0)/2) + i, 1, selectedunit.yLocation - (possibleMovements.GetLength(1) / 2) + j);
                if(possibleMovements[i,j] == 1)
                {
                    GameObject spawnedunit = Instantiate(moveableTile, location, Quaternion.identity);
                    spawnedunit.transform.parent = transform;
                }
                if (possibleMovements[i, j] == 2)
                {
                    GameObject spawnedunit = Instantiate(attackableTile, location, Quaternion.identity);
                    spawnedunit.transform.parent = transform;
                }
            }
    }

    base_soldier[] enemies = new base_soldier[width];
    base_soldier[] players = new base_soldier[width];

    void GenerateMap()
    {
        enemies = new base_soldier[width];
        players = new base_soldier[width];
        Debug.Log("MAP GENERATED");
        //PLAYER
        for (int i = 0; i < width; i++)
        {
            if(i == 7)
            {
                map[i, 0] = new general(i, 0, true, generalModel);
                players[i] = map[i, 0];
                continue;
            }
            if (i % 3 == 0)
            {
                map[i, 0] = new pikeman(i, 0, true, pikemanModel);
                players[i] = map[i, 0];
                continue;
            }
            if (i % 2 == 0)
            {
                map[i, 0] = new infantry(i, 0, true, infantryModel);
                players[i] = map[i, 0];
                continue;
            }

            map[i, 0] = new cavalry(i, 0, true, cavalryModel);
            players[i] = map[i, 0];
        }
        //ENEMY
        for (int i = 0; i < width; i++)
        {
            if (i == 7)
            {
                map[i, height - 1] = new general(i, height - 1, false, enemyGeneralModel);
                enemies[i] = map[i, height - 1];
                continue;
            }
            if (i % 3 == 0)
            {
                map[i, height - 1] = new pikeman(i, height - 1, false, enemyPikemanModel);
                enemies[i] = map[i, height - 1];
                continue;
            }
            if (i % 2 == 0)
            {
                map[i, height - 1] = new infantry(i, height - 1, false, enemyInfantryModel);
                enemies[i] = map[i, height-1];
                continue;
            }
            
            map[i, height - 1] = new cavalry(i, height - 1, false, enemyCavalryModel);
            enemies[i] = map[i, height - 1];
        }
        enemyAI = new AI(enemies, players);
    }

    private void Start()
    {
        Debug.Log("HI");
        map = new base_soldier[width, height];
        GenerateMap();
        DisplayMap();
        HandleUI();
    }

    bool mapDisplayed = false;
    bool gameOver = false;
    private void Update()
    {
        if (!gameOver && !gamesOver)
        {
            if (isPlayerOnesTurn)
            {
                if (!mapDisplayed)
                {
                    DisplayMap();
                    mapDisplayed = true;
                }
                OnClick();
            }
            else
            {
                enemyAI.takeActionEndTurn();
                if(selectedUnit != null)
                {
                    HandleUpgrade();
                }
                selectedUnit = null;
                
                mapDisplayed = false;
            }
            gameOver = enemyAI.AllAIDead();
            isGameOver();
        }
        else
        {
            Debug.Log("GAME OVER");
            SceneHandler scenehandle = FindObjectOfType<SceneHandler>();
            scenehandle.NextScene();
        }
    }

    void OnClick()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SelectObject();
            HandleSelection();
            HandleUI();
            if (selectedUnit != null)
                DisplayPossibleMovements(selectedUnit);
            else
                DisplayMap();
        }
    }

    //PLAYER CONTROLS
    void SelectObject()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit, 1000f))
        {
            selectedTransform = hit.transform;
            return;
        }
        selectedTransform = null;
    }

    void HandleSelection()
    {
        if (selectedTransform.tag == "UNIT")
        {
            int xCord = (int)selectedTransform.position.x, yCord = (int)selectedTransform.position.z;
            if (xCord >= width || xCord < 0 || yCord >= height || yCord < 0) return;

            selectedUnit = map[xCord, yCord];
            if (isPlayerOnesTurn != selectedUnit.isPlayerOne)
            {
                selectedUnit = null;
                selectedTransform = null;
                return;
            }
        }
        else if (selectedTransform.tag == "MOVEABLE" && selectedUnit != null)
        {
            int xMov = (int)selectedTransform.position.x - selectedUnit.xLocation;
            int yMov = (int)selectedTransform.position.z - selectedUnit.yLocation;
            selectedUnit.TakeAction(xMov, yMov);
            if(selectedUnit != null) HandleUpgrade();                 //Might cause problems
            selectedUnit = null;
            selectedTransform = null;
            isPlayerOnesTurn = false;
        }
    }

    //Handle Upgrading Units Based On XP and Location
    void HandleUpgrade()
    {
        /*
        if(selectedUnit.experience >= 2)
        {
            selectedUnit.strength++;
            selectedUnit.defense++;
            selectedUnit.health++;
        }
        */
        if(selectedUnit.atOtherEndOfBoard)
        {
            GameObject newModel = enemyCavalryModel;
            if (selectedUnit.name.Equals("Infantry") && selectedUnit.isPlayerOne == true) newModel = cavalryModel;
            else if (selectedUnit.name.Equals("Infantry") && selectedUnit.isPlayerOne == false) newModel = enemyCavalryModel;
            if(selectedUnit.unitmodel.Equals(infantryModel) || selectedUnit.unitmodel.Equals(enemyInfantryModel))
            {
                //int tempstrength = selectedUnit.strength, tempdefense = selectedUnit.defense, temphealth = selectedUnit.health;
                base_soldier replacement = new cavalry(selectedUnit.xLocation, selectedUnit.yLocation, selectedUnit.isPlayerOne, newModel);
                //replacement.strength = tempstrength;
                //replacement.defense = tempdefense;
                //replacement.health = temphealth;
                int PlayerIndex = -1;
                int AIIndex = -1;
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].Equals(selectedUnit))
                    {
                        PlayerIndex = i;
                        break;
                    }
                }
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (enemies[i].Equals(selectedUnit))
                    {
                        AIIndex = i;
                        break;
                    }
                }
                if(PlayerIndex != -1)
                {
                    players[PlayerIndex] = replacement;
                }
                else if(AIIndex != -1)
                {
                    enemies[AIIndex] = replacement;
                }
                map[selectedUnit.xLocation, selectedUnit.yLocation] = replacement;
                enemyAI = new AI(enemies, players);
            }
        }
    }

    void HandleUI()
    {
        if(isPlayerOnesTurn)
            turnText.text = "It's Player One's Turn";
        else
            turnText.text = "It's Player Two's Turn";
        HandleImageReference();
    }

    void HandleImageReference()
    {
        setNames("", 0, 0, 0);
        while(imageReference.transform.childCount > 0)
        {
            GameObject child = imageReference.transform.GetChild(0).gameObject;
            child.transform.parent = null;
            Destroy(child);
        }
        if (selectedUnit != null)
        {
            GameObject instantiatedImageReference = Instantiate(selectedUnit.unitmodel, Vector3.zero,Quaternion.identity);
            instantiatedImageReference.transform.parent = imageReference.transform;
            instantiatedImageReference.transform.localPosition = Vector3.zero;
            instantiatedImageReference.AddComponent<Rotate>();
            setNames(selectedUnit.name, selectedUnit.health, selectedUnit.strength, selectedUnit.defense);
        }
    }

    void setNames(string unitTitle, int unitsHealth, int unitsStrength, int unitsDefense)
    {
        if(unitsHealth == 0)
        {
            imageHealth.text = "";
            imageStrength.text = "";
            imageDefense.text = "";
        }
        imageHealth.text = "Health: " + unitsHealth;
        imageStrength.text = "Strength: " + unitsStrength;
        imageDefense.text = "Defense: " + unitsDefense;
        imageName.text = unitTitle;

    }
}
