using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gridFill : MonoBehaviour
{
    public GameObject gridSquare;
    public int size = 40;
    // Start is called before the first frame update
    void Start()
    {
        fillGrid();
    }

    void fillGrid()
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                GameObject square = Instantiate(gridSquare);
                square.transform.parent = transform;
                Vector3 pos = square.transform.position;
                square.transform.localPosition = new Vector3(j, -i, 0);
                

            }
        }
    }
}
