using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    // the player in the game.
    public Player player;

    // drag variables
    // changes mass if leaving water.
    // the air drag is the default for all objects.
    public float waterDragFactor = 1.95F;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = FindObjectOfType<Player>();
    }

    // applies the water drag to the rigidbody.
    public void ApplyWaterDrag(Rigidbody rb)
    {
        rb.drag = (rb.drag + 1) * waterDragFactor;
        Debug.Log("Drag: " + rb.drag);
    }

    // removes the water drag.
    public void RemoveWaterDrag(Rigidbody rb)
    {
        rb.drag = (rb.drag / waterDragFactor) - 1; // reverses the equation.
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
