/*
 * Resources:
 *  - https://docs.unity3d.com/Manual/MultipleCameras.html
 *  - https://docs.unity3d.com/ScriptReference/Camera-targetDisplay.html
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    // the starting camera.
    // capped to be in [1, 8]
    public int startingCam = 1;

    // list of cameras
    [Header("Cameras")]
    // the current camera (1 - 8)
    private Camera currentCam;

    // list of cameras
    public Camera Cam1 = null;
    public Camera Cam2 = null;
    public Camera Cam3 = null;
    public Camera Cam4 = null;
    public Camera Cam5 = null;
    public Camera Cam6 = null;
    public Camera Cam7 = null;
    public Camera Cam8 = null;

    // used to trigger the cameras.
    [Header("Trigger Keys")]
    // if 'true', the cameras are switched using in-script key commands.
    public bool useTriggerKeys = true;
    
    // keys
    public KeyCode Cam1Key = KeyCode.Keypad1;
    public KeyCode Cam2Key = KeyCode.Keypad2;
    public KeyCode Cam3Key = KeyCode.Keypad3;
    public KeyCode Cam4Key = KeyCode.Keypad4;
    public KeyCode Cam5Key = KeyCode.Keypad5;
    public KeyCode Cam6Key = KeyCode.Keypad6;
    public KeyCode Cam7Key = KeyCode.Keypad7;
    public KeyCode Cam8Key = KeyCode.Keypad8;

    // Start is called before the first frame update
    void Start()
    {
        startingCam = Mathf.Clamp(startingCam, 1, 8);

        // the set camera
        Camera setCam = null;

        // selects camera
        switch (startingCam)
        {
            default:
            case 1: // cam 1
                setCam = Cam1;
                break;

            case 2: // cam 2
                setCam = Cam2;
                break;

            case 3: // cam 3
                setCam = Cam3;
                break;

            case 4: // cam 4
                setCam = Cam4;
                break;

            case 5: // cam 5
                setCam = Cam5;
                break;

            case 6: // cam 6
                setCam = Cam6;
                break;

            case 7: // cam 7
                setCam = Cam7;
                break;

            case 8: // cam 8
                setCam = Cam8;
                break;

        }

        // the requested camera does not exist.
        if (setCam == null)
        {
            Debug.LogError("Requested camera does not exist. Attempting to set to another camera.");
            
            // checks each camera to see if they're viable.
            if (Cam1 != null)
                setCam = Cam1;
            else if (Cam2 != null)
                setCam = Cam2;
            else if (Cam3 != null)
                setCam = Cam3;
            else if (Cam4 != null)
                setCam = Cam4;
            else if (Cam5 != null)
                setCam = Cam5;
            else if (Cam6 != null)
                setCam = Cam6;
            else if (Cam7 != null)
                setCam = Cam7;
            else if (Cam8 != null)
                setCam = Cam8;

        }

        // sets current camera
        if (setCam != null)
            currentCam = setCam;
        else
            Debug.LogError("No cameras exist in the list.");
            
    }

    // changes the current camera.
    private void SetCurrentCamera(Camera cam)
    {
        // null refernece sent.
        if(cam == null)
        {
            Debug.LogError("Camera does not exist.");
            return;
        }

        currentCam.enabled = false;
        cam.enabled = true;
        currentCam = cam;
    }

    // enables camera 1
    public void EnableCamera1()
    {
        SetCurrentCamera(Cam1);
    }

    // enables camera 2
    public void EnableCamera2()
    {
        SetCurrentCamera(Cam2);
    }

    // enables camera 3
    public void EnableCamera3()
    {
        SetCurrentCamera(Cam3);
    }

    // enables camera 4
    public void EnableCamera4()
    {
        SetCurrentCamera(Cam4);
    }

    // enables camera 5
    public void EnableCamera5()
    {
        SetCurrentCamera(Cam5);
    }

    // enables camera 6
    public void EnableCamera6()
    {
        SetCurrentCamera(Cam6);
    }

    // enables camera 7
    public void EnableCamera7()
    {
        SetCurrentCamera(Cam7);
    }

    // enables camera 8
    public void EnableCamera8()
    {
        SetCurrentCamera(Cam8);
    }

    // Update is called once per frame
    void Update()
    {
        // the trigger keys are not used.
        if (!useTriggerKeys)
            return;

        // switching to different cameras
        if (Input.GetKeyDown(Cam1Key) && Cam1 != null) // switch to camera 1
        {
            SetCurrentCamera(Cam1);
        }
        else if (Input.GetKeyDown(Cam2Key) && Cam2 != null) // switch to camera 2
        {
            SetCurrentCamera(Cam2);
        }
        else if (Input.GetKeyDown(Cam3Key) && Cam3 != null) // switch to camera 3
        {
            SetCurrentCamera(Cam3);
        }
        else if (Input.GetKeyDown(Cam4Key) && Cam4 != null) // switch to camera 4
        {
            SetCurrentCamera(Cam4);
        }
        else if (Input.GetKeyDown(Cam5Key) && Cam5 != null) // switch to camera 5
        {
            SetCurrentCamera(Cam5);
        }
        else if (Input.GetKeyDown(Cam6Key) && Cam6 != null) // switch to camera 6
        {
            SetCurrentCamera(Cam6);
        }
        else if (Input.GetKeyDown(Cam7Key) && Cam7 != null) // switch to camera 7
        {
            SetCurrentCamera(Cam7);
        }
        else if (Input.GetKeyDown(Cam8Key) && Cam8 != null) // switch to camera 8
        {
            SetCurrentCamera(Cam8);
        }

    }
}
