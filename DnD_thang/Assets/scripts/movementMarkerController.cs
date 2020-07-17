using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static System.Math;

public class movementMarkerController : MonoBehaviour
{
    public float moveTime = .2f;    
    private float currentTime = 0f;
    
    public float distanceSensitivity = 0.1f;
    private float currentDistance;
    
    public Tilemap grid;
    public GameObject player;
    private HashSet<Vector3Int> valid = new HashSet<Vector3Int>();

    private tilePathfinding tpf;
    private bool setNewPath = false;

    private bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        tpf = player.GetComponent<tilePathfinding>();
        currentTime = moveTime;
    }

    void setMove(bool newStatus)
    {
        if (newStatus != canMove)
        {
            if (newStatus == true)
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        currentTime += Time.deltaTime;
        if ((Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0 ) && currentTime >= moveTime)
        {
            player.GetComponent<tilePathfinding>().markValid(Vector3Int.RoundToInt(player.transform.position));
            valid = player.GetComponent<tilePathfinding>().getValid();
            //print(valid.Count);
            moveMarker();
            currentTime = 0f;
        }
        if (Input.GetAxis("Interact") != 0)
        {
            player.GetComponent<Pathfinding.AIPath>().canMove = true;
            player.GetComponent<Pathfinding.AIDestinationSetter>().target = transform.GetChild(0);
            tpf.resetVisited();
        }
        //turn colliders on and off as needed to avoid getting stuck
        if (setNewPath == false && player.GetComponent<Pathfinding.AIPath>().reachedDestination)
        {
            setNewPath = true;
            player.GetComponent<Pathfinding.AIPath>().canMove = false;
            player.GetComponent<tilePathfinding>().markValid(Vector3Int.RoundToInt(player.transform.position));
            valid = player.GetComponent<tilePathfinding>().getValid();
        }
       
    }

    void moveMarker()
    {
        //gets the direction that the player has inputted
        Vector3 direction = new Vector3(Sign(Input.GetAxis("Horizontal")), Sign(Input.GetAxis("Vertical")), 0f);
        Vector3 tryMove = transform.position + direction;
        //if you don't set z to zero, the set will not register it as containing that value
        tryMove.z = 0;
        HashSet<Vector3> test = tpf.getValidWorldspace();

        if (test.Contains(tryMove + Vector3.left))
        {
            tryMove.z = transform.position.z;
            transform.position = tryMove;
            return;
        }
        else
        {
            for (int i = 1; i <= tpf.maxDepth; i++)
            {
                if (test.Contains(tryMove + (direction * i) + Vector3.left))
                {
                    tryMove.z = transform.position.z;
                    transform.position = tryMove + (direction * i);
                    print("hopping");
                    return;
                }
            }
            for (int i = tpf.maxDepth * -2; i <= 0; i++)
            {
                if (test.Contains(tryMove + (direction * i) + Vector3.left))
                {
                    tryMove.z = transform.position.z;
                    transform.position = tryMove + (direction * i);
                    return;
                }
            }
        }

        

       
    }
}
