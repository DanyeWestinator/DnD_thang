using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class combatSelector : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();

    private bool canTarget = false;

    private int currentTarget = 0;

    public float intervalTime = 0.2f;
    private float currentTime = 0f;
    public float defaultTransparency = 0.3f;

    public float range = 5f;

    public Color inRangeColor = Color.red;
    public Color outOfRangeColor = Color.grey;
    public Animation swordAnim;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        //swordAnim.Stop();
        //swordAnim.enabled = false;
        animator = swordAnim.gameObject.GetComponent<Animator>();
        
        //animator.StopPlayback();
        //animator.enabled = false;
        animator.SetBool("swordAnim 0", false);
        currentTime = intervalTime;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("enemy"))
        {
            enemies.Add(go);
        }
        setCanTarget(false);
    }

    public void setCanTarget(bool toSet)
    {
        canTarget = toSet;
        if (toSet == false)
        {
            foreach (GameObject go in enemies)
            {
                go.GetComponentInChildren<crosshair>().setTransparency(0f);
                go.GetComponentInChildren<crosshair>().canRotate = false;
            }
        }
        else
        {
            foreach (GameObject go in enemies)
            {
                Vector2 dir = go.transform.position - transform.position;
                GameObject test = Physics2D.Raycast(transform.position, dir, range).collider.gameObject;
                if (test == go)
                {
                    go.GetComponentInChildren<crosshair>().setColor(inRangeColor);
                }
                else
                {
                    go.GetComponentInChildren<crosshair>().setColor(outOfRangeColor);
                }
                go.GetComponentInChildren<crosshair>().setTransparency(defaultTransparency);
            }
            enemies[currentTarget].GetComponentInChildren<crosshair>().setTransparency(1f);
            enemies[currentTarget].GetComponentInChildren<crosshair>().canRotate = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (canTarget)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= intervalTime && (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0))
            {
                currentTime = 0f;
                moveCursor();
            }
            
            if (Input.GetAxis("Interact") != 0 && currentTime >= intervalTime)
            {
                print(animator.GetBool("swordAnim 0"));
                currentTime = 0f;
                //swordAnim.gameObject.GetComponent<Animator>().enabled = true;
                //swordAnim.enabled = true;
                //swordAnim.Play();
                pointSword();
                //animator.SetBool("swordAnim 0", false);
                //animator.enabled = false;
            }
            else if (Input.GetAxis("Interact") != 0 && currentTime < intervalTime)
            {
                animator.SetBool("swordAnim 0", true);

            }
        }
    }
    void pointSword()
    {
        GameObject swordParent = transform.GetChild(1).gameObject.transform.GetChild(0).gameObject;
        swordParent.SetActive(true);
        Vector3 dir = enemies[currentTarget].transform.position - transform.GetChild(1).transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        swordParent.transform.localScale = new Vector3(1f, 1f, 1f);
        if (dir.x < 0)
        {
            swordParent.transform.localScale = new Vector3(1f, -1f, 1f);
        }
        swordParent.transform.localPosition = new Vector3(-0.72f * Sign(dir.x), 0f, -5f);
        swordParent.transform.eulerAngles = new Vector3(0, 0, angle);
    }
    void moveCursor()
    {
        //print("moving cursor");
        while (enemies[currentTarget] == null)
        {
            print("null enemy");
            currentTarget++;
            if (currentTarget >= enemies.Count)
                currentTarget = 0;
        }
        enemies[currentTarget].GetComponentInChildren<crosshair>().setTransparency(defaultTransparency);
        enemies[currentTarget].GetComponentInChildren<crosshair>().canRotate = false;
        //enemies[currentTarget].GetComponentInChildren<crosshair>().resetRotation();
        currentTarget++;
        if (currentTarget >= enemies.Count)
            currentTarget = 0;
        enemies[currentTarget].GetComponentInChildren<crosshair>().setTransparency(1f);
        enemies[currentTarget].GetComponentInChildren<crosshair>().canRotate = true;
    }
}
