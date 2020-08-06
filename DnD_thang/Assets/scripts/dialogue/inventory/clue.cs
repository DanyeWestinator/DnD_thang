using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clue : MonoBehaviour
{
    public string clueName;
    public string originLocation = "";
    public string flavorText = "";
    public inventoryManager inventoryManager;
    public Text prompt;
    private Canvas clueCanvas;

    private bool inBox;
    
    // Start is called before the first frame update
    void Start()
    {
        if (clueCanvas == null)
        {
            clueCanvas = inventoryManager.gameObject.transform.Find("singleItemCanvas").GetComponent<Canvas>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inBox && Input.GetAxis("Interact") != 0)
        {
            inBox = false;
            clueCanvas.gameObject.SetActive(true);
            clueCanvas.transform.Find("Location").gameObject.GetComponent<Text>().text = "Found in:\n\t" + originLocation;
            clueCanvas.transform.Find("Flavortext").gameObject.GetComponent<Text>().text = flavorText;
            clueCanvas.transform.Find("Name").gameObject.GetComponent<Text>().text = clueName;
            clueCanvas.transform.Find("Image").gameObject.GetComponent<Image>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
            inventoryManager.setButton(true);
            inventoryManager.createButton(this.gameObject);
            //addToJournal();
        }
        
    }

    void addToJournal()
    {
        inventoryManager.createButton(this.gameObject);
        
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inBox = true;
        prompt.gameObject.SetActive(true);
        prompt.text = "Press E to inspect " + clueName;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        inBox = false;
        prompt.gameObject.SetActive(false);
    }
}
