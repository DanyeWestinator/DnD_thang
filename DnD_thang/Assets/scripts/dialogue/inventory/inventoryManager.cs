using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class inventoryManager : MonoBehaviour
{
    public GameObject baseButton;
    public Transform clueListTransform;
    public GameObject notebookCanvas;
    public Sprite book;
    public Sprite x;
    public Image background;
    public GameObject singleItemCanvas;
    private GameObject currentClue;
    public Button notebookButton;

    private static inventoryManager instance = null;

    public static inventoryManager Instance { get { return instance; } }



    private void Awake()
    {
        
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        instance = this;
        DontDestroyOnLoad(transform.root.gameObject);
    }

    public void createButton(GameObject go)
    {
        if (go.GetComponentInChildren<SpriteRenderer>() == null)
        {
            return;
        }
        currentClue = go;
        GameObject newButton = Instantiate(baseButton, clueListTransform);
        newButton.SetActive(true);
        newButton.GetComponent<Image>().sprite = go.GetComponentInChildren<SpriteRenderer>().sprite;
        clue clue = go.GetComponentInChildren<clue>();
        newButton.name = clue.clueName + "Clue";
        newButton.GetComponent<clueButton>().clueName = clue.clueName;
        newButton.GetComponent<clueButton>().flavorText = clue.flavorText;
        newButton.GetComponentInChildren<Text>().text = clue.clueName;
        newButton.GetComponent<clueButton>().clueLocation = clue.originLocation;
        //newButton.GetComponentInChildren<Text>().text = 
    }

    void clear()
    {
        while (clueListTransform.childCount != 0)
        {
            GameObject go = clueListTransform.GetChild(0).gameObject;
            Destroy(go);
        }
    }

    public void setButton(bool isOpen)
    {
        if (isOpen == false)
        {
            notebookButton.image.sprite = book;
            notebookButton.gameObject.GetComponentInChildren<Text>().text = "Notes";
        }
        else
        {
            notebookButton.image.sprite = x;
            notebookButton.gameObject.GetComponentInChildren<Text>().text = "Close";
        }
    }
    
    public void toggleCanvas()
    {
        
        if (singleItemCanvas.activeInHierarchy == false && notebookCanvas.activeInHierarchy == false)
        {
            notebookCanvas.SetActive(true);
        }
        else
        {
            if (currentClue != null)
            {
                Destroy(currentClue);
            }
            singleItemCanvas.SetActive(false);
            notebookCanvas.SetActive(false);
        }
        setButton(singleItemCanvas.activeInHierarchy || notebookCanvas.activeInHierarchy);
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            setButton(false);
            singleItemCanvas.SetActive(false);
            notebookCanvas.SetActive(false);
        }
    }
}
