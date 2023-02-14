using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class Hatches_Script : MonoBehaviour
{
    #region Variables
    //--PANELS--
    [Header("Panels")]

    //The Hatches Game panel
    [SerializeField]
    GameObject hatchesGamePanel;

    //The Map Overview Menu Panel
    [SerializeField]
    GameObject mapOverviewPanel;
    
    //The panel that holds all the map data buttons
    [SerializeField]
    GameObject mapsDataPanel;

    //The end panel that appears at the end of a game
    [SerializeField]
    GameObject endPanel;

    //The gameBlockingPanel blocks interaction with the game
    [SerializeField]
    GameObject gameBlockingPanel;

    //--TEXT GAMEOBJECTS--
    [Header("Text Gameobjects")]

    //The text used to display the current map name
    [SerializeField]
    TextMeshProUGUI nameOfMapText;

    //The text used to display the current floor name
    [SerializeField]
    TextMeshProUGUI nameOfFloorText;

    //The text used to display the number of remaining hatches
    [SerializeField]
    TextMeshProUGUI remainingHatchesText;

    //The text used to display the player's lives
    [SerializeField]
    TextMeshProUGUI playerLivesText;

    //The text used to display the gameTimer
    [SerializeField]
    TextMeshProUGUI gameTimerText;

    //The text title above the currentHatchNameText
    [SerializeField]
    GameObject currentHatchNameTitle;

    //The text used to display the current hatch name
    [SerializeField]
    TextMeshProUGUI currentHatchNameText;

    //The endPanelText that is displayed at the end of a game
    [SerializeField]
    TextMeshProUGUI endPanelText;

    //The text used to display the end time
    [SerializeField]
    TextMeshProUGUI endTimeText;

    //The gameBlockingPanelText is displayed when the gameBlockingPanel is enabled
    [SerializeField]
    TextMeshProUGUI gameBlockingPanelText;

    //--SCRIPTS--
    [Header("Scripts")]

    //The current floorScript attached to the floor gameobject
    Floors_Script floorsScriptObject;

    //--LISTS--

    //The list to hold all the map buttons that have been clicked sucessfully so far
    List<GameObject> hatchesButtonsList = new List<GameObject>();

    //This lists holds the name of each button on the current floor
    List<string> hatchesNamesList;

    //--BOOLS--

    //The bool to determine if it's the start of the game
    bool startOfGame;

    //The bool to determine if the gameTimer should start counting
    bool startGameTimer;

    //The bool to determine if it is the final floor
    bool finalFloor;

    //--INTS & FLOATS

    //The current index in the hatchesNamesList
    int currentIndex;

    //The last used index in the hatchesNamesList
    int lastIndex;

    //The text used to display the player's lives
    int playerLives;

    //The amount of hatches remaining on the current floor
    int numOfRemainingHatches;

    //The game timer tracks how long the player is taking to complete the game
    float gameTimer;

    //--STRINGS--

    //The name of the current map
    string nameOfMap;

    //The name of the current floor
    string nameOfFloor;

    //--BUTTONS--
    [Header("Buttons")]

    //The continue button used to advance to the next floor
    [SerializeField]
    GameObject continueButton;
    #endregion

    #region Unity Methods
    // Start is called before the first frame update
    private void Start()
    {
        //Set startOfGame to true at Start
        startOfGame = true;
    }

    // Update is called once per frame
    private void Update()
    {
        //If the game has begun then start the gameTimer
        if (startGameTimer == true)
        {
            //Increase the gameTimer's time
            gameTimer += Time.deltaTime;

            //Set the gameTimerText equal to the gameTimer
            gameTimerText.text = gameTimer.ToString("0.0") + " Seconds";
        }
    }
    #endregion

    #region Set Up Methods
    //Called from the MapOverview menu when the HatchesButton is clicked; this sets up the start of the game
    private void InitialSetUp()
    {
        //Play sound when clicked
        SoundManager_Script.instance.PlaySound("Menu Button");

        //Enable the hatchesGamePanel
        hatchesGamePanel.SetActive(true);

        //Check if the current map name is not equal to the last played map
        if (nameOfMap != MainMenu_Script.instance.currentSelectedMap)
        {
            //Assign the name of the current map to the currentSelectedMap variable
            nameOfMap = MainMenu_Script.instance.currentSelectedMap;

            //Update the nameOfMapText
            nameOfMapText.text = nameOfMap;

            //Find the child in the mapsDataPanel that matches the nameOfMap; then assign its Floors_Script to the floorsScriptObject
            floorsScriptObject = mapsDataPanel.transform.Find(nameOfMap).GetComponent<Floors_Script>();
        }

        //Disable the mapOverviewPanel
        mapOverviewPanel.SetActive(false);

        //Set playerLives to 3
        playerLives = 3;

        //Update playerLivesText
        playerLivesText.text = playerLives.ToString();

        //Enable the currentHatchNameTitle
        currentHatchNameTitle.SetActive(true);

        //Call StartRound method which gets the data from the floorsScriptObject and starts the round
        StartRound();
    }

    //Gets the data from the floorsScriptObject and starts the round
    private void StartRound()
    {
        //Get the nameOfFloor, finalFloor bool and the hatchesNamesList from the floorsScriptObject
        floorsScriptObject.FloorSelected(out nameOfFloor, out finalFloor, out hatchesNamesList);

        //Set the nameOfFloorText equal to the nameOfFloor starting at the end of the nameOfMap's length index
        nameOfFloorText.text = nameOfFloor.Substring(nameOfMap.Length);

        //Set the numOfRemainingHatches equal to the size of the hatchesNamesList
        numOfRemainingHatches = hatchesNamesList.Count;

        //Remove duplicates from hatchesNameslist (Some names will be shown twice to fill out a room's area)
        hatchesNamesList = hatchesNamesList.Distinct().ToList();

        //Set the remainingHatchesText equal to the numOfRemainingHatches
        remainingHatchesText.text = numOfRemainingHatches.ToString();

        //Call the SetNewHatchName method to assign a random hatch name
        SetNewHatchName();

        //Check if it's the startOfGame
        if (startOfGame == true)
        {
            //Set the startOfGame bool to false as the game has started
            startOfGame = false;

            //Set the gameBlockingPanelText to "Ready?"
            gameBlockingPanelText.text = "Ready?";

            //Enable the gameBlockingPanel
            gameBlockingPanel.SetActive(true);

            //Invoke the FirstRoundStart method after 1 second
            Invoke("FirstRoundStart", 1f);
        }      
    }

    //Delays the start of the first round. Called from StartRound() via Invoke
    private void FirstRoundStart()
    {
        //Disable the gameBlockingPanel
        gameBlockingPanel.SetActive(false);

        //Set the startGameTimer to true to start the gameTimer
        startGameTimer = true;
    }

    //Set a new hatch to be found
    private void SetNewHatchName()
    {
        //Assign the value of the currentIndex to the lastIndex variable
        lastIndex = currentIndex;

        //Set the currentIndex to a range between 0 and the current size of the hatchesNamesList
        currentIndex = Random.Range(0, hatchesNamesList.Count);

        //A while loop to check if the current hatch is the same as the last
        while (currentIndex == lastIndex && hatchesNamesList.Count > 1)
        {
            //Set the current index to a range between 0 and the current size of the hatchesNamesList
            currentIndex = Random.Range(0, hatchesNamesList.Count);
        }

        //Update the currentHatchNameText with the new hatch name
        currentHatchNameText.text = hatchesNamesList[currentIndex];
    }
    #endregion

    #region Button Methods
    //When a map button is clicked, check if its name matches the current hatch name
    private void OnHatchButtonClick()
    {
        //Assign the button that was clicked to a variable
        var thisButton = EventSystem.current.currentSelectedGameObject;

        //If the button wasn't null and the button's name is equal to the current hatch name
        if (thisButton != null && thisButton.name == currentHatchNameText.text)
        {
            //Play sound when clicked
            SoundManager_Script.instance.PlaySound("Correct Button");

            //Disable the button component
            thisButton.GetComponent<Button>().enabled = false;

            //Enable the text in the child object
            thisButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;

            //Enable the image in the child object
            thisButton.gameObject.transform.Find("Hatch").GetComponent<Image>().enabled = true;

            //Add the clicked button to the hatchesButtonsList
            hatchesButtonsList.Add(thisButton);

            //Remove the clicked button's name from the hatchesNamesList
            hatchesNamesList.RemoveAt(currentIndex);

            //Decrease the numOfRemainingHatches by 1
            numOfRemainingHatches--;

            //Update the remainingHatchesText
            remainingHatchesText.text = numOfRemainingHatches.ToString();

            //If there are still hatches left then set a new hatch
            if (hatchesNamesList.Count > 0)
            {
                //Set a new hatch name
                SetNewHatchName();
            }
            //If there are no hatches left then check for player win or next floor
            else
            {
                //Check if it is the final floor
                if (finalFloor == true)
                {
                    //Call the PlayerWin method as the player has won the game
                    PlayerWin();
                }
                //If it is not final floor then show the continue button to allow progression to the next floor
                else
                {
                    //Disable the currentHatchNameTitle to display the continue button in its place
                    currentHatchNameTitle.SetActive(false);

                    //Set the currentHatchNameText to ""
                    currentHatchNameText.text = "";

                    //Enable the continue button
                    continueButton.SetActive(true);

                    //Set the gameBlockingPanelText to the current floor name + "\nCompleted"
                    gameBlockingPanelText.text = nameOfFloorText.text + "\nCompleted";

                    //Enable the gameBlockingPanel
                    gameBlockingPanel.SetActive(true);
                }
            }
        }
        //If the name of the clicked hatch was NOT equal to the current hatch name
        else
        {
            //Play sound when clicked
            SoundManager_Script.instance.PlaySound("Wrong Button");

            //Check if player has any lives left
            if (playerLives > 0)
            {
                //Decrease playerLives by 1
                playerLives--;

                //Update the playerlivesText
                playerLivesText.text = playerLives.ToString();
            }
            else
            {
                //Call the PlayerLose method as the player has lost
                PlayerLose();
            }
        }
    }

    //When the player clicks somewhere that isn't a hatch, decrease playerLives by 1
    private void OnWrongButtonClick()
    {
        //Play sound when clicked
        SoundManager_Script.instance.PlaySound("Wrong Button");

        //Check if player has any lives left
        if (playerLives > 0)
        {
            //Decrease playerLives by 1
            playerLives--;

            //Update the playerlivesText
            playerLivesText.text = playerLives.ToString();
        }
        else
        {
            //Call the PlayerLose method as the player has lost
            PlayerLose();
        }
    }

    //The player has completed the floor and has pressed the continue button
    private void OnContinueButtonClick()
    {
        //Play sound when clicked
        SoundManager_Script.instance.PlaySound("Menu Button");

        //Disable the continue button
        continueButton.SetActive(false);

        //Disable the gameBlockingPanel
        gameBlockingPanel.SetActive(false);

        //Resets all the buttons that were clicked to their default state
        ResetHatchesButtons();

        //Enable the currentHatchNameTitle
        currentHatchNameTitle.SetActive(true);

        //Call the StartRound method to go to the next floor
        StartRound();
    }

    //Resets the game and brings the player back to the map overview menu
    private void OnMapMenuButtonClick()
    {
        //Play sound when clicked
        SoundManager_Script.instance.PlaySound("Menu Button");

        //Reset all the buttons that were clicked to their default state
        ResetHatchesButtons();

        //Call the ResetFloors method which disables all the floor gameobjects
        floorsScriptObject.ResetFloors();

        //Reset the gameTimer to 0
        ResetGameTimer();

        //Reset the startOfGame bool to true
        startOfGame = true;

        //Enable the currentHatchNameTitle
        currentHatchNameTitle.SetActive(true);

        //Disable the continueButton
        continueButton.SetActive(false);
       
        //Disable the gameBlockingPanel
        gameBlockingPanel.SetActive(false);

        //Cancel "FirstRoundStart" Invoke
        CancelInvoke("FirstRoundStart");

        //Disable the endPanel
        endPanel.SetActive(false);

        //Disable the hatchesGamePanel
        hatchesGamePanel.SetActive(false);

        //Enable the mapOverviewPanel
        mapOverviewPanel.SetActive(true);
    }

    //Resets the game and loads a new round
    private void OnRetryButttonClick()
    {
        //Play sound when clicked
        SoundManager_Script.instance.PlaySound("Menu Button");

        //Reset the startOfGame bool to true
        startOfGame = true;

        //Enable the currentHatchNameTitle
        currentHatchNameTitle.SetActive(true);

        //Reset the gameTimer
        ResetGameTimer();

        //Reset all the buttons that were clicked to their default state
        ResetHatchesButtons();

        //Disable the endPanel
        endPanel.SetActive(false);

        //Reset the playerLives to 3
        playerLives = 3;

        //Update the playerlivesText
        playerLivesText.text = playerLives.ToString();

        //Disable all active floors objects
        floorsScriptObject.ResetFloors();

        //Call the StartRound method to get the floor data
        StartRound();
    }
    #endregion

    #region End State Methods

    //Is called when the player loses
    private void PlayerLose()
    {
        //Play sound when the player loses
        SoundManager_Script.instance.PlaySound("Lose");

        //Set the startGameTimer bool to false to disable the gameTimer
        startGameTimer = false;

        //Enable the endPanel
        endPanel.SetActive(true);

        //Set the endTimeText equal to the gameTimer
        endTimeText.text = gameTimer.ToString("0.0") + " Seconds";

        //Set the endPanelText to the name of the map + "\nFailed"
        endPanelText.text = nameOfMap + "\nFailed";

        //Set the colour of the endPanelText to red
        endPanelText.color = new Color(232.0f / 255.0f, 65.0f / 255.0f, 24.0f / 255.0f);
    }

    //Is called when the player wins
    private void PlayerWin()
    {
        //Play sound when the player wins
        SoundManager_Script.instance.PlaySound("Victory");

        //Set the startGameTimer bool to false to disable the gameTimer
        startGameTimer = false;

        //Enable the endPanel
        endPanel.SetActive(true);

        //Set the endTimeText equal to the gameTimer
        endTimeText.text = gameTimer.ToString("0.0") + " Seconds";

        //Set the endPanelText to the name of the map + "\nCompleted"
        endPanelText.text = nameOfMap + "\nCompleted";

        //Set the colour of the endPanelText to green
        endPanelText.color = new Color(76.0f / 255.0f, 209.0f / 255.0f, 55.0f / 255.0f);

        //Send the player's gameTimer score to the Hatches Scores Dictonary
        MainMenu_Script.instance.UpdateHatchesScoresDictionary(gameTimer);
    }
    #endregion

    #region Utility Method

    //Reset the gameTimer to 0
    private void ResetGameTimer()
    {
        //Set the startGameTimer bool to false to disable the gameTimer
        startGameTimer = false;

        //Set the gameTimer to 0
        gameTimer = 0;

        //Set the gameTimerText to "0.0 Seconds"
        gameTimerText.text = "0.0" + " Seconds";
    }

    //Reset the values of the clicked buttons
    private void ResetHatchesButtons()
    {
        //Reset all the clicked buttons in the hatchesButtonsList
        foreach (GameObject button in hatchesButtonsList)
        {
            //Disable the text in the child of the button
            button.GetComponentInChildren<TextMeshProUGUI>().enabled = false;

            //Enable the button component
            button.transform.GetComponent<Button>().enabled = true;

            //Disable the hatch image
            button.transform.Find("Hatch").GetComponent<Image>().enabled = false;
        }
    }
    #endregion
}
