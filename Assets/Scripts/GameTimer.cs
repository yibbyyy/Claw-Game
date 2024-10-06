using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameTimer : MonoBehaviour
{
    public GameObject timerDisplay;
    public int timerLengthInSec;

    public Object[] sprites;
    IList<Transform> places = new List<Transform>();

    private enum State
    {
        started,
        paused,
        stopped
    }


    private void Awake()
    {
        sprites = Resources.LoadAll("Digits", typeof(Texture2D));
        places = timerDisplay.GetComponentsInChildren<Transform>();
        
        foreach(var sprite in sprites)
        {
            Debug.Log("sprite = " + sprite.name);
        }

    }
    private void Start()
    {
        ClawManager.StartClawTimer += StartTimerCoroutine;
    }

    void Update()
    {
        
    }

    public void StartTimerCoroutine()
    {
        Debug.Log("started coroutine");
        StartCoroutine(Timer());
    }

    public void PauseTimer()
    {
        StopCoroutine(Timer());
    }


    IEnumerator Timer()
    {
        while (timerLengthInSec < 0)
        {
            ConvertTime();
            yield return new WaitForSeconds(1);
            timerLengthInSec -= 1;
            
        }

    }


    private void ConvertTime()
    {
        string timerCountString = timerLengthInSec.ToString();
        for (int i = 0; i < places.Count; i++)
        {
            int dig = timerCountString[i];
            Transform transform = places[i].transform;
            Object sprite = sprites[dig];
            Debug.Log("dig = " + dig + " | transform = " + transform + " | sprite = " + sprite);
            Instantiate(sprite, transform);
        }
    }
}
