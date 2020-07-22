using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class tilePathfinding : MonoBehaviour
{
    
    public Tilemap grid;
    public Tilemap environment;

    private HashSet<Vector3Int> envTiles = new HashSet<Vector3Int>();

    private Vector3Int pos;

    public int maxDepth = 5;

    private Color originalColor;
    private HashSet<Vector3Int> valid = new HashSet<Vector3Int>();

    public Color visitedColor = Color.blue;

    //private Dictionary<Vector3Int, bool> visited = new Dictionary<Vector3Int, bool>();
    HashSet<Vector3Int> visited = new HashSet<Vector3Int>();

    private bool hasStarted = false;
    int recursiveCalls = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        hasStarted = true;
        originalColor = grid.GetColor(Vector3Int.zero);
        for (int i = - (grid.size.x); i < grid.size.x; i++)
        {
            for (int j = - grid.size.y; j < grid.size.y; j++)
            {
                Vector3Int current = new Vector3Int(i, j, 0);
                grid.SetTileFlags(current, TileFlags.None);
                //visited.Add(current, false);
                if (environment.HasTile(current))
                {
                    envTiles.Add(current);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("r"))
        {
            markValid(Vector3Int.RoundToInt(transform.position));
            print(recursiveCalls);
        }

        if (Input.GetKey("t"))
        {
            resetVisited();
        }
    }
    
    public void markValid(Vector3Int start)
    {
        if (hasStarted == false)
        {
            Start();
        }
        start.x += 1;
        resetVisited();
        valid = recursiveFinder(0, start);
        foreach (Vector3Int item in valid)
        {
            grid.SetColor(item, visitedColor);
        }
    }

    public void resetVisited()
    {
        foreach (Vector3Int item in valid)
        {
            //Vector3Int newPos = item + new Vector3Int(0, 0, 1);
            //grid.SetColor(newPos, Color.clear);
            grid.SetColor(item, originalColor);
        }
        recursiveCalls = 0;
        visited = new HashSet<Vector3Int>();

    }

    HashSet<Vector3Int> findValid(Vector3Int start)
    {
        HashSet<Vector3Int> valid = new HashSet<Vector3Int>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                Vector3Int current = new Vector3Int(start.x + i, start.y + j, 0);
                if (envTiles.Contains(current) == false && current != start && visited.Contains(current) == false)
                {
                    valid.Add(current);
                }
            }
        }
        return valid;
    }


    HashSet<Vector3Int> recursiveFinder(int depth, Vector3Int start)
    {
        recursiveCalls++;
        start.z = 0;
        HashSet<Vector3Int> valid = new HashSet<Vector3Int>();
        if (hasStarted == false) { Start(); }
        
        if (depth >= maxDepth)
        {
            return valid;
        }
        

        valid = findValid(start);

        foreach (Vector3Int current in findValid(start))
        {
            valid.UnionWith(recursiveFinder(depth + 1, current));
        }

        visited.Add(start);

        return valid;
    }
    
    public HashSet<Vector3Int> getValid() { return valid; }
    public HashSet<Vector3> getValidWorldspace()
    {
        HashSet<Vector3> toReturn = new HashSet<Vector3>();
        foreach (Vector3Int item in valid)
        {
            Vector3 temp = grid.CellToWorld(item);
            temp.z = 0f;
            toReturn.Add(temp);
            //print("" + item + " " + grid.CellToWorld(item));
        }

        return toReturn;
    }
}
