using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Explosion : MonoBehaviour
{
    public Sprite Sprite2;
    public Sprite ogSprite;
    public float lenOfBomb1 = 0.2f;
    public float lenOfBomb2 = 0.2f;
    // Start is called before the first frame update
    void OnEnable()
    {
        StartCoroutine(startExplosion(lenOfBomb1));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator startExplosion(float duration1)
    {
        yield return new WaitForSeconds(duration1);
        // Make explosion part 2
        ogSprite = GetComponent<Image>().sprite;
        GetComponent<Image>().sprite = Sprite2;
        yield return new WaitForSeconds(lenOfBomb2);

        GetComponent<Image>().sprite = ogSprite;
    }

    public void Dispose()
    {

    }
}
