using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class sceneSwitcher : MonoBehaviour
{
    public string sceneToSwitchTo;
    public GameObject player;
    public GameObject promptCanvas;
    private Vector3 position;
    //private int timesHere = 0;
    private static GameObject instance;

    public Vector3 newPos = Vector3.zero;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        promptCanvas.SetActive(true);
        if (Input.GetKey("e"))
        {
            SceneManager.LoadScene(sceneToSwitchTo);
            StartCoroutine(move());
            
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        promptCanvas.SetActive(false);
    }

    IEnumerator move()
    {

        //GameObject.FindGameObjectWithTag("Player").transform.position = newPos;
        Transform.FindObjectOfType<Camera>().transform.position = newPos;

        yield return new WaitForSeconds(.001f);
        GameObject.FindGameObjectWithTag("Player").transform.position = newPos;
        Transform.FindObjectOfType<Camera>().transform.position = newPos;

        //print("moved");

        //print(player.transform.position);

        Destroy(this.gameObject);
        
    }
}
