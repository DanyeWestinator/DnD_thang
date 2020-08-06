using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class turnManager : MonoBehaviour
{
    public List<GameObject> fighters = new List<GameObject>();

    public GameObject displayCanvas;
    public movementMarkerController movementMarker;
    public combatSelector combatSelector;

    //private bool isPlayerTurn = true;


    public List<string> playerActions = new List<string> { "move", "attack" };
    private Dictionary<string, bool> playerActionsTaken = new Dictionary<string, bool>();
    private int currentAction = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (string action in playerActions)
        {
            playerActionsTaken.Add(action, false);
        }
        currentAction = -1;
        nextAction();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            nextAction();
        }
    }

    void nextAction()
    {
        currentAction++;
        if (currentAction >= playerActions.Count)
        {
            currentAction = 0;
        }
        Image currentImage = null;
        foreach (string action in playerActions)
        {
            currentImage = displayCanvas.transform.GetChild(0).Find(action + "Sprite").GetComponent<Image>();
            currentImage.color = new Color(currentImage.color.r, currentImage.color.b, currentImage.color.g, 0.4f);
        }
        currentImage = displayCanvas.transform.GetChild(0).Find(playerActions[currentAction] + "Sprite").GetComponent<Image>();
        currentImage.color = new Color(currentImage.color.r, currentImage.color.b, currentImage.color.g, 1f);

        switch (playerActions[currentAction])
        {
            case "move":
                combatSelector.setCanTarget(false);
                movementMarker.setGrid(true);
                playerActionsTaken["move"] = movementMarker.getMove();
                break;
            case "attack":
                movementMarker.setGrid(false);
                combatSelector.setCanTarget(true);
                break;
            default:
                movementMarker.setGrid(false);
                combatSelector.setCanTarget(false);
                break;
        }
    }

    public void markMove()
    {
        playerActionsTaken["move"] = !movementMarker.getMove();
    }

    //make it so you can't move again

    public void markX(string name)
    {
        displayCanvas.transform.GetChild(0).Find(name + "Sprite").Find(name + "X").gameObject.SetActive(true);
    }

    private void Reset()
    {
        foreach (string action in playerActions)
        {
            playerActionsTaken[action] = false;
            displayCanvas.transform.GetChild(0).Find(action + "Sprite").Find(name + "X").gameObject.SetActive(false);
        }
        currentAction = -1;
        nextAction();
        movementMarker.Reset();
    }
}
