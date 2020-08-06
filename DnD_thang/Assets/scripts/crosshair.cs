using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

public class crosshair : MonoBehaviour
{
    public bool canRotate = false;
    public float rotationSpeed = 1f;

    public float roundingSensitivity = 0.01f;
    


    public void resetRotation()
    {
        transform.eulerAngles = Vector3.zero;

    }
    public void setColor(Color c)
    {
        gameObject.GetComponent<SpriteRenderer>().color = c;
    }


    void toggleSprite(bool toSet)
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = toSet;
    }

    // Update is called once per frame
    void Update()
    {
        if (Abs((transform.eulerAngles.z % 90 )) < roundingSensitivity && canRotate == false)
        {
            resetRotation();
        }
        if (canRotate == true || (transform.rotation.eulerAngles.z % 90) != 0)
        {
            Vector3 oldRotation = transform.rotation.eulerAngles;

            oldRotation.z += Time.deltaTime * rotationSpeed;
            transform.eulerAngles = oldRotation;
        }
        
    }

    public void setTransparency(float a)
    {
        /*
        if (hasStarted == false) { Start(); }
        if (a > 0)
        {
            Vector2 dir = -transform.position + player.transform.position;
            if (Physics2D.Raycast(transform.position, dir).collider.gameObject == player)
            {
                gameObject.GetComponent<SpriteRenderer>()
            }
            print("" +  + " " + transform.parent.gameObject.name);
        }*/
        Color newColor = gameObject.GetComponent<SpriteRenderer>().color;
        newColor.a = a;
        gameObject.GetComponent<SpriteRenderer>().color = newColor;
    }
}
