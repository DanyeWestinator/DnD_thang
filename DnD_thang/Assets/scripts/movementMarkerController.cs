using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static System.Math;


public class movementMarkerController : MonoBehaviour
{
    public float moveTime = .2f;    
    private float currentTime = 0f;
    
    private float currentDistance;
    
    public Tilemap grid;
    public GameObject player;


    //hash set chosen for constant time searching (since contains is used a lot)
    private HashSet<Vector3> valid = new HashSet<Vector3>();

    private tilePathfinding tpf;

    private bool hasStarted = false;

    public turnManager turnManager;

    private bool displayGrid = false;
    private bool canMove = true;


    //for the love of god refactor the code

    // Start is called before the first frame update
    public void Start()
    {
        if (hasStarted == true) { return; }
        
        hasStarted = true;
        canMove = true;

        //sets the tilepathfinding component to the one in the player gameobject
        tpf = player.GetComponent<tilePathfinding>();
        tpf.Start();
        currentTime = moveTime;
        transform.position = player.transform.position + new Vector3(0.5f, -0.5f, -1f);
        
    }


    public void setGrid(bool toSet)
    {
        if (hasStarted == false) { Start(); }
        displayGrid = toSet;
        gameObject.transform.GetChild(0).gameObject.SetActive(toSet);

        if (toSet == true)
        {
            Vector3 pos = player.transform.position;
            pos.y -= 0.1f;
            pos.x += 0.0f;
            tpf.markValid(Vector3Int.RoundToInt(pos));
        }
        else
        {
            tpf.resetVisited();
        }
    }

    public bool getMove()
    {
        if (hasStarted == false) { Start(); }
        return (tpf.distanceLeft > 0 && canMove);
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove && tpf.distanceLeft > 0)
        {
            currentTime += Time.deltaTime;
            //needs to check input to make sure that you don't redraw the blue squares every frame

            if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && currentTime >= moveTime && displayGrid == true)
            {
                moveMarker();
                currentTime = 0f;
            }

            if (Input.GetAxis("Interact") != 0 && displayGrid == true)
            {
                //print(tpf.distanceLeft);
                tpf.distanceLeft -= tpf.distanceToStart(Vector3Int.RoundToInt(transform.position), 0);
                displayGrid = false;
                //turns off the players collider
                player.GetComponent<CapsuleCollider2D>().enabled = false;
                transform.GetChild(0).gameObject.SetActive(false);
                player.GetComponent<Pathfinding.AIPath>().canMove = true;
                player.GetComponent<Pathfinding.AIDestinationSetter>().target = transform.GetChild(0);
                tpf.resetVisited();
                Vector3Int pos = Vector3Int.RoundToInt(transform.position);
                //pos.x -= 1;
                //print(tpf.distanceToStart(pos, 0));
                //print(tpf.distanceLeft);
            }


            if (displayGrid == false && player.GetComponent<Pathfinding.AIPath>().reachedDestination)
            {
                Vector3 pos = player.transform.position;
                //print("" + pos.x + " " + pos.y);
                pos += new Vector3(0.5f, 0.5f, 0);
                pos = Vector3Int.RoundToInt(pos);
                pos -= new Vector3(0.5f, 0.5f);
                //canMove = false;
                //print("" + pos.x + " " + pos.y);
                player.transform.position = pos;
                player.GetComponent<CapsuleCollider2D>().enabled = true;
                
                
                player.GetComponent<Pathfinding.AIPath>().canMove = false;
            }

            if (tpf.distanceLeft <= 0)
            {
                turnManager.markX("move");
                canMove = false;
            }
        }
       
    }

    public void Reset()
    {
        canMove = true;
        tpf.distanceLeft = tpf.maxDepth - 1;
    }

    void moveMarker()
    {
        //gets the direction that the player has inputted
        Vector3 direction = new Vector3(Sign(Input.GetAxis("Horizontal")), Sign(Input.GetAxis("Vertical")), 0f);
        Vector3 tryMove = transform.position + direction;
        
        //if you don't set z to zero, the set will not register it as containing that value
        tryMove.z = 0;

        //gets the hash set of valid spaces
        valid = tpf.getValidWorldspace();

        if (valid.Contains(tryMove + Vector3.left))
        {
            //resets the z to the original position
            tryMove.z = transform.position.z;

            //moves the dot to the new position
            transform.position = tryMove;
            return;
        }
        else
        {
            //tries going in the same direction as the player moved until it finds a valid space
            for (int i = 1; i <= tpf.maxDepth; i++)
            {
                if (valid.Contains(tryMove + (direction * i) + Vector3.left))
                {
                    tryMove.z = transform.position.z;
                    transform.position = tryMove + (direction * i);
                    return;
                }
            }
            //goes really far in the negative direction to find a valid space, and moves closer to the current space until it finds one
            for (int i = tpf.maxDepth * -2; i <= 0; i++)
            {
                if (valid.Contains(tryMove + (direction * i) + Vector3.left))
                {
                    tryMove.z = transform.position.z;
                    transform.position = tryMove + (direction * i);
                    return;
                }
            }
        }

        

       
    }
}
