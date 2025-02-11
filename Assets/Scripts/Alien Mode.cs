using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienMode : MonoBehaviour
{
    public float alienAGoGo = 0;
    private float alienMax = 100; 
    public float alienGrowin;
    public float alienShrinkage;
    
    public AlienModeEffects alienEffect;
    public DropBox dropBox;
    public float ufoBonus;

    public GameTimer gameTimer;

    public GameObject alienGoo;
    private float alienGooHome;

    //Audio sources for switching between main and alien tracks
    private AudioSource mainSource;
    public AudioSource alienSource;

    //variables for storing volume values during audio transition
    float mainVolume, alienVolume, storedVolume;
    public float deltaVolume;


    public State state = State.stalled;
    public enum State
    {
        empty,
        stalled,
        running,
        full,
        resetting

    }

    private void Awake()
    {
        alienGooHome = alienGoo.transform.position.y;

        //get the main audio source
        GameMusic music = FindAnyObjectByType<GameMusic>();
        mainSource = music.GetComponent<AudioSource>();
    }

    private void Start()
    {
        GameController.Setup += SetupAlien;
    }
    void Update()
    {
        ufoBonus = dropBox.alienValue;

        if (gameTimer.currentState == GameTimer.State.running && (state == State.stalled || state == State.empty))
        {
            state = State.running;
        }
        if (gameTimer.currentState != GameTimer.State.running)
        {
            state = State.stalled;
        }


        if (state == State.running)
        {
            alienGoo.transform.Translate(Vector3.up * alienGrowin * Time.deltaTime);
            alienAGoGo += Time.deltaTime;
        }

        if (ufoBonus > 0 && state != State.full)
        {
            Debug.Log("ufoBonus = " + ufoBonus);
            alienAGoGo += ufoBonus;
            alienGoo.transform.Translate(Vector3.up * alienGrowin * ufoBonus);
            ufoBonus = 0;
            dropBox.alienValue = 0;
        }

        
        if (alienAGoGo >= alienMax)
        {
            Debug.Log("alien A GO TO 100!!!");
            state = State.full;
            // call alienmode effect\
            alienEffect.startAlienMode = true;
            // time needs to go faster
            Time.timeScale = 1.5f;
            // start minusing alien goo
            StartCoroutine(ResetGoo());
            // still have ufo add to alien goo

            //changes tracks for alien mode 
            ChangeTrack();

        }



    }

    IEnumerator ResetGoo()
    {
        while (alienGoo.transform.position.y >= alienGooHome)
        {
            state = State.resetting;
            alienGoo.transform.Translate(Vector3.down * alienShrinkage * Time.deltaTime);
            alienAGoGo = 0;
            yield return null;
        }

        if (alienGoo.transform.position.y <= alienGooHome)
        {
            Time.timeScale = 1f;
            alienEffect.ExitAlienMode();
            state = State.empty;
        }

    }

    void SetupAlien() 
    {
        ResetGoo();
        GameController.Setup -= SetupAlien;
    }

    //function to switch between alien track and normal track
    void ChangeTrack()
    {
        AudioSource sourceA = null;
        AudioSource sourceB = null;
        if (alienVolume > mainVolume)
        {
            sourceA = alienSource; sourceB = mainSource;
        }
        else
        {
            sourceA = mainSource; sourceB = mainSource;
        }

        storedVolume = sourceA.volume;
        Debug.Log($"AlienMode ChangeTrack() called| sourceA = {sourceA}| sourceB = {sourceB}");
        StartCoroutine(FadeSwap(sourceA, sourceB));
    }

    //coroutine to smooth the track transition
    private IEnumerator FadeSwap(AudioSource sourceA, AudioSource sourceB)
    {

        Debug.Log($"AlienMode FadeSwap() called");
        while (sourceA.volume > 0)
        {
            sourceA.volume -= deltaVolume; sourceB.volume += deltaVolume;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        if (sourceA.volume <= 0)
        {
            sourceB.volume = storedVolume; sourceA.volume = 0;
            yield return null;
        }
    }
}
