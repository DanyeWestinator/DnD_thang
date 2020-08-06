using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NPCController : MonoBehaviour
{
    public float rotateTime = 2f;
    private float currentTime = 0f;
    public float conversationBuffer = 5f;

    public GameObject dialogueCanvas;

    public GameObject displayCanvas;
    private bool inBox = false;
    public bool canRotate = false;

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
        if (canRotate && currentTime >= rotateTime)
        {
            currentTime = 0f;
            rotate();
        }
        if (inBox == true && Input.GetAxis("Interact") != 0)
        {
            dialogueCanvas.gameObject.SetActive(true);
            PlayerMove.canMove = false;
            //Time.timeScale = 0;
        }


    }
    void rotate()
    {
        int i = UnityEngine.Random.Range(0, 4);
        gameObject.transform.eulerAngles = new Vector3(0f, 0f, i * 90);
        gameObject.transform.root.gameObject.GetComponent<SpriteRenderer>().sprite = sprites[i][0];


    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        displayCanvas.SetActive(true);
        inBox = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inBox = false;
        displayCanvas.SetActive(false);
    }

}
