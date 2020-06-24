using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMover : MonoBehaviour
{
    public GameObject player;
    private Transform endPosition;
    private Vector3 startPosition;

    public float speed = 1f;
    private float startTime;
    private float distance;

    public float refreshTime = .1f;
    private float currentTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        endPosition = player.transform;
        startTime = Time.time;
        Transform p = GameObject.Find("Norman").transform;
        transform.position = new Vector3(p.position.x, p.position.y, -15f);
        startPosition = this.gameObject.transform.position;
        distance = Vector2.Distance(startPosition, endPosition.position);
        StartCoroutine(resetCameraZ());
    }

    IEnumerator resetCameraZ()
    {
        yield return new WaitForSeconds(0.01f);
        transform.position = new Vector3(transform.position.x, transform.position.y, -15f);
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= refreshTime)
        {
            currentTime = 0f;
            endPosition = player.transform;
            startPosition = this.gameObject.transform.position;
            startTime = Time.time;
            distance = Vector3.Distance(startPosition, endPosition.position);
        }

        

        if (distance != 0)
        {
            float distanceCovered = (Time.time - startTime) * speed;

            float fractionDistance = distanceCovered / distance;

            this.gameObject.transform.position = Vector3.Lerp(startPosition, endPosition.position, fractionDistance);
            this.gameObject.transform.position = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, startPosition.z);
        }
        
    }
}
