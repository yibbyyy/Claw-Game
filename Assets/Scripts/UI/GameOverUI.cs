using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public Scoreboard scoreboard;
    public float fadeDuration = .5f;
    public float GameOverDuration = 5.0f, scoreShownDuration = 4.0f;
    public Image GameOverImg, BorderImg;
    public RawImage BackGround;
    public TMP_Text scoreText, scoreNum, enterNameText;

    public GameObject[] letterGameObjects;
    
    AudioSource audioSource;
    private int score;
    private bool enterName = false;
    private void OnEnable()
    {
        score = scoreboard.score;
        StartCoroutine(GameOverSequence());
    }
    private IEnumerator GameOverSequence()
    {
        Debug.Log("Starting gameove sequence");
        // Fade in Gameover UI
        StartCoroutine(FadeRaw(0f, 1f, fadeDuration, BackGround));
        StartCoroutine(Fade(0f, 1f, fadeDuration, BorderImg));
        yield return StartCoroutine(Fade(0f, 1f, fadeDuration, GameOverImg));

        // Delay for GameOver Delay
        yield return StartCoroutine(Delay(GameOverDuration));
        // Fade Out Gameover
        yield return StartCoroutine(Fade(1f, 0f, fadeDuration, GameOverImg));

        // Fade in Current Score that's big, after delay, fade out
        StartCoroutine(FadeText(0f, 1f, fadeDuration, scoreText));
        yield return StartCoroutine(FadeText(0f, 1f, fadeDuration, scoreNum));
        // Count up score from 0-currScore
        yield return StartCoroutine(ScoreCountUp(0, score, scoreNum));
        yield return Delay(scoreShownDuration);
        // Fade both UI out
        StartCoroutine(FadeText(1f, 0f, fadeDuration, scoreText));
        yield return StartCoroutine(FadeText(1f, 0f, fadeDuration, scoreNum));

        // Fade in "Enter Name"
        yield return StartCoroutine(FadeText(0f, 1f, fadeDuration, enterNameText));
        // Take in 3 Letters, Uppercase and Lowercase
        // enter moves selected digit
        // if selected digit is the far right, register name
        enterName = true;
        yield return new WaitWhile(() =>  enterName);
        // Then fade out

        // Display their highscore and name among other entries with fade
        // With very short delay, fade in Play Again and Main Menu button

    }

    private void Start()
    {
        upArrowTransform = upArrow.GetComponent<RectTransform>();
        downArrowTransform = downArrow.GetComponent<RectTransform>();
        letterTexts = new TMP_Text[3];
        letterPosX = new float[3];
        for (int i = 0; i < 3; i++)
        {
            letterTexts[i] = letterGameObjects[i].GetComponent<TMP_Text>();
            // Works bc both arrows and letters have the same pivot point
            letterPosX[i] = letterGameObjects[i].GetComponent<RectTransform>().anchoredPosition.x;
        }
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        
        if (enterName)
            HandleInput();
        
    }

    // table for arrow positions

    char[] letters = new char[3] { 'A', 'A', 'A' };
    char lastChar = 'Z', firstChar = 'A';
    float[] letterPosX = new float[3];
    public int currentArrowPos = 0;
    public TMP_Text[] letterTexts;
    public GameObject upArrow, downArrow;
    public float arrowSpeed = 2;
    private RectTransform upArrowTransform, downArrowTransform;
    float horizontal = 0f, vertical = 0f, submit = 0f;
    float targetPosX;
    bool leftPressed = false, rightPressed = false, upPressed = false, downPressed = false;
    public AudioClip letterSwitchSFX, arrowMoveSFX, submitSFX;
    private void HandleInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        // Hitting enter:
        submit = Input.GetAxis("Submit");
        // When you move left or right it moves the arrows over and plays a sound effect

        // When you click enter it moves the letters to the right (use axisName "submit")
        if ((horizontal > 0f || submit != 0f) && !rightPressed)
        {
            // Move over to right:
            // change current arrow pos to next one
            rightPressed = true;
            currentArrowPos++;
            if (currentArrowPos >= 2)
            {
                currentArrowPos = 2;
                // submit name break out of function

            }
            // play sfx
            PlayAudioClip(arrowMoveSFX);
            // get the x position of this new arrow location
            targetPosX = letterPosX[currentArrowPos];
            // move both top and bottom arrows to get there
            //upArrowTransform.anchoredPosition = Vector2.Lerp(upArrowTransform.anchoredPosition, targetPos, Time.deltaTime * arrowSpeed);
            upArrowTransform.anchoredPosition = new Vector2(targetPosX, upArrowTransform.anchoredPosition.y);
            downArrowTransform.anchoredPosition = new Vector2(targetPosX, downArrowTransform.anchoredPosition.y);

        }
        else if(horizontal < 0f && !leftPressed)
        {
            // move to left
            leftPressed = true;
            currentArrowPos--;
            if (currentArrowPos < 0)
            {
                currentArrowPos = 0;
                return;
            }

            // play sfx
            PlayAudioClip(arrowMoveSFX);
            // get the x position of this new arrow location
            targetPosX = letterPosX[currentArrowPos];
            // move both top and bottom arrows to get there
            //upArrowTransform.anchoredPosition = Vector2.Lerp(upArrowTransform.anchoredPosition, targetPos, Time.deltaTime * arrowSpeed);
            upArrowTransform.anchoredPosition = new Vector2(targetPosX, upArrowTransform.anchoredPosition.y);
            downArrowTransform.anchoredPosition = new Vector2(targetPosX, downArrowTransform.anchoredPosition.y);
        }

        // reset button pressed vars if no horizontal input
        if (horizontal == 0f && submit == 0f)
        {
            rightPressed = false;
            leftPressed = false;
        }

        // When you move up or down, it recurses across the letters, clicks the arrow buttons and plays a sound effect
        if (vertical > 0f && !upPressed)
        {
            // move letter up
            upPressed = true;

            // play sfx
            PlayAudioClip(letterSwitchSFX);
            // Use Ascii to change to next letter in alphabet, if 'Z' go back to 'A'
            letters[currentArrowPos] = letters[currentArrowPos] == firstChar ? lastChar : (char)(letters[currentArrowPos] - 1);

            letterTexts[currentArrowPos].text = letters[currentArrowPos].ToString();
            
        }

        // When you move up or down, it recurses across the letters, clicks the arrow buttons and plays a sound effect
        else if (vertical < 0f && !downPressed)
        {
            // move letter down
            downPressed = true;
            // play sfx
            PlayAudioClip(letterSwitchSFX);
            // Use Ascii to change to next letter in alphabet, if 'Z' go back to 'A'
            letters[currentArrowPos] = letters[currentArrowPos] == lastChar ? firstChar : (char)(letters[currentArrowPos] + 1);
            letterTexts[currentArrowPos].text = letters[currentArrowPos].ToString();
        }

        if (vertical == 0f)
        {
            upPressed = false;
            downPressed = false;
        }
    }

    private void PlayAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }
    private IEnumerator Fade(float startAlpha, float endAlpha, float duration, Image image)
    {
        float elapsedTime = 0f;

        Color color = image.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            image.color = color;
            yield return null;
        }

        color.a = endAlpha;
        image.color = color;
    }

    private IEnumerator FadeRaw(float startAlpha, float endAlpha, float duration, RawImage image)
    {
        float elapsedTime = 0f;

        Color color = image.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            image.color = color;
            yield return null;
        }

        color.a = endAlpha;
        image.color = color;
    }
    private IEnumerator FadeText(float startAlpha, float endAlpha, float duration, TMP_Text text)
    {
        float elapsedTime = 0f;

        Color color = text.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / fadeDuration);
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            text.color = color;
            yield return null;
        }

        color.a = endAlpha;
        text.color = color;
    }

    private IEnumerator ScoreCountUp(int startNum, int endNum, TMP_Text score)
    {
        int[] increments = { 1000000000, 100000000, 10000000, 1000000, 10000, 1000, 100, 10, 1 };
        foreach(int i in increments)
        {
            while(startNum + i < endNum)
            {
                startNum += i;
                score.text = startNum.ToString();
                yield return null;
            }
        }
    }
    private IEnumerator Delay(float dur)
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }
}
