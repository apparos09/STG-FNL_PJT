using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the player, which is the controlled by the user of the sim.
public class Player : MonoBehaviour
{
    // the player's rigidbody.
    public Rigidbody rigidBody;

    // view
    [Header("View")]

    // the first person camera
    public Camera firstPersonCamera;

    // the third person camera
    public Camera thirdPersonCamera;

    // if 'true', the game starts in first person.
    public bool startInFirstPerson = false;

    // the movement for the player.
    [Header("Movement")]

    // movement force
    public Vector2 moveSpeedXZ = new Vector2(1200.0F, 1200.0F);

    // maximum speed.
    public float maxMoveSpeed = 250.0F;

    // if the move speed should be limited.
    public bool limitMoveSpeed = true;

    // the turn speed for the player.
    public Vector2 turnSpeedXY = new Vector2(90.0F, 90.0F);

    // the force for jumping.
    public float jumpForce = 500.0F;

    // becomes 'true' if the player is in water.
    public bool inWater = false;

    // Start is called before the first frame update
    void Start()
    {
        // grabs the rigidbody if it's not set.
        if (rigidBody == null)
            rigidBody = FindObjectOfType<Rigidbody>();

        // if third person camera not set.
        if(thirdPersonCamera == null)
            thirdPersonCamera = Camera.main;

        // gets hte first person camera if not set.
        if (firstPersonCamera == null)
            firstPersonCamera = GetComponentInChildren<Camera>();

        // checks what camera should be enabled.
        if(startInFirstPerson)
        {
            thirdPersonCamera.enabled = false;
            firstPersonCamera.enabled = true;
        }
        else
        {
            thirdPersonCamera.enabled = true;
            firstPersonCamera.enabled = false;
        }


    }

    // if the player is in first person mode.
    public bool IsInFirstPersonMode()
    {
        return firstPersonCamera.enabled;
    }

    // if the player is in the third person mode.
    public bool IsInThirdPersonMode()
    {
        return thirdPersonCamera.enabled;
    }

    // if the view mode should be switched.
    public void SwitchCamera()
    {
        // switch camera
        if(thirdPersonCamera.enabled) // in third person mode.
        {
            thirdPersonCamera.enabled = false;
            firstPersonCamera.enabled = true;
        }
        else if(!thirdPersonCamera.enabled) // in first person mode.
        {
            thirdPersonCamera.enabled = true;
            firstPersonCamera.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // MOVEMENT //
        // move forward or backwards
        if (Input.GetAxisRaw("Movement Z") != 0.0F)
            rigidBody.AddForce(transform.forward * Input.GetAxisRaw("Movement Z") * moveSpeedXZ.y * Time.deltaTime);

        // move left or right
        if (Input.GetAxisRaw("Movement X") != 0.0F)
            rigidBody.AddForce(transform.right * Input.GetAxisRaw("Movement X") * moveSpeedXZ.x * Time.deltaTime);

        // ROTATION //
        // rotation/movement force (left and right)
        if (Input.GetAxisRaw("Rotation Y") != 0.0F)
        {
            transform.Rotate(transform.up, Input.GetAxisRaw("Rotation Y") * turnSpeedXY.y * Time.deltaTime);
        }

        // TODO: rotate camera, not whole body.
        // rotation/movement force (look up and down)
        // if (Input.GetAxisRaw("Rotation X") != 0.0F)
        // {
        //     transform.Rotate(transform.right, Input.GetAxisRaw("Rotation X") * turnSpeedXY.x * Time.deltaTime);
        // }

        // JUMP //
        // TODO: only have this work underwater
        if(inWater)
        {
            if (Input.GetAxisRaw("Jump") != 0.0F)
                rigidBody.AddForce(transform.up * Input.GetAxisRaw("Jump") * jumpForce * Time.deltaTime, ForceMode.Impulse);
        }
        else
        {
            if (Input.GetKeyDown("space"))
                rigidBody.AddForce(transform.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
        }


        // if hte movement should be limited.
        if (limitMoveSpeed)
        {
            // gets the current speed.
            float currSpeed = rigidBody.velocity.magnitude;

            // if the maximum move speed has been passed.
            if (Mathf.Abs(currSpeed) > maxMoveSpeed)
                rigidBody.velocity = rigidBody.velocity.normalized * maxMoveSpeed;
        }

        // switches the camera
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0) ||
            Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            SwitchCamera();
    }
}
