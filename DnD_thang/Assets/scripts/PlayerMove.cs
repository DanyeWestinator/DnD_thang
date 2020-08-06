using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;



public class PlayerMove : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed = 5f;
    public float sensitivity = .1f;

    private int i = 0;
    private Sprite currentSprite;
    private List<Sprite> currentList;

    public float animationLength = 0.5f;
    private float currentTime = 0f;

    public List<Sprite> upSprites = new List<Sprite>();
    public List<Sprite> downSprites = new List<Sprite>();
    public List<Sprite> leftSprites = new List<Sprite>();
    public List<Sprite> rightSprites = new List<Sprite>();
    public static bool canMove = true;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        currentSprite = downSprites[0];
        currentList = downSprites;
        updateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = Vector3.zero;
        if (Abs(Input.GetAxis("Horizontal")) >= sensitivity || Abs(Input.GetAxis("Vertical")) >= sensitivity)
        {
            if (canMove)
                Move();
        }
        
    }

    void Move()
    {
        List<Sprite> oldSprites = currentList;
        float vertMove = Input.GetAxis("Vertical") * speed;
        float horizMove = Input.GetAxis("Horizontal") * speed;
        float hAbs = Abs(horizMove);
        float vAbs = Abs(vertMove);

        if (hAbs > vAbs)
        {
            currentList = leftSprites;
            if (horizMove > 0)
            {
                currentList = rightSprites;
            }
        }
        else
        {
            currentList = upSprites;
            if (vertMove < 0)
            {
                currentList = downSprites;
            }
        }
        if (currentList != oldSprites)
        {
            i = 0;
            updateSprite();
        }
        
        currentTime += Time.deltaTime;
        if (currentTime >= animationLength)
        {
            currentTime = 0f;
            updateSprite();
        }
        
        rb.velocity = new Vector2(horizMove, vertMove);
        if (hAbs < sensitivity && vAbs < sensitivity)
        {
            rb.velocity = Vector2.zero;
            i = 0;
            updateSprite();
        }
    }

    void updateSprite()
    {
        i += 1;
        if (i >= currentList.Count)
        {
            i = 0;
        }
        currentSprite = currentList[i];
        this.gameObject.GetComponentInChildren<SpriteRenderer>().sprite = currentSprite;
    }
}
