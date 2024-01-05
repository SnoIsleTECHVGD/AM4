using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : MonoBehaviour
{
    public float speed;

    int direction = 1;

    void Start()
    {
        
    }

    void Update()
    {
        transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.contacts[0].normal.x == 1)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
    }
}
