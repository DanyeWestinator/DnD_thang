using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clueButton : MonoBehaviour
{
    public Text displayText;
    public string flavorText;
    public Text locationFound;
    public string clueLocation;
    public Text nameText;
    public string clueName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void setDisplayWindow()
    {
        displayText.text = flavorText;
        locationFound.text = "Found in:\n\t" + clueLocation;
        nameText.text = clueName;
    }
}
