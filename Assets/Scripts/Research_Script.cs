using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Research_Script : MonoBehaviour
{
    #region Variables
    //--PANELS--
    [Header("Panels")]
    
    //The Current Research Panel
    [SerializeField]
    GameObject currentResearchPanel;

    //The Map Overview Menu Panel
    [SerializeField]
    GameObject mapOverviewPanel;

    //The panel that holds all the map data gameobjects
    [SerializeField]
    GameObject mapsDataPanel;

    //--Text GameObjects--
    [Header("Text GameObjects")]

    //The text used to display the total amount of objects on the current floor
    [SerializeField]
    TextMeshProUGUI totalObjectsText;

    //The text used to display the current map name
    [SerializeField]
    TextMeshProUGUI nameOfMapText;

    //The text used to display the current floor name
    [SerializeField]
    TextMeshProUGUI nameOfFloorText;

    //--SCRIPTS--
    [Header("Scripts")]

    //The floorsScriptObject attached to the current map gameobject
    Floors_Script floorsScriptObject;

    //--STRINGS--

    //The name of the current map
    string nameOfMap;

    //The name of the current floor
    string nameOfFloor;

    //--LISTS--

    //This lists holds all the names of each gameobject on the current floor
    List<string> objectsNamesList;

    //--BOOLS--

    //The bool to determine if it is the final floor
    bool finalFloor;

    //--INTS--

    //The total number of cameras on the current floor
    int totalObjects;

    //--BUTTONS--
    [Header("Buttons")]

    //The nextButton to change to the next floor
    [SerializeField]
    GameObject nextButton;
    #endregion

    #region Methods
    //Called from the MapOverview menu when the ResearchButton is clicked; this sets up the start of the game
    private void InitialSetUp()
    {
        //Enable the currentResearchPanel
        currentResearchPanel.SetActive(true);

        //Disable the mapOverviewPanel
        mapOverviewPanel.SetActive(false);

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

        //Call the DisplayData method to load the floor
        DisplayData();
    }

    //Gets the data from the floorsScriptObject
    private void DisplayData()
    {
        //Play sound when clicked
        SoundManager_Script.instance.PlaySound("Menu Button");

        //Get the nameOfFloor, finalFloor bool and the objectsNamesList from the floorsScriptObject
        floorsScriptObject.FloorSelected(out nameOfFloor, out finalFloor, out objectsNamesList);

        //Set the totalObjects equal to the size of the objectsNamesList
        totalObjects = objectsNamesList.Count;

        //Update the totalObjectsText
        totalObjectsText.text = totalObjects.ToString();

        //Set the nameOfFloorText equal to the nameOfFloor starting at the end of the nameOfMap's length index
        nameOfFloorText.text = nameOfFloor.Substring(nameOfMap.Length);

        //Check if the map only has one floor
        if (floorsScriptObject.NumOfFloors() <= 1)
        {
            //Disable the nextButton
            nextButton.SetActive(false);
        }
    }

    //Resets the game and brings the player back to the Map Overview Menu
    private void OnMapMenuButtonClick()
    {
        //Play sound when clicked
        SoundManager_Script.instance.PlaySound("Menu Button");

        //Call the ResetFloors method which disables all the floor gameobjects
        floorsScriptObject.ResetFloors();

        //Enable Map Overview panel
        mapOverviewPanel.SetActive(true);

        //Disable the currentResearchPanel
        currentResearchPanel.SetActive(false);

        //Enable the nextButton
        nextButton.SetActive(true);
    }
    #endregion
}
