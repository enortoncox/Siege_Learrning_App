using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;

public class Floors_Script : MonoBehaviour
{
    #region Variables

    //List to hold the gameobjects of each floor in a map
    List<Transform> floorList = new List<Transform>();

    //The index of the floor list which shows what floor the user is on
    int currentFloorIndex;

    //The bool to determine if it is the last floor
    bool lastFloor;

    //The bool to determine if it is the first floor
    bool firstFloor;

    //The name of the current floor
    string nameOfFloor;

    //List of the button names found in the current map's floor
    List<string> buttonNamesList;

    #endregion

    //Called once when the script is first enabled
    private void Awake()
    {
        //Add all child transform floors to the floorList
        foreach (Transform floor in this.transform)
        {
            //Add the floor to the list
            floorList.Add(floor);
        }

        //Set the floors and bools to their default state
        ResetFloors();
    }

    //Set the floor to the first one
    public void FloorSelected(out string nameOfFloor, out bool lastFloor, out List<string> nameList)
    {
        //Check if it is the first floor
        if (firstFloor == true)
        {          
            //Set the floor index to the first value
            currentFloorIndex = 0;

            //Set the firstFloor bool to false
            firstFloor = false;
        }
        //If not then disable the current floor, and then increase the index to the next floor if it isn't the last
        else
        {
            //Disable the current floor to be ready to enable the next
            floorList[currentFloorIndex].gameObject.SetActive(false);

            //Check if it's not the last floor
            if (currentFloorIndex != floorList.Count - 1)
            {
                //If it's not the last floor then increase the index by 1
                currentFloorIndex++;
            }
            else
            {
                //If it's the last floor then reset it back to the first floor
                currentFloorIndex = 0;
            }
        }

        //With the new index set the current floor to be active
        floorList[currentFloorIndex].gameObject.SetActive(true);

        //Check if the player is currently on the final floor
        if (currentFloorIndex == floorList.Count - 1)
        {
            //Set lastFloor to true
            lastFloor = true;
        }
        else
        {
            //Set lastFloor to false
            lastFloor = false;
        }

        //Get the name of the current floor
        nameOfFloor = floorList[currentFloorIndex].name;

        //Initilise nameList so it can be outputted
        nameList = new List<string>();

        //Populate the list with the name of each child button gameobject in the floor
        foreach(Transform obj in floorList[currentFloorIndex])
        {
            //Check if obj doesn't have the name 'Wrong Button' as this button isn't to be counted as part of the data
            if (obj.name != "Wrong Button")
            {
                //Add the name of the object in floorList to nameList
                nameList.Add(obj.name);
            }
        }

        //Use distinct to make sure the list has no duplicates
        nameList = nameList.Distinct().ToList();
    }
   
    //Disable all active floors (Used when leaving the game midway and at the very start)
    public void ResetFloors()
    {
        //Disable all active floors in the floorList
        foreach (Transform floor in floorList)
        {
            //disable the floor object
            floor.gameObject.SetActive(false);
        }

        //Reset the firstFloor bool to true
        firstFloor = true;
    }

    //Returns the number of floors in the floorList (Used to check if there is only one floor)
    public int NumOfFloors()
    {
        //Returns the number of floors in the floorList
        return floorList.Count;
    }
}
