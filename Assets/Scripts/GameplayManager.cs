using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour
{
    // the player in the game.
    public Player player;

    // displays the water level.
    public Text waterLevelText;

    // concerns water variables.
    [Header("Water")]

    // drag variables
    // changes mass if leaving water.
    // the air drag is the default for all objects.
    public float waterDragFactor = 1.95F;

    // water level changers
    [Header("Water/Levels")]

    // the current water level in the scene.
    public int currentWaterLevel = 4;

    // if 'true', the water level is limited to the min and max.
    public bool limitWaterLevels = true;

    // the minimum water level.
    public int waterLevelMin = 1;

    // the maximum water level.
    public int waterLevelMax = 5;

    // the difference in water level heights
    public float waterHeightDiff = 10.0F;

    // the change that occurs in the a
    public float globalChangeRate = 20.0F;

    // if 'true', the set water level change is used for all bodies of water in the list.
    public bool overrideLocalChangeRate = true;

    // the list of changable water bodies.
    public List<WaterLevelChanger> changableWater = new List<WaterLevelChanger>();

    // if 'true', the list looks for water bodies that can be changed.
    public bool findChangableWater = true;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = FindObjectOfType<Player>();

        if(findChangableWater)
        {
            // if the list should be checked for duplicates.
            bool checkForDupes = changableWater.Count != 0;

            // finds the objects.
            WaterLevelChanger[] temp = FindObjectsOfType<WaterLevelChanger>(); // find
            changableWater.AddRange(temp); // add

            // checks for duplicates.
            if(checkForDupes)
                changableWater = changableWater.Distinct().ToList(); // remove duplicates

        }

        // displays current water level.
        if (waterLevelText != null)
            waterLevelText.text = "Water Level: " + currentWaterLevel.ToString();
    }

    // applies the water drag to the rigidbody.
    public void ApplyWaterDrag(Rigidbody rb)
    {
        rb.drag = (rb.drag + 1) * waterDragFactor;
    }

    // removes the water drag.
    public void RemoveWaterDrag(Rigidbody rb)
    {
        rb.drag = (rb.drag / waterDragFactor) - 1; // reverses the equation.
    }

    // raises the water levels.
    public void RaiseWaterLevels()
    {
        SetWaterLevels(currentWaterLevel + 1);
    }

    // drops the water levels.
    public void DropWaterLevels()
    {
        SetWaterLevels(currentWaterLevel - 1);
    }
    
    // changes the water level
    public void SetWaterLevels(int newLevel)
    {
        // if the limits are on, and the new level would surpass them, do nothing.
        if (limitWaterLevels && (newLevel < waterLevelMin || newLevel > waterLevelMax))
            return;

        // no change
        if (newLevel == currentWaterLevel)
            return;

        // the height multiplier.
        float heightMult = newLevel - currentWaterLevel;

        // goes through all water bodies.
        foreach(WaterLevelChanger waterBody in changableWater)
        {
            // checks if the local change should be overwritten.
            // if (overrideLocalChangeRate)
            //     waterBody.changeRate = (newLevel - currentWaterLevel) * globalChangeRate;
            // else
            //     waterBody.changeRate *= (newLevel < currentWaterLevel) ? -1.0F : 1.0F; 

            
            // changes the water level.
            waterBody.ChangeLevel(waterHeightDiff * heightMult, globalChangeRate);
        }

        // save current level.
        currentWaterLevel = newLevel;

        // update text.
        if (waterLevelText != null)
            waterLevelText.text = "Water Level: " + currentWaterLevel.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
