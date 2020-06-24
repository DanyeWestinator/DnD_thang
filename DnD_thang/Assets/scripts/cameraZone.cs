using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraZone : MonoBehaviour
{
    public GameObject Camera;
    private bool moveCamera = false;
    private float distance = 0f;
    private Vector3 pos;
    private Rigidbody2D rb;

    public float publicSpeed = 1f;
    private static float speed;

    private void Start()
    {
        pos = gameObject.transform.position;
        rb = Camera.gameObject.GetComponent<Rigidbody2D>();
        speed = publicSpeed;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            moveCamera = true;
            distance = Vector3.Distance(collision.gameObject.transform.position, gameObject.transform.position);
            //Camera.transform.position = gameObject.transform.position; 
        }
    }

    private void Update()
    {
        
        if (moveCamera == true)
        {
            Transform cPos = Camera.gameObject.transform;
            if (distance >= Vector3.Distance(pos, cPos.position))
            {
                rb.velocity = (pos - cPos.position).normalized * speed;
            }
            if ((pos - cPos.position).magnitude <= .1f)
            {
                
                rb.velocity = (pos - cPos.position).normalized * speed;
            }
            distance = Vector3.Distance(pos, cPos.position);

            if (cPos.rotation.z != 0)
            {
                cPos.eulerAngles = new Vector3(cPos.rotation.x, cPos.rotation.y, 0f);
            }
        }
    }
}
