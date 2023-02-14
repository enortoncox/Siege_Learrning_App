using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;
using System.Linq;

public class MainMenu_Script : MonoBehaviour
{
    #region Variables
    //--MENU PANELS--
    [Header("Menu Panels")]

    //The MainMenu Panel
    [SerializeField]
    GameObject mainMenuPanel;

    //The Map Overview panel
    [SerializeField]
    GameObject mapOverviewPanel;

    //--GAME PANELS--
    [Header("Game Panels")]

    //The Room Names Panel
    [SerializeField]
    GameObject roomNamesPanel;

    //The Room Names Research Panel
    [SerializeField]
    GameObject roomNamesResearchPanel;

    //The Cameras Panel
    [SerializeField]
    GameObject camerasPanel;

    //The Cameras Research Panel
    [SerializeField]
    GameObject camerasResearchPanel;

    //The Hatches Panel
    [SerializeField]
    GameObject hatchesPanel;

    //The Hatches Research Panel
    [SerializeField]
    GameObject hatchesResearchPanel;

    //--TEXT GAMEOBJECTS--
    [Header("Text Gameobjects")]

    //The text used to display the current map name
    [SerializeField]
    TextMeshProUGUI nameOfMapText;

    //Map Overview bestRNtime text
    [SerializeField]
    TextMeshProUGUI bestRNTimeText;

    //Map Overview bestCameratime text
    [SerializeField]
    TextMeshProUGUI bestCameraTimeText;

    //Map Overview bestHatchtime text
    [SerializeField]
    TextMeshProUGUI bestHatchTimeText;

    //--Dictionaries--

    //Dictionary to store Room Names scores
    Dictionary<string, float> scoresRNDict;

    //Dictionary to store Cameras scores
    Dictionary<string, float> scoresCamerasDict;

    //Dictionary to store Hatches scores
    Dictionary<string, float> scoresHatchesDict;

    //--STRINGS--

    //The current map name
    public string currentSelectedMap;

    //The datapath for the location of RNScoresDictionary.txt
    string dataPathRN;

    //The datapath for the location of CamerasScoresDictionary.txt
    string dataPathCameras;

    //The datapath for the location of HatchesScoresDictionary.txt
    string dataPathHatches;

    //--BOOLS--

    //The bool to determine if the music is playing
    bool musicPlaying;

    //--SCRIPTS--

    //A static instance of the MainMenu_Script
    public static MainMenu_Script instance = null;
    #endregion

    #region Unity Methods

    //Awake is used to initialize any variables or game state before the game starts
    private void Awake()
    {
        //If the current instance is not null
        if (instance != null)
        {
            //If the current instance is not equal to this gameObject
            if (instance != this)
            {
                //Destroy this gameObject as an instance of the MainMenu_Script already exists
                Destroy(gameObject);
            }
        }
        //Else if the current instance is null
        else
        {
            //Set the current instance equal to this gameObject
            instance = this;
        }
    }

    //Start is called before the first frame update
    private void Start()
    {
        //Set musicPlaying to true
        musicPlaying = true;

        //Set the currentSelectedMap to a default value
        currentSelectedMap = "No Map Selected";

        //Populate the scores dictionaries with the initial keys matching the map names and setting each score to a default 0
        PopulateScoresDictionaries();

        //Set the dataPathRN to lead to the RNScoresDictionary.txt file
        dataPathRN = Application.streamingAssetsPath + @"\TextFiles\RNScoresDictionary.txt";

        //Call the LoadScoresToDictionary method with the scoresRNDict as input
        LoadScoresToDictionary(scoresRNDict, dataPathRN);

        //Set the dataPathCameras to lead to the CamerasScoresDictionary.txt file
        dataPathCameras = Application.streamingAssetsPath + @"\TextFiles\CamerasScoresDictionary.txt";

        //Call the LoadScoresToDictionary method with the scoresCamerasDict as input
        LoadScoresToDictionary(scoresCamerasDict, dataPathCameras);

        //Set the dataPathHatches to lead to the HatchesScoresDictionary.txt file
        dataPathHatches = Application.streamingAssetsPath + @"\TextFiles\HatchesScoresDictionary.txt";

        //Call the LoadScoresToDictionary method with the scoresHatchesDict as input
        LoadScoresToDictionary(scoresHatchesDict, dataPathHatches);

        //Disable all menus to ensure none are active at start
        DisableAllMenus();

        //Enable mainMenuPanel
        mainMenuPanel.SetActive(true);
    }
    #endregion

    #region Button Methods

    //When a map is clicked from the scroll rect, load the Map Overview menu
    private void OnMapOverviewButtonClick()
    {
        //Assign the button clicked to a variable
        currentSelectedMap = EventSystem.current.currentSelectedGameObject.name;

        //Set the mapOverviewPanel to active
        mapOverviewPanel.SetActive(true);

        //Disable the mainMenuPanel
        mainMenuPanel.SetActive(false);

        //Set the nameOfMapText to the current map's name
        nameOfMapText.text = currentSelectedMap;

        //Set the roomNameScoreTime equal to the score in the dictionary
        bestRNTimeText.text = scoresRNDict[currentSelectedMap].ToString("0.0");

        //Set camera score time equal to the score in the dictionary
        bestCameraTimeText.text = scoresCamerasDict[currentSelectedMap].ToString("0.0");

        //Set hatches score time equal to the score in the dictionary
        bestHatchTimeText.text = scoresHatchesDict[currentSelectedMap].ToString("0.0");

        //Play sound when clicked
        SoundManager_Script.instance.PlaySound("Menu Button");
    }

    //While in the Map Overview menu, disable it and enable the Main Menu
    private void OnMapSelectButtonClick()
    {
        //Disable the mapOverviewPanel
        mapOverviewPanel.SetActive(false);

        //Enable the mainMenuPanel
        mainMenuPanel.SetActive(true);

        //Play sound when clicked
        SoundManager_Script.instance.PlaySound("Menu Button");
    }

    //When the exit button is clicked quit the game
    private void OnExitButtonClick()
    {
        //Play sound when clicked
        SoundManager_Script.instance.PlaySound("Menu Button");

        //Quit the game
        Application.Quit();
    }

    //Toggle the music on or off using the checkbox toggle
    private void OnMusicToggleClicked()
    {
        //Is the music currently playing?
        if (musicPlaying == true)
        {
            //Set musicPlaying to false
            musicPlaying = false;

            //Call the StopSound method to stop the music playing
            SoundManager_Script.instance.StopSound("Main Menu Music");
        }
        //The music is NOT currently playing
        else
        {
            //Set musicPlaying to true
            musicPlaying = true;

            //Call the PlaySound method to make the music play
            SoundManager_Script.instance.PlaySound("Main Menu Music");
        }
    }
    #endregion

    #region Dictionary Methods

    //Set a default value for each key pair in the scores dictionaries
    private void PopulateScoresDictionaries()
    {
        //Room Name dictionary
        scoresRNDict = new Dictionary<string, float>
        {
            { "Bank", 0 },
            { "Border", 0 },
            { "Chalet", 0 },
            { "Clubhouse", 0 },
            { "Coastline", 0 },
            { "Consulate", 0 },
            { "Favela", 0 },
            { "Fortress", 0 },
            { "Hereford", 0 },
            { "House", 0 },
            { "Kafe", 0 },
            { "Kanal", 0 },
            { "Oregon", 0 },
            { "Outback", 0 },
            { "Plane", 0 },
            { "Skyscraper", 0 },
            { "Theme Park", 0 },
            { "Tower", 0 },
            { "Villa", 0 },
            { "Yacht", 0 }
        };

        //Cameras dictionary
        scoresCamerasDict = new Dictionary<string, float>
        {
            { "Bank", 0 },
            { "Border", 0 },
            { "Chalet", 0 },
            { "Clubhouse", 0 },
            { "Coastline", 0 },
            { "Consulate", 0 },
            { "Favela", 0 },
            { "Fortress", 0 },
            { "Hereford", 0 },
            { "House", 0 },
            { "Kafe", 0 },
            { "Kanal", 0 },
            { "Oregon", 0 },
            { "Outback", 0 },
            { "Plane", 0 },
            { "Skyscraper", 0 },
            { "Theme Park", 0 },
            { "Tower", 0 },
            { "Villa", 0 },
            { "Yacht", 0 }
        };

        //Hatches dictionary
        scoresHatchesDict = new Dictionary<string, float>
        {
            { "Bank", 0 },
            { "Border", 0 },
            { "Chalet", 0 },
            { "Clubhouse", 0 },
            { "Coastline", 0 },
            { "Consulate", 0 },
            { "Favela", 0 },
            { "Fortress", 0 },
            { "Hereford", 0 },
            { "House", 0 },
            { "Kafe", 0 },
            { "Kanal", 0 },
            { "Oregon", 0 },
            { "Outback", 0 },
            { "Plane", 0 },
            { "Skyscraper", 0 },
            { "Theme Park", 0 },
            { "Tower", 0 },
            { "Villa", 0 },
            { "Yacht", 0 }
        };
    }

    //Update the Room Names scores dictionary with the new score
    public void UpdateRNScoresDictonary(float score)
    {
        //Check if the current score is 0 (Is this the first new score?)
        if (scoresRNDict[currentSelectedMap] == 0)
        {
            //Update dictionary with the new score
            scoresRNDict[currentSelectedMap] = score;

            //Set bestRNTimeText.text equal to the value of the new score
            bestRNTimeText.text = score.ToString("0.0");

            //Call the SaveScoresToTextFile() method to save the new score to the text file
            SaveScoresToTextFile(scoresRNDict, dataPathRN);
        }
        //Check if current score is greater than the new score (Is the new score better than the current score?)
        else if (scoresRNDict[currentSelectedMap] > score)
        {
            //Update the dictionary with the new score
            scoresRNDict[currentSelectedMap] = score;

            //Set bestRNTimeText.text equal to the value of the new score
            bestRNTimeText.text = score.ToString("0.0");

            //Call the SaveScoresToTextFile() method to save the new score to the text file
            SaveScoresToTextFile(scoresRNDict, dataPathRN);
        }       
    }

    //Update the Cameras scores dictionary with the new score
    public void UpdateCameraScoresDictionary(float score)
    {
        //Check if the current score is 0 (Is this the first new score?)
        if (scoresCamerasDict[currentSelectedMap] == 0)
        {
            //Update dictionary with the new score
            scoresCamerasDict[currentSelectedMap] = score;

            //Set bestCameraTimeText.text equal to the value of the new score
            bestCameraTimeText.text = score.ToString("0.0");

            //Call the SaveScoresToTextFile() method to save the new score to the text file
            SaveScoresToTextFile(scoresCamerasDict, dataPathCameras);
        }
        //Check if current score is greater than the new score (Is the new score better than the current score?)
        else if (scoresCamerasDict[currentSelectedMap] > score)
        {
            //Update the dictionary with the new score
            scoresCamerasDict[currentSelectedMap] = score;

            //Set bestCameraTimeText.text equal to the value of the new score
            bestCameraTimeText.text = score.ToString("0.0");

            //Call the SaveScoresToTextFile() method to save the new score to the text file
            SaveScoresToTextFile(scoresCamerasDict, dataPathCameras);
        }       
    }

    //Update the Hatches scores dictionary with the new score
    public void UpdateHatchesScoresDictionary(float score)
    {
        //Check if the current score is 0 (Is this the first new score?)
        if (scoresHatchesDict[currentSelectedMap] == 0)
        {
            //Update dictionary with new score
            scoresHatchesDict[currentSelectedMap] = score;

            //Set bestHatchTimeText.text equal to the value of the new score
            bestHatchTimeText.text = score.ToString("0.0");

            //Call the SaveScoresToTextFile() method to save the new score to the text file
            SaveScoresToTextFile(scoresHatchesDict, dataPathHatches);
        }
        //Check if current score is greater than the new score (Is the new score better than the current score?)
        else if (scoresHatchesDict[currentSelectedMap] > score)
        {
            //Update dictionary with new score
            scoresHatchesDict[currentSelectedMap] = score;

            //Set bestHatchTimeText.text equal to the value of the new score
            bestHatchTimeText.text = score.ToString("0.0");

            //Call the SaveScoresToTextFile() method to save the new score to the text file
            SaveScoresToTextFile(scoresHatchesDict, dataPathHatches);
        }        
    }
    #endregion

    #region Utility Methods
    //Disable all menus before the game starts
    private void DisableAllMenus()
    {
        //Disable the roomNamesPanel
        roomNamesPanel.SetActive(false);

        //Disable the roomNamesReseachPanel
        roomNamesResearchPanel.SetActive(false);

        //Disable the camerasPanel
        camerasPanel.SetActive(false);

        //Disable the camerasResearchPanel
        camerasResearchPanel.SetActive(false);

        //Disable the hatchesPanel
        hatchesPanel.SetActive(false);

        //Disable the hatchesResearchPanel
        hatchesResearchPanel.SetActive(false);
    }

    //Save the new score to the text file
    private void SaveScoresToTextFile(Dictionary<string, float> dict, string dataPath)
    {
        //Create a StreamWriter using the path to the text file
        using (StreamWriter sw = new StreamWriter(dataPath))
        {
            //For each KeyValuePair in the dictionary write its data to the text file
            foreach (KeyValuePair<string, float> data in dict)
            {
                //Write the current KeyValuepair with a ':' char between them to the text file
                sw.WriteLine(data.Key + ":" + data.Value);
            }
        }
    }

    //At the start of the game, load in the data values to the dictionary
    private void LoadScoresToDictionary(Dictionary<string, float> dict, string path)
    {
        //Create a StreamReader using the path to the text file
        using (StreamReader reader = new StreamReader(File.Open(path, FileMode.OpenOrCreate)))
        {
            //A string to hold the current line in the text's value
            string line;

            //A float to hold the value part of the data
            float scoreValue;

            //While the line being read is not null
            while ((line = reader.ReadLine()) != null)
            {
                //Split the line via the ':' char to split the key and the value apart
                string[] data = line.Split(':');

                //Check if the data contains both a key and a value
                if (data.Length > 1)
                {
                    //Check if the dictionary contains the key
                    if (dict.ContainsKey(data[0]))
                    {
                        //Check if data[1] can be converted to a float
                        if (float.TryParse(data[1], out scoreValue))
                        {
                            //Set the value for the data[0] key in the dict equal to scoreValue
                            dict[data[0]] = scoreValue;
                        }
                        //If it's not a float then send LogWarning
                        else
                        {
                            Debug.LogWarning("MainMenu_Script: The value '" + data[1] + "' is not a float");
                        }
                    }
                    //If it's not a key then send LogWarning
                    else
                    {
                        Debug.LogWarning("MainMenu_Script: The key '" + data[0] + "' is not in the dictionary");
                    }
                }
                //If it can't be split then send LogWarning
                else
                {
                    Debug.LogWarning("MainMenu_Script: The line '" + line + "' couldn't be split");
                }
            }
        }
    }
    #endregion
}
