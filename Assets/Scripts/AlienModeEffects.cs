using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;


public class AlienModeEffects : MonoBehaviour
{
    public float minHue, maxHue, duration;
    private bool increasing = true;
    public bool alienMode = false;
    public bool startAlienMode = false;
    public PostProcessVolume Volume;
    private ColorGrading grading;
    private AmbientOcclusion occlusion;
    private float t = 0f;
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
            grading.hueShift.value = Mathf.Lerp(minHue, maxHue, t);
            if (increasing)
            {
                t += Time.deltaTime / duration;
                if (t >= 1f)
                {
                    t = 1f;
                    increasing = false;
                }
            }
            else
            {
                t -= Time.deltaTime / duration;
                if (t <= 0f)
                {
                    t = 0f;
                    increasing = true;
                }
            }
           
        }

       
    }

    public void ExitAlienMode()
    {
        grading.enabled.value = false;
        occlusion.enabled.value = false;

        alienMode = false;
        startAlienMode = false;
    } 
}
