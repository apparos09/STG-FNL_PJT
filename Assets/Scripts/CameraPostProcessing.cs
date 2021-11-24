using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// post processing functions for the camera.
// this is used to enable and disable the post processing so that it's more accurate to the player's view.
public class CameraPostProcessing : MonoBehaviour
{
    // post processing volume for being in the water.
    public PostProcessVolume inWaterPostProcess;

    // Start is called before the first frame update
    void Start()
    {
        // finds the post processing volume for the scene.
        // this should not be used if more than one volume is added.
        if (inWaterPostProcess == null)
            inWaterPostProcess = FindObjectOfType<PostProcessVolume>(true);

        // disables this post process.
        if (inWaterPostProcess != null)
            inWaterPostProcess.enabled = false;
    }

    // camera has entered space.
    private void OnTriggerStay(Collider other)
    {
        // if looking through water, turn on the water shader.
        if (other.gameObject.tag == "Water")
        {
            inWaterPostProcess.enabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // if left water
        if (other.gameObject.tag == "Water")
        {
            inWaterPostProcess.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
