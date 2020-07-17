using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class combatCharMove : MonoBehaviour
{
    private bool isTurn = false;
    public List<Sprite> upSprites = new List<Sprite>();
    public List<Sprite> downSprites = new List<Sprite>();
    public List<Sprite> leftSprites = new List<Sprite>();
    public List<Sprite> rightSprites = new List<Sprite>();

    public int startDirection = 0;
    public float distanceSensitivity = 0.01f;

    public float refreshTime = 0.1f;
    private float currentTime = 0f;

    private int direction = 0;
    private List<List<Sprite>> spriteLists = new List<List<Sprite>>();


    private Vector3 lastPos;
    // Start is called before the first frame update
    void Start()
    {
        direction = startDirection;
        lastPos = transform.position;
        spriteLists.Add(upSprites);
        spriteLists.Add(downSprites);
        spriteLists.Add(leftSprites);
        spriteLists.Add(rightSprites);
    }


    //fix sprite rotater
    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= refreshTime && direction != getDirection())
        {
            currentTime = 0f;
            //gameObject.GetComponentInChildren<SpriteRenderer>().sprite = spriteLists[getDirection()][0];
        }
        
    }



    public void setTurn(bool turn)
    {
       if (isTurn == false && turn == true)
       {
            GameObject camera = GameObject.Find("Main Camera"); 
            if (camera != null)
            {
                camera.GetComponent<cameraMover>().player = gameObject;
            }
       }
       isTurn = turn;
    }

    private int getDirection()
    {
        if (Vector3.Distance(lastPos, transform.position) <= distanceSensitivity)
        {
            //return direction;
        }
        Vector3 sub = transform.position - lastPos;
        float x = transform.position.x - lastPos.x;
        float y = transform.position.y - lastPos.y;
        if (Abs(x) > Abs(y))
        {
            return (x > 0) ? 2 : 3;
        }
        else if (Abs(y) > Abs(x))
        {
            return (y > 0) ? 0 : 1; 
        }

        return startDirection;


    }
}
