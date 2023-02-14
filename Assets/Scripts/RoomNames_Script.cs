using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class RoomNames_Script : MonoBehaviour
{
    #region Variables
    //--PANELS--
    [Header("Panels")]

    //The Room Names Menu Panel
    [SerializeField]
    GameObject roomNamesPanel;

    //The Map Overview Menu Panel
    [SerializeField]
    GameObject mapOverviewPanel;

    //The buttonBlockingPanel stops an incorrect button being pressed twice
    [SerializeField]
    GameObject buttonBlockingPanel;

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

    //The text used to display the remaining rooms
    [SerializeField]
    TextMeshProUGUI remainingRoomsText;

    //The text used to display the player's lives
    [SerializeField]
    TextMeshProUGUI playerLivesText;

    //The text used to display the gameTimer
    [SerializeField]
    TextMeshProUGUI gameTimerText;

    //The title above the current room name
    [SerializeField]
    GameObject currentRoomNameTitle;

    //The text used to display the current room name
    [SerializeField]
    TextMeshProUGUI currentRoomNameText;

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

    //The list to hold all the map buttons that have been clicked successfully so far
    List<GameObject> roomNamesButtonsList = new List<GameObject>();

    //This lists holds the name of each button on the current floor
    List<string> roomNamesList;

    //--BOOLS--

    //The bool to determine if it's the start of the game
    bool startOfGame;

    //The bool to determine if the gameTimer should start counting
    bool startGameTimer;

    //The bool to determine if it is the final floor
    bool finalFloor;
   
    //--INTS & FLOATS--

    //The current index in the roomNamesList
    int currentIndex;

    //The last used index in the roomNamesList
    int lastIndex;

    //The remaining lives that the player has left
    int playerLives;

    //The amount of rooms remaining on the current floor
    int numOfRemainingRooms;

    //The gameTimer tracks how long the player is taking to complete the game
    float gameTimer;

    //--STRINGS--

    //The name of the current map
    string nameOfMap;

    //The name of the current floor
    string nameOfFloor;
   
    //--BUTTONS--
    [Header("Buttons")]

    //The continue button is used to advance to the next floor
    [SerializeField]
    GameObject continueButton;

    //The temporary holder for the map button that has just been clicked
    GameObject thisButton;
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
    //Called from the MapOverview menu when the RoomNamesButton is clicked; this sets up the start of the game
    private void InitialSetUp()
    {
        //Play sound when clicked
        SoundManager_Script.instance.PlaySound("Menu Button");

        //Enable the roomNamesPanel
        roomNamesPanel.SetActive(true);

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

        //Enable currentRoomNameTitle
        currentRoomNameTitle.SetActive(true);

        //Call StartRound method which gets the data from the floorsScriptObject and starts the round
        StartRound();
    }
   
    //Gets the data from the floorsScriptObject and starts the round
    private void StartRound()
    {
        //Get the nameOfFloor, finalFloor bool and the roomNamesList from the floorsScriptObject
        floorsScriptObject.FloorSelected(out nameOfFloor, out finalFloor, out roomNamesList);

        //Set the nameOfFloorText equal to the nameOfFloor starting at the end of the nameOfMap's length index (Bank1F becomes 1F)
        nameOfFloorText.text = nameOfFloor.Substring(nameOfMap.Length);

        //Set the numOfRemainingRooms equal to the size of the roomNamesList
        numOfRemainingRooms = roomNamesList.Count;

        //Set the remainingRoomsText equal to the numOfRemainingRooms
        remainingRoomsText.text = numOfRemainingRooms.ToString();

        //Call the SetNewRoomName method to assign a random room name
        SetNewRoomName();

        //Check if it's the startOfGame
        if (startOfGame == true)
        {
            //Set the startOfGame bool to false as the game has started
            startOfGame = false;

            //Set gameBlockingPanelText to "Ready?"
            gameBlockingPanelText.text = "Ready?";

            //Enable the gameBlockingPanel
            gameBlockingPanel.SetActive(true);
           
            //Invoke the FirstRoundStart method after 1 second
            Invoke("FirstRoundStart", 1F);
        }
    }

    //Delays the start of the first round. Called from StartRound() via Invoke
    private void FirstRoundStart()
    {
        //Disable the gameBlockingPanel
        gameBlockingPanel.SetActive(false);

        //Set the startGameTimer bool to true to start the gameTimer
        startGameTimer = true;
    }

    //Set a new room name to be found
    private void SetNewRoomName()
    {
        //Assign the value of the currentIndex to the lastIndex variable
        lastIndex = currentIndex;

        //Set the current index to a range between 0 and the current size of the roomNamesList
        currentIndex = Random.Range(0, roomNamesList.Count);

        //A while loop to check if the current room name is the same as the last
        while (currentIndex == lastIndex && roomNamesList.Count > 1)
        {
            //Set the current index to a range between 0 and the current size of the roomNamesList
            currentIndex = Random.Range(0, roomNamesList.Count);
        }

        //Update the currentRoomNameText with the new room name
        currentRoomNameText.text = roomNamesList[currentIndex];
    }
    #endregion

    #region Button Methods

    //When a map button is clicked, check if its name matches the current room name
    private void OnRoomNameButtonClick()
    {
        //Assign the button that was clicked to a variable
        thisButton = EventSystem.current.currentSelectedGameObject;

        //If the button wasn't null and the button's name is equal to the current room name
        if (thisButton != null && thisButton.name == currentRoomNameText.text)
        {
            //Play sound when clicked
            SoundManager_Script.instance.PlaySound("Correct Button");

            //Disable the button component
            thisButton.GetComponent<Button>().enabled = false;

            //Disable the image component
            thisButton.GetComponent<Image>().enabled = false;

            //Enable the text in the child object
            thisButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;

            //Add the clicked button to the roomNamesButtonsList
            roomNamesButtonsList.Add(thisButton);

            //Remove the clicked button's name from the roomNamesList
            roomNamesList.RemoveAt(currentIndex);

            //Decrease the numOfRemainingRooms by 1
            numOfRemainingRooms--;

            //Update the remainingRoomsText
            remainingRoomsText.text = numOfRemainingRooms.ToString();

            //If there are still rooms left then set a new room
            if (roomNamesList.Count > 0)
            {
                //Set a new room name
                SetNewRoomName();
            }
            //If there are no rooms left then check for player win or next floor
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
                    //Disable the currentRoomNameTitle to display the continue button in its place
                    currentRoomNameTitle.SetActive(false);

                    //Set the currentRoomNameText to ""
                    currentRoomNameText.text = "";

                    //Enable the continue button
                    continueButton.SetActive(true);

                    //Set the gameBlockingPanelText to the current floor name + "\nCompleted"
                    gameBlockingPanelText.text = nameOfFloorText.text + "\nCompleted";

                    //Enable the gameBlockingPanel
                    gameBlockingPanel.SetActive(true);
                }
            }
        }
        //If the name of the clicked button was NOT equal to the current room name
        else
        {          
            //Check if player has any lives left
            if (playerLives > 0)
            {
                //Play sound when clicked
                SoundManager_Script.instance.PlaySound("Wrong Button");

                //Make the clicked button not interactable
                thisButton.transform.GetComponent<Button>().interactable = false;

                //Enable the buttonBlockingPanel to stop another button being clicked
                buttonBlockingPanel.SetActive(true);

                //Invoke the SpriteReset method which disables the buttonBlockingPanel after 0.7 seconds
                Invoke("SpriteReset", 0.7f);

                //Decrease playerLives by 1
                playerLives--;

                //Update the playerlivesText
                playerLivesText.text = playerLives.ToString();

                //Set a new room name
                SetNewRoomName();
            }
            //Player has NO lives left
            else
            {
                //Call the PlayerLose method as the player has lost
                PlayerLose();
            }            
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
        ResetRoomNameButtons();

        //Enable the currentRoomNameTitle
        currentRoomNameTitle.SetActive(true);

        //Call the StartRound method to go to the next floor
        StartRound();
    }

    //Resets the game and brings the player back to the Map Overview menu
    private void OnMapMenuButtonClick()
    {
        //Play sound when clicked
        SoundManager_Script.instance.PlaySound("Menu Button");

        //Reset all the buttons that were clicked to their default state
        ResetRoomNameButtons();

        //Call the ResetFloors method which disables all the floor gameobjects
        floorsScriptObject.ResetFloors();

        //Reset the gameTimer to 0
        ResetGameTimer();
       
        //Reset the startOfGame bool to true
        startOfGame = true;

        //Enable the currentRoomNameTitle
        currentRoomNameTitle.SetActive(true);

        //Disable the continueButton
        continueButton.SetActive(false);

        //Disable the gameBlockingPanel
        gameBlockingPanel.SetActive(false);

        //Cancel "FirstRoundStart" Invoke
        CancelInvoke("FirstRoundStart");

        //Disable the endPanel
        endPanel.SetActive(false);

        //Disable the roomNamesPanel
        roomNamesPanel.SetActive(false);

        //Enable the mapOverviewPanel
        mapOverviewPanel.SetActive(true);
    }

    //Resets the game and loads a new round
    private void OnRetryButtonClick()
    {
        //Play sound when clicked
        SoundManager_Script.instance.PlaySound("Menu Button");

        //Reset the startOfGame bool to true
        startOfGame = true;

        //Enable the currentRoomNameTitle
        currentRoomNameTitle.SetActive(true);

        //Reset the gameTimer
        ResetGameTimer();

        //Reset all the buttons that were clicked to their default state
        ResetRoomNameButtons();

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
        //Play sound when player loses
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
        //Play sound when player wins
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

        //Send the player's gameTimer score to the Room Names Scores Dictonary
        MainMenu_Script.instance.UpdateRNScoresDictonary(gameTimer);       
    }
    #endregion

    #region Utility Methods
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
    private void ResetRoomNameButtons()
    {
        //Reset all the clicked buttons in the roomNamesButtonsList
        foreach (GameObject button in roomNamesButtonsList)
        {
            //Enable the image component
            button.GetComponent<Image>().enabled = true;

            //Disable the text component
            button.GetComponentInChildren<TextMeshProUGUI>().enabled = false;

            //Enable the button component
            button.GetComponent<Button>().enabled = true;
        }
    }

    //Disables the buttonBlockingPanel and makes the button that was clicked interactable again
    private void SpriteReset()
    {
        //Disable the buttonBlockingPanel
        buttonBlockingPanel.SetActive(false);

        //Set the clicked button to be interactable again
        thisButton.transform.GetComponent<Button>().interactable = true;
    }
    #endregion
}
