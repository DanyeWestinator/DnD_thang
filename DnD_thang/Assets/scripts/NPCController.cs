using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CoroutineUtil
{
    public static IEnumerator WaitForRealSeconds(float time)
    {
        float start = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < start + time)
        {
            yield return null;
        }
    }
}

public class NPCController : MonoBehaviour
{
    public float rotateTime = 2f;
    private float currentTime = 0f;
    public float conversationBuffer = 5f;
    private bool canTalk = true;

    public GameObject dialogueCanvas;

    public GameObject displayCanvas;

    public List<Sprite> upSprites = new List<Sprite>();
    public List<Sprite> downSprites = new List<Sprite>();
    public List<Sprite> leftSprites = new List<Sprite>();
    public List<Sprite> rightSprites = new List<Sprite>();
    private List<List<Sprite>> sprites = new List<List<Sprite>>();
    // Start is called before the first frame update
    void Start()
    {
        sprites.Add(leftSprites);
        sprites.Add(downSprites);
        sprites.Add(rightSprites);
        sprites.Add(upSprites);
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= rotateTime)
        {
            currentTime = 0f;
            rotate();
        }
        if (Input.GetKey("e"))
        {
            //print("holding e");
        }
        
    }
    void rotate()
    {
        int i = UnityEngine.Random.Range(0, 4);
        gameObject.transform.eulerAngles = new Vector3(0f, 0f, i * 90);
        gameObject.transform.root.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[i][0];


    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        displayCanvas.SetActive(true);
        if (Input.GetKey("e"))// && canTalk == true
        {
            dialogueCanvas.gameObject.SetActive(true);
            Time.timeScale = 0;
            StartCoroutine(resetConversation(conversationBuffer));
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        displayCanvas.SetActive(false);
    }

    IEnumerator resetConversation(float seconds)
    {
        // Do stuff
        canTalk = false;

        yield return new WaitForSeconds(seconds);

        canTalk = true;
        // Do other stuff
    }
}
