using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

// post processing effects for water
public class WaterPostProcessing : MonoBehaviour
{
    // the volume being used.
    public PostProcessVolume volume;

    // destorts the view when in water.
    [Header("View Distortion")]

    // the lens distortion effect.
    public LensDistortion lensDistort;

    // runs operations only when the volume is enabled.
    public bool onlyWhenVolumeEnabled = true;

    // applies the swaying effect in the water.
    public bool applyDistort = true;

    // mechanics for the view distrotion.
    [Header("View Distortion/Mechanics")]

    // the speed of the sway
    public Vector2 distortSpeed = Vector2.one;

    // the minimum values for the x and y axes
    public Vector2 xyMultMin = new Vector2(0.0F, 0.0F);

    // the maximum values for the x and y axes.
    public Vector2 xyMultMax = new Vector2(1.0F, 1.0F);

    // the t values for x and y.
    private Vector2 xyT = Vector2.zero;

    // the direction the x and y is going (increase or decrease)
    public Vector2 xyDirec = new Vector2(1.0F, -1.0F);

    // Start is called before the first frame update
    void Start()
    {
        // tries to find volume component.
        if (volume == null)
            volume = GetComponent<PostProcessVolume>();
       
        // if lens distortion not set.
        if(lensDistort == null && volume != null)
        {
            // gets the lens distortion effect.
            LensDistortion temp;
            if (volume.sharedProfile.TryGetSettings<LensDistortion>(out temp))
                lensDistort = temp;
        }

        // lens distortion effect supplied.
        if(lensDistort != null)
        {
            xyT.x = Mathf.Clamp01(Mathf.InverseLerp(xyMultMin.x, xyMultMax.x, lensDistort.intensityX.value));
            xyT.y = Mathf.Clamp01(Mathf.InverseLerp(xyMultMin.y, xyMultMax.y, lensDistort.intensityY.value));
        }
    }

    // Update is called once per frame
    void Update()
    {
        // volume not enabled, so don't do calculations.
        if (onlyWhenVolumeEnabled && !volume.enabled)
            return;

        // if the distortion effect shoudl be applied.
        if(applyDistort)
        {
            // add delta time with modifiers
            xyT.x += Time.deltaTime * distortSpeed.x * xyDirec.x;
            xyT.y += Time.deltaTime * distortSpeed.y * xyDirec.y;

            // clamp values
            xyT.x = Mathf.Clamp01(xyT.x);
            xyT.y = Mathf.Clamp01(xyT.y);

            // sets the lens distrotion values.
            lensDistort.intensityX.value = Mathf.Lerp(xyMultMin.x, xyMultMax.x, xyT.x);
            lensDistort.intensityY.value = Mathf.Lerp(xyMultMin.y, xyMultMax.y, xyT.y);

            // changing direction
            // direction change on the x.
            if (xyT.x <= 0.0F || xyT.x >= 1.0F)
                xyDirec.x *= -1;

            // direction change on the y.
            if (xyT.y <= 0.0F || xyT.y >= 1.0F)
                xyDirec.y *= -1;

        }

        
    }
}
