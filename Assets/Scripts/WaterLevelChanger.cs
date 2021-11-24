using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// changes the water level.
// NOTE: the hitbox size does not change with the height of the water.
// - In a full version, it would, but since this is a demo (and we have little time), it's being left as if.
// - Maybe someone will fix it, but it's not a high priority.
// - Anyway, make sure the hitbox reaches the sea floor.
public class WaterLevelChanger : MonoBehaviour
{
    // if 'true', the changer is enabled.
    public bool enableChanger = true;

    // the instant change for the water level.
    public bool instantChange = false;

    // the rate of the water level change.
    [Tooltip("The rate of change in units per Update() call (gets multiplied by delta time).")]
    public float changeRate = 50.0F;

    // the ending y-value when the water level is changing.
    private float endingPosY;

    // Start is called before the first frame update
    void Start()
    {
        endingPosY = transform.position.y;
    }

    // sets the water height.
    public void SetHeight(float newHeight)
    {
        ChangeLevel(newHeight - transform.position.y);
    }

    // sets the water height.
    public void SetHeight(float newHeight, float changeRate)
    {
        ChangeLevel(newHeight - transform.position.y, changeRate);
    }

    // changes the water level.
    public void ChangeLevel(float heightChange)
    {
        // sets to the ending position just in case it hasn't reached it yet.
        if(transform.position.y != endingPosY)
        {
            // adjusts height change accordingly.
            heightChange = (endingPosY + heightChange) - transform.position.y;
        }

        // checks if the change has happened.
        if(instantChange)
        {
            transform.position += new Vector3(0.0F, heightChange, 0.0F);
            endingPosY = transform.position.y;
        }
        else
        {
            // end result
            endingPosY = transform.position.y + heightChange;
        }
    }

    // changes the level, and the change rate.
    public void ChangeLevel(float heightChange, float changeRate)
    {
        ChangeLevel(heightChange);
        this.changeRate = changeRate;
    }

    // Update is called once per frame
    void Update()
    {
        // don't change the water.
        if (!enableChanger)
            return;

        // if the y-value is not been met yet.
        if (transform.position.y != endingPosY)
        {
            // direction
            float direc = (endingPosY > transform.position.y) ? 1.0F : -1.0F;

            // new position.
            Vector3 newPos = transform.position + new Vector3(0.0F, changeRate * direc * Time.deltaTime, 0.0F);
            
            // clamps to meet right position.
            if(direc > 0.0F) // was rising up
            {
                if (newPos.y > endingPosY)
                    newPos.y = endingPosY;
            }
            else if(direc < 0.0F) // was going down
            {
                if (newPos.y < endingPosY)
                    newPos.y = endingPosY;
            }

            // if it has reached its position.
            if ((direc > 0.0F && newPos.y > endingPosY) || 
                (direc < 0.0F && newPos.y < endingPosY))
                newPos.y = endingPosY;


            transform.position = newPos;
        }


    }
}
