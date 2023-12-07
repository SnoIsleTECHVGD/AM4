using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;

    bool inAir;

    public Rigidbody2D rb;
    public SpriteRenderer sr;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector3(speed, rb.velocity.y, 0);
            sr.flipX = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector3(-speed, rb.velocity.y, 0);
            sr.flipX = true;
        }
        else
        {
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
        }
        if (Input.GetKey(KeyCode.X) && !inAir)
        {
            rb.velocity = new Vector3(rb.velocity.x, 2, 0);
        }
        if (true)
        {
            inAir = false;
        }
        else
        {
            inAir = true;
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.contacts);
    }
}
