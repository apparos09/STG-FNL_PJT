using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


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
    public Vector2 moveSpeedXZ = new Vector2(35.0F, 35.0F);

    // maximum speed.
    public float maxMoveSpeed = 45.0F;

    // if the move speed should be limited.
    public bool limitMoveSpeed = true;

    // the turn speed for the player.
    public Vector2 turnSpeedXY = new Vector2(90.0F, 90.0F);

    // the force for jumping.
    public float jumpForce = 400.0F;

    // becomes 'true' if the player is in water.
    public bool inWater = false;

    // changes mass if leaving water.
    public float InAirDrag = 0.0F;
    public float InWaterDrag = 1.5F;

    // becomes 'true' if the player hits the ground.
    public bool onGround = false;


    // [Header("Post Processing")]
    // // if 'true', post processing is enabled.
    // public bool usePostProcessing = true;


    // post processing volume for being in the water.
    // public PostProcessVolume inWaterPostProcess;

    // Start is called before the first frame update
    void Start()
    {
        // grabs the rigidbody if it's not set.
        if (rigidBody == null)
            rigidBody = FindObjectOfType<Rigidbody>();

        InAirDrag = rigidBody.drag;
        InWaterDrag = (rigidBody.drag + 1) * InWaterDrag; // does a plus one to account for a drag of 0. 

        // if third person camera not set.
        if(thirdPersonCamera == null)
            thirdPersonCamera = Camera.main;

        // gets hte first person camera if not set.
        if (firstPersonCamera == null)
            firstPersonCamera = GetComponentInChildren<Camera>(true);

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


        // finds the post processing volume for the scene.
        // this should not be used if more than one volume is added.
        // if (inWaterPostProcess == null)
        //     inWaterPostProcess = FindObjectOfType<PostProcessVolume>(true);
        // 
        // // disables this post process.
        // if (inWaterPostProcess != null)
        //     inWaterPostProcess.enabled = false;

    }

    // checks for collision
    private void OnCollisionStay(Collision collision)
    {
        // checks if on the ground
        if(!onGround && (collision.gameObject.tag == "Stage" || collision.gameObject.tag == "Untagged"))
        {
            Ray downRay = new Ray(transform.position, -transform.up);
            RaycastHit hitInfo;

            // checks if standing on surface.
            bool rayHit = collision.collider.Raycast(downRay, out hitInfo, 1.0F);

            // if standing on the ground, then set this to true.
            onGround = rayHit;
        }
    }

    // on exiting a collider
    private void OnCollisionExit(Collision collision)
    {
        // chekcs if the player has left the ground
        if (onGround && (collision.gameObject.tag == "Stage" || collision.gameObject.tag == "Untagged"))
            onGround = false;
    }


    // enter water
    // private void OnTriggerEnter(Collider other)
    // {
    //     // if entered water
    //     if (other.gameObject.tag == "Water")
    //     {
    //         inWater = true;
    //         rigidBody.drag = InWaterDrag;
    // 
    //     }
    // }

    // entered water.
    private void OnTriggerStay(Collider other)
    {
        // if entered water; note that water is a plane, so the collider should be altered to allow for this.
        if (other.gameObject.tag == "Water")
        {
            inWater = true;
            rigidBody.drag = InWaterDrag;
            
            // enables the post processing effects.
            // if (inWaterPostProcess != null)
            //     inWaterPostProcess.enabled = true;
        }
    }

    // left water
    private void OnTriggerExit(Collider other)
    {
        // if left water
        if (other.gameObject.tag == "Water")
        {
            inWater = false;
            rigidBody.drag = InAirDrag;

            // disables the post processing effects.
            // if (inWaterPostProcess != null)
            //     inWaterPostProcess.enabled = false;
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
            rigidBody.AddForce(transform.forward * Input.GetAxisRaw("Movement Z") * moveSpeedXZ.y * Time.deltaTime, ForceMode.Impulse);

        // move left or right
        if (Input.GetAxisRaw("Movement X") != 0.0F)
            rigidBody.AddForce(transform.right * Input.GetAxisRaw("Movement X") * moveSpeedXZ.x * Time.deltaTime, ForceMode.Impulse);

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
            // if (Input.GetAxisRaw("Jump") != 0.0F)
            //     rigidBody.AddForce(transform.up * Input.GetAxisRaw("Jump") * jumpForce * Time.deltaTime, ForceMode.Impulse);

            if (Input.GetKeyDown("space"))
                rigidBody.AddForce(transform.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
        }
        else
        {
            if (Input.GetKeyDown("space") && onGround)
                rigidBody.AddForce(transform.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
        }


        // if hte movement should be limited.
        if (limitMoveSpeed)
        {
            // clamps the maximum move speed.
            if (Mathf.Abs(rigidBody.velocity.magnitude) > maxMoveSpeed)
                rigidBody.velocity = Vector3.ClampMagnitude(rigidBody.velocity, maxMoveSpeed);

            // gets the current speed.
            // float currSpeed = rigidBody.velocity.magnitude;
            // 
            // // if the maximum move speed has been passed.
            // if (Mathf.Abs(currSpeed) > maxMoveSpeed)
            //     rigidBody.velocity = rigidBody.velocity.normalized * maxMoveSpeed;
            // 
        }

        // switches the camera
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0) ||
            Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            SwitchCamera();
    }

}
