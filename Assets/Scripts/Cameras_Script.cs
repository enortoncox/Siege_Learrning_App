using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
public class Cameras_Script : MonoBehaviour
{
    #region Variables
    //--PANELS--
    [Header("Panels")]

    //The Cameras Game Menu Panel
    [SerializeField]
    GameObject camerasGamePanel;

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

    //The text used to display the title above the remaining number of cameras
    [SerializeField]
    GameObject remainingCamerasTitle;

    //The text used to display the number of remaining cameras
    [SerializeField]
    TextMeshProUGUI remainingCamerasText;

    //The text used to display the player's lives
    [SerializeField]
    TextMeshProUGUI playerLivesText;

    //The text used to display the gameTimer
    [SerializeField]
    TextMeshProUGUI gameTimerText;

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
    List<GameObject> camerasButtonsList = new List<GameObject>();

    //This lists holds the name of each button on the current floor
    List<string> camerasNamesList;

    //--BOOLS--

    //The bool to determine if it's the start of the game
    bool startOfGame;

    //The bool to determine if the gameTimer should start counting
    bool startGameTimer;

    //The bool to determine if it is the final floor
    bool finalFloor;

    //--INTS & FLOATS--

    //The remaining lives that the player has left
    int playerLives;

    //The amount of cameras remaining on the current floor
    int numOfRemainingCameras;

    //The gameTimer tracks how long the player is taking to complete the game
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
    //Start is called before the first frame update
    private void Start()
    {
        //Set startOfGame to true at Start
        startOfGame = true;
    }

    // Update is called once per frame
    private void Update()
    {
        //If the game has begun then start the game timer
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
    //Called from the MapOverview menu when the CamerasButton is clicked; this sets up the start of the game
    private void InitialSetUp()
    {
        //Play sound when clicked
        SoundManager_Script.instance.PlaySound("Menu Button");

        //Enable the camerasGamePanel
        camerasGamePanel.SetActive(true);

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

        //Update the playerLivesText
        playerLivesText.text = playerLives.ToString();

        //Call StartRound method which gets the data from the floorsScriptObject and starts the round
        StartRound();
    }

    //Gets the data from the floorsScriptObject and starts the round
    private void StartRound()
    {
        //Get the nameOfFloor, finalFloor bool and the camerasNamesList from the floorsScriptObject
        floorsScriptObject.FloorSelected(out nameOfFloor, out finalFloor, out camerasNamesList);

        //Set the nameOfFloorText equal to the nameOfFloor starting at the end of the nameOfMap's length index (Bank1F becomes 1F)
        nameOfFloorText.text = nameOfFloor.Substring(nameOfMap.Length);

        //Set the numOfRemainingCameras equal to the size of the camerasNamesList
        numOfRemainingCameras = camerasNamesList.Count;

        //Set the remainingCamerasText equal to the numOfRemainingCameras
        remainingCamerasText.text = numOfRemainingCameras.ToString();

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

        //Set the startGameTimer to true to start the gameTimer
        startGameTimer = true;
    }
    #endregion

    #region Button Methods

    //When a camera button is clicked decrease the number of remaining cameras
    private void OnCameraButtonClick()
    {
        //Play sound when clicked
        SoundManager_Script.instance.PlaySound("Correct Button");

        //Assign the button that was clicked to a variable
        var thisButton = EventSystem.current.currentSelectedGameObject;

        //Disable the button component
        thisButton.GetComponent<Button>().enabled = false;

        //Enable the text in the child object
        thisButton.GetComponentInChildren<TextMeshProUGUI>().enabled = true;

        //Enable the camera image
        thisButton.transform.Find("Camera").GetComponent<Image>().enabled = true;

        //Add the clicked button to the camerasButtonsList
        camerasButtonsList.Add(thisButton);

        //Decrease the numOfRemainingCameras by 1
        numOfRemainingCameras--;

        //Update the remainingCamerasText
        remainingCamerasText.text = numOfRemainingCameras.ToString();

        //Check if all the cameras on this floor have been found
        if (numOfRemainingCameras == 0)
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
                //Disable the remainingCamerasTitle to display the continue button in its place
                remainingCamerasTitle.SetActive(false);

                //Set the remainingCamerasText to ""
                remainingCamerasText.text = "";

                //Enable the continue button
                continueButton.SetActive(true);

                //Set gameBlockingPanelText to the current floor name + "\nCompleted"
                gameBlockingPanelText.text = nameOfFloorText.text + "\nCompleted";

                //Enable the gameBlockingPanel
                gameBlockingPanel.SetActive(true);
            }
        }
    }

    //When the player clicks somewhere that isn't a camera, decrease playerLives by 1
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

        //Enable remainingCamerasTitle
        remainingCamerasTitle.SetActive(true);

        //Resets all the buttons that were clicked to their default state
        ResetCamerasButtons();

        //Call the StartRound method to go to the next floor
        StartRound();
    }

    //Resets the game and loads a new round
    private void OnRetryButtonClick()
    {
        //Play sound when clicked
        SoundManager_Script.instance.PlaySound("Menu Button");

        //Reset the startOfGame bool to true
        startOfGame = true;

        //Enable the remainingCamerasTitle
        remainingCamerasTitle.SetActive(true);

        //Reset the gameTimer
        ResetGameTimer();

        //Reset all the buttons that were clicked to their default state
        ResetCamerasButtons();

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

    //Resets the game and brings the player back to the map overview menu
    private void OnMapMenuButtonClick()
    {
        //Play sound when clicked
        SoundManager_Script.instance.PlaySound("Menu Button");

        //Reset all the buttons that were clicked to their default state
        ResetCamerasButtons();

        //Call the ResetFloors method which disables all the floor gameobjects
        floorsScriptObject.ResetFloors();

        //Reset the gameTimer to 0
        ResetGameTimer();

        //Reset the startOfGame bool to true
        startOfGame = true;

        //Enable the remainingCamerasTitle
        remainingCamerasTitle.SetActive(true);

        //Disable the continueButton
        continueButton.SetActive(false);

        //Disable the gameBlockingPanel
        gameBlockingPanel.SetActive(false);

        //Cancel "FirstRoundStart" Invoke
        CancelInvoke("FirstRoundStart");

        //Disable the endPanel
        endPanel.SetActive(false);

        //Disable the camerasGamePanel
        camerasGamePanel.SetActive(false);

        //Enable the mapOverviewPanel
        mapOverviewPanel.SetActive(true);
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

        //Send the player's gameTimer score to the Cameras Scores Dictonary
        MainMenu_Script.instance.UpdateCameraScoresDictionary(gameTimer);
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
        gameTimerText.text = "0.0 Seconds";
    }

    //Reset the values of the clicked buttons
    private void ResetCamerasButtons()
    {
        //Reset all the clicked buttons in the camerasButtonsList
        foreach (GameObject button in camerasButtonsList)
        {
            //Disable the text component
            button.GetComponentInChildren<TextMeshProUGUI>().enabled = false;

            //Enable the button component
            button.GetComponent<Button>().enabled = true;

            //Disable the Camera image
            button.transform.Find("Camera").GetComponent<Image>().enabled = false;
        }
    } 
    #endregion
}
