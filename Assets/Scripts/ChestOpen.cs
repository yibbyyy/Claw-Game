using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestOpen : MonoBehaviour
{

    public Animation chestAnimation;
    public GameObject coinSystem;


    int chestSpin;
    int chestOpen;

    private void Awake()
    {

    }
    private void Start()
    {
        // Start the animation sequence

        

    }

    private void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            StartCoroutine(PlayAnimationsAndActivateParticles());
        }
    }
    private IEnumerator PlayAnimationsAndActivateParticles()
    {
        // Play the first animation
        chestAnimation.Play("Chest spin");

        // Wait for the first animation to finish
        yield return new WaitForSeconds(2);

        // Play the second animation
        chestAnimation.Play("Chest Open"); // You might want to set another trigger if needed
        yield return new WaitForSeconds(1);

        // Activate the particle system
        coinSystem.SetActive(true);
    }


}
