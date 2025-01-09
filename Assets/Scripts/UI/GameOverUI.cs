using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameOverUI : MonoBehaviour
{
    [System.Serializable]
    public struct HighScoreEntry : IComparable<HighScoreEntry>
    {
        public string Name;
        public int Score; 


        // Constructor
        public HighScoreEntry(string name, int score)
        {
            this.Name = name;
            this.Score = score;
        }

        // Implement CompareTo for IComparable
        public int CompareTo(HighScoreEntry other)
        {
            // Sort in descending order (highest score first)
            return other.Score.CompareTo(this.Score);
        }
    }

    [SerializeField] public List<HighScoreEntry> StarterHighScores = new();
    public Scoreboard scoreboard;
    public float fadeDuration = .5f;
    public float GameOverDuration = 5.0f, scoreShownDuration = 4.0f;
    public Image GameOverImg, BorderImg;
    public RawImage BackGround;
    public TMP_Text scoreText, scoreNum, enterNameText;
    public GameObject enterNameGameObject;
    public GameObject[] letterGameObjects;

    public TMP_Text DebugName;
    AudioSource audioSource;
    private int score;
    private bool enterName = false;
    public CanvasGroup HighScoreParent;
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

        // Fade in "Enter Name" and all children UI
        yield return StartCoroutine(FadeImagesAndTextInParent(enterNameGameObject, 0f, 1f, fadeDuration));
        // Take in 3 Letters, Uppercase and Lowercase
        // enter moves selected digit
        // if selected digit is the far right, register name
        enterName = true;
        yield return new WaitWhile(() =>  enterName);
        // Then fade out
        yield return StartCoroutine(FadeImagesAndTextInParent(enterNameGameObject, 1f, 0f, fadeDuration));

        // Make HighScore entry and save it to list
        HighScoreEntry playerEntry = new HighScoreEntry(submittedName, score);
        AddHighScore(playerEntry, highScores);
        DisplayScores();


        // Display their highscore and name among other entries with fade
        LeanTweenFadeGroup(0f, 1f, HighScoreParent);
        
        // With very short delay, fade in Play Again and Main Menu button
        
    }

    void LeanTweenFadeGroup(float start, float end, CanvasGroup parent)
    {
        if (parent == null)
        {
            Debug.LogError("CanvasGroup is not assigned!");
            return;
        }

        
        parent.alpha = start;

        // Fade in the entire CanvasGroup (parent and children)
        LeanTween.value(gameObject, start, end, 5f)
            .setOnUpdate((float alpha) => parent.alpha = alpha)
            .setEase(LeanTweenType.easeInOutQuad); // Optional easing
    }
    private void Start()
    {
        enterNameText = enterNameGameObject.GetComponent<TMP_Text>();
        upArrowTransform = upArrow.GetComponent<RectTransform>();
        downArrowTransform = downArrow.GetComponent<RectTransform>();
        downArrowImg = downArrow.GetComponent<Image>();
        upArrowImg = upArrow.GetComponent<Image>();
        letterTexts = new TMP_Text[3];
        letterPosX = new float[3];
        for (int i = 0; i < 3; i++)
        {
            letterTexts[i] = letterGameObjects[i].GetComponent<TMP_Text>();
            // Works bc both arrows and letters have the same pivot point
            letterPosX[i] = letterGameObjects[i].GetComponent<RectTransform>().anchoredPosition.x;
        }
        audioSource = GetComponent<AudioSource>();

        // Load HighScores from Player Prefs
        LoadHighScores();

        // Merge and order HighScores hardcoded
        highScores = OrderHighScores(savedScores, StarterHighScores);
    }

    private void Update()
    {
        
        if (enterName)
            HandleInput();
        
    }

    // High Score Stuff
    private const string HighScoresKey = "HighScores";
    public List<HighScoreEntry> savedScores = new List<HighScoreEntry>();
    public List<HighScoreEntry> highScores= new List<HighScoreEntry>();


    void AddHighScore(HighScoreEntry hScore, List<HighScoreEntry> hList)
    {
        // Run a fucking bin search
        int idx = hList.BinarySearch(hScore);

        // Searches for score of highscore provided
        // If same score is found, the idx becomes that index
        // If not found, it finds the index where it should go and make it negative and add 1
        // So we use the bitwise NOT operator to flip all the bits which makes it positive and -1 lol
        if (idx < 0) idx = ~idx;

        hList.Insert(idx, hScore);
    }
    public GameObject highScoreEntryPrefab;
    public Transform highScoresParent;
    void DisplayScores()
    {
        // Clear children
        foreach (Transform child in highScoresParent)
        {
            Destroy(child.gameObject);
        }

        // populate parent
        foreach (HighScoreEntry hScore in highScores)
        {
            GameObject entry = Instantiate(highScoreEntryPrefab, highScoresParent);
            entry.transform.Find("Name").GetComponent<TMP_Text>().text = hScore.Name;
            entry.transform.Find("Score").GetComponent<TMP_Text>().text = hScore.Score.ToString();
        }
    }
    void LoadHighScores()
    {
        if (PlayerPrefs.HasKey(HighScoresKey))
        {
            string json = PlayerPrefs.GetString(HighScoresKey);
            savedScores = JsonUtility.FromJson<List<HighScoreEntry>>(json);
        }
        else
        {
            Debug.Log("No HighScores Saved");
        }
    }

    List<HighScoreEntry> OrderHighScores(List<HighScoreEntry> lstA, List<HighScoreEntry> lstB)
    {
        List<HighScoreEntry> res = new List<HighScoreEntry>(lstA);
        res.AddRange(lstB);
        res.Sort();
        return res;
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
    private Image upArrowImg, downArrowImg;
    private Color activatedButtonColor = new Color(0.75f, 0.75f, 0.75f);
    private Color ogButtonColor = Color.white;
    private Coroutine buttonTransitionCoroutine = null;
    float horizontal = 0f, vertical = 0f, submit = 0f;
    float targetPosX;
    bool leftPressed = false, rightPressed = false, upPressed = false, downPressed = false;
    public AudioClip letterSwitchSFX, arrowMoveSFX, submitSFX;

    public string submittedName;
    private void HandleInput()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        // Hitting enter:
        submit = Input.GetAxis("Submit");
        // When you move left or right it moves the arrows over and plays a sound effect

        
        if ((horizontal > 0f) && !rightPressed)
        {
            // Move over to right:
            // change current arrow pos to next one
            rightPressed = true;
            currentArrowPos++;
            
           
            if (currentArrowPos >= 2)
            {
                currentArrowPos = 2;

               

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
        if (horizontal == 0f)
        {
            rightPressed = false;
            leftPressed = false;
        }

       

        // When you move up or down, it recurses across the letters, clicks the arrow buttons and plays a sound effect
        if (vertical > 0f && !upPressed)
        {
            // move letter up
            upPressed = true;

            // Play button clicked animation
            StartButtonColorTransition(upArrowImg);
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

            // Play button clicked animation
            StartButtonColorTransition(downArrowImg);
            // Use Ascii to change to next letter in alphabet, if 'Z' go back to 'A'
            letters[currentArrowPos] = letters[currentArrowPos] == lastChar ? firstChar : (char)(letters[currentArrowPos] + 1);
            letterTexts[currentArrowPos].text = letters[currentArrowPos].ToString();
        }

        if (vertical == 0f)
        {
            upPressed = false;
            downPressed = false;
        }

        if (submit != 0f)
        {
            // save name
            for (int i = 0; i < letters.Length; i++)
            {
                submittedName += letters[i];
            }
            // submit name break out of function
            enterName = false;
        }
    }

    private void SetText(TMP_Text text, string message)
    {
        text.text = message;
    }

    private void StartButtonColorTransition(Image buttonImage)
    {

        // Stop already existing coroutine
        if (buttonTransitionCoroutine != null)
        {
            StopCoroutine(buttonTransitionCoroutine);
        }

        buttonTransitionCoroutine = StartCoroutine(PlayButtonClickAnimation(ogButtonColor, activatedButtonColor, buttonImage));
    }
    // May need to add a duration param if needed to extend for other funcs
    private IEnumerator PlayButtonClickAnimation(Color ogColor, Color clickedColor, Image buttonImage)
    {
        float elapsedTime = 0f;
        float fadeDuration = 0.1f;
        // Greying of button
        while (elapsedTime < fadeDuration)
        {
            buttonImage.color = Color.Lerp(ogColor, clickedColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // ensure it changed
        buttonImage.color = clickedColor;

        // Transition back to og color
        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            buttonImage.color = Color.Lerp(clickedColor, ogColor, elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // ensure it changed
        buttonImage.color = ogColor;
    }
    private void PlayAudioClip(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    //--------------------------------------------------------------FADES--------------------------------------------------------
    private IEnumerator FadeImagesAndTextInParent(GameObject parentObject, float startAlpha, float endAlpha, float duration)
    {
        // Get all Image components in children of the parentObject
        Image[] images = parentObject.GetComponentsInChildren<Image>();
        TMP_Text[] texts = parentObject.GetComponentsInChildren<TMP_Text>();

        if (images.Length == 0 && texts.Length == 0) yield break; // Exit if there are no images or texts

        for (int i = 0; i < images.Length; i++)
        {
            StartCoroutine(Fade(startAlpha, endAlpha, duration, images[i]));
        }


        for (int i = 0; i < texts.Length; i++)
        {
            StartCoroutine(FadeText(startAlpha, endAlpha, duration, texts[i]));
        }

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
