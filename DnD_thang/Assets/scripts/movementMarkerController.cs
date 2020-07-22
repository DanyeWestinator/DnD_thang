using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static System.Math;

public class movementMarkerController : MonoBehaviour
{
    public float moveTime = .2f;    
    private float currentTime = 0f;
    
    public float sensitivity = 0.2f;
    private float currentDistance;
    
    public Tilemap grid;
    public GameObject player;

    //hash set chosen for constant time searching (since contains is used a lot)
    private HashSet<Vector3> valid = new HashSet<Vector3>();

    private tilePathfinding tpf;
    public bool setNewPath = true;

    private bool canMove = true;

    public bool displayGridAtStart = true;

    public turnManager turnManager;
    private bool hasStarted = false;


    // Start is called before the first frame update
    public void Start()
    {
        hasStarted = true;
        setNewPath = true;
        //sets the tilepathfinding component to the one in the player gameobject
        tpf = player.GetComponent<tilePathfinding>();
        currentTime = moveTime;
        transform.position = player.transform.position + new Vector3(0.5f, -0.5f, -1f);
        if (displayGridAtStart == true)
        {
            //tpf.markValid(Vector3Int.RoundToInt(player.transform.position));
        }
    }

    public void setMove(bool newStatus)
    {
        if (hasStarted == false) { Start(); }
        canMove = newStatus;
        
        if (canMove == true)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
            tpf.markValid(Vector3Int.RoundToInt(player.transform.position));
                
        }
        else
        {
            tpf.resetVisited();

            gameObject.transform.GetChild(0).gameObject.SetActive(false);
        }
        
    }

    public bool getMove() { return canMove && setNewPath; }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            currentTime += Time.deltaTime;
            //needs to check input to make sure that you don't redraw the blue squares every frame

            if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) && currentTime >= moveTime && setNewPath == true)
            {
                moveMarker();
                currentTime = 0f;
            }

            if (Input.GetAxis("Interact") != 0)
            {
                setNewPath = false;
                //turns off the players collider
                player.GetComponent<CapsuleCollider2D>().enabled = false;
                gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
                player.GetComponent<Pathfinding.AIPath>().canMove = true;
                player.GetComponent<Pathfinding.AIDestinationSetter>().target = transform.GetChild(0);
                tpf.resetVisited();
            }

            //TODO

            //turn colliders on and off as needed to avoid getting stuck
            if (setNewPath == false && player.GetComponent<Pathfinding.AIPath>().reachedDestination)
            {
                canMove = false;
                player.GetComponent<CapsuleCollider2D>().enabled = true;
                turnManager.markX("move");
                
                player.GetComponent<Pathfinding.AIPath>().canMove = false;
                //player.GetComponent<tilePathfinding>().markValid(Vector3Int.RoundToInt(player.transform.position));
            }
        }
       
    }

    public void Reset()
    {
        setNewPath = true;
        //gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
    }

    void moveMarker()
    {
        player.GetComponent<tilePathfinding>().markValid(Vector3Int.RoundToInt(player.transform.position));
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
