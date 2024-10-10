using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    public DropBox dropBox;
    int score = 0;
    int lastScore = 0;
    int finalScore;
    public float waitTime;

    IList<Transform> digits = new List<Transform>();
    public Sprite[] sprites = new Sprite[10];
    public Sprite emptyDigit;
    private void Awake()
    {
        digits = GetComponentsInChildren<Transform>().Skip(1).ToList();
    }

    private void Start()
    {
        GameController.Setup += SetupScore;
    }

    void Update()
    {
        //Gets totalScore from dropBox
        score = dropBox.totalScore;
        //Checks total score against the last saved score, if not equal, updates scoreboard sprites
        if (score != lastScore ) 
        {
            IntToSprite();
        }
        //updates lastScore to the new score
        lastScore = score; 

        if (Input.GetKeyDown("t") && score != 0 )
        {
            StartCoroutine(FlashAndReset());
        }
    }

    private void IntToSprite()
    {
        string scoreString = score.ToString();

        for (int i = 0; i < digits.Count; i++)
        {
            //If the string is shorter than the expected length, adds a 0 to the left
            string tempString = scoreString.PadLeft(digits.Count, '0');
            //Gets sprite renderer from the indexed child 
            SpriteRenderer spriteRenderer = transform.GetChild(i).GetComponent<SpriteRenderer>();

            //Takes passed string and indexes into it, returning the integer at that index
            //Indexing into a string returns the character as unicode(ASCII)
            //By subtracting the ASCII number for 0(48) from any int 0-9, the integer will be returned as an int
            int dig = tempString[i] - '0';

            Sprite newSprite = sprites[dig];
            spriteRenderer.sprite = newSprite;
            
            
            
        }
    }

    IEnumerator FlashAndReset()
    {
        EmptyScore();        
        yield return new WaitForSeconds(waitTime);
        FullScore();
        yield return new WaitForSeconds(waitTime);
        EmptyScore();
        yield return new WaitForSeconds(waitTime);
        FullScore();
        yield return new WaitForSeconds(waitTime);
        EmptyScore();
        yield return new WaitForSeconds(waitTime);
        ZeroScore();
        yield return new WaitForSeconds(waitTime);
        EmptyScore();
        yield return new WaitForSeconds(waitTime);
        ZeroScore();
        yield return new WaitForSeconds(waitTime);
        EmptyScore();
        yield return new WaitForSeconds(waitTime);

    }

    private void EmptyScore()
    {
        foreach (Transform t in digits)
        {
            SpriteRenderer spriteRenderer = t.GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = emptyDigit;
        }
    }

    private void FullScore()
    {
        IntToSprite();
    }

    private void ZeroScore()
    {
        score = 0;
        IntToSprite();
    }

    private void SetupScore()
    {
        score = 0;
        IntToSprite();
        GameController.Setup -= SetupScore;
    }
}
