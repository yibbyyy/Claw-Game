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
    public Image GameOverImg, BackGround, BorderImg;
    public TMP_Text scoreText, scoreNum;
    private int score;
    private void OnEnable()
    {
        score = scoreboard.score;
        StartCoroutine(GameOverSequence());
    }
    private IEnumerator GameOverSequence()
    {
        Debug.Log("Starting gameove sequence");
        // Fade in Gameover UI
        StartCoroutine(Fade(0f, 1f, fadeDuration, BackGround));
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

        // Take in 3 Letters, Uppercase and Lowercase
        // enter moves selected digit
        // if selected digit is the far right, register name
        // Then fade out

        // Display their highscore and name among other entries with fade
        // With very short delay, fade in Play Again and Main Menu button

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
