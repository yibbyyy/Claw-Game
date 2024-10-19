using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using static UnityEditor.PlayerSettings;

public class AlienModeEffects : MonoBehaviour
{
    public float hueRange;
    public float hueShiftSpeed;
    public bool alienMode = false;
    public bool startAlienMode = false;
    public PostProcessVolume Volume;
    private ColorGrading grading;
    private AmbientOcclusion occlusion;
    // Start is called before the first frame update
    void Start()
    {
        Volume = GetComponent<PostProcessVolume>();
        PostProcessProfile profile = Volume.profile;
        grading = profile.GetSetting<ColorGrading>();
        occlusion = profile.GetSetting<AmbientOcclusion>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            startAlienMode = true;
        }
        if (startAlienMode)
        {
            // set effects to true
            grading.enabled.value = true;
            occlusion.enabled.value = true;

            alienMode = true;
            startAlienMode = false;
        }

        if (alienMode)
        {
            
            // lerp between min and max hue
            // Use Mathf.PingPong to alternate hue shift between hueMin and hueMax over the specified duration
            float hueShiftValue = Mathf.Sin(Time.time * hueShiftSpeed) * hueRange;

            // Set the hue shift value in the Color Grading effect
            grading.hueShift.value = hueShiftValue;
        }
    }
}
