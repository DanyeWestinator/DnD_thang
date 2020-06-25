using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class readableObject : MonoBehaviour
{
    public Text speechText;
    public Image speechBubbleImage;
    public Text prompt;

    private bool fadeIn = false;

    public float fadeTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        speechBubbleImage.color = new Color(speechBubbleImage.color.r, speechBubbleImage.color.g, speechBubbleImage.color.b, 0f);
        speechText.color = new Color(0f, 0f, 0f, 0f);
        prompt.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Interact") != 0 && prompt.gameObject.activeInHierarchy)
        {
            prompt.gameObject.SetActive(false);
            fadeIn = true;
        }
        if (fadeIn == false)
        {
            speechBubbleImage.color = new Color(speechBubbleImage.color.r, speechBubbleImage.color.g, speechBubbleImage.color.b, speechBubbleImage.color.a - Time.deltaTime / fadeTime);
            speechText.color = new Color(0f, 0f, 0f, speechText.color.a - Time.deltaTime / fadeTime);
        }
        if (fadeIn == true)
        {
            speechBubbleImage.color = new Color(speechBubbleImage.color.r, speechBubbleImage.color.g, speechBubbleImage.color.b, 1f);
            speechText.color = new Color(0f, 0f, 0f, 1f);
        }
        if (speechBubbleImage.color.a < 0)
        {
            speechBubbleImage.color = new Color(speechBubbleImage.color.r, speechBubbleImage.color.g, speechBubbleImage.color.b, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            prompt.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            fadeIn = false;
            prompt.gameObject.SetActive(false);
        }
    }
}
