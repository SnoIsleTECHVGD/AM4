using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float ease;

    bool inAir;
    Vector2 colPoint;
    bool[] onWall = new bool[] { false, false };
    float xVelocity;

    public Rigidbody2D rb;
    public SpriteRenderer sr;

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) && !onWall[1])
        {
            xVelocity = Mathf.Min(xVelocity + Time.deltaTime * ease, speed);
            sr.flipX = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !onWall[0])
        {
            xVelocity = Mathf.Max(xVelocity - Time.deltaTime * ease, -speed);
            sr.flipX = true;
        }
        else if (Mathf.Abs(xVelocity) > 0.1f)
        {
            xVelocity -= Mathf.Sign(xVelocity) * Time.deltaTime * ease;
        }
        else
        {
            xVelocity = 0;
        }
        if (Input.GetKeyDown(KeyCode.X) && !inAir)
        {
            inAir = true;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
        }
        else if (Input.GetKeyDown(KeyCode.X) && onWall[0])
        {
            rb.velocity = new Vector3(speed, jumpForce, 0);
        }
        else if (Input.GetKeyDown(KeyCode.X) && onWall[1])
        {
            rb.velocity = new Vector3(-speed, jumpForce, 0);
        }
        rb.velocity = new Vector3(xVelocity, rb.velocity.y, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        colPoint = collision.contacts[0].normal;
        if (colPoint == new Vector2(0, 1))
        {
            inAir = false;
        }
        else if (Mathf.Abs(colPoint.x) == 1)
        {
            StartCoroutine(HitWall(Mathf.RoundToInt(-colPoint.x / 2 + 0.5f)));
        }
    }

    IEnumerator HitWall(int side)
    {
        onWall[side] = true;
        while (Mathf.Abs(colPoint.x) == 1)
        {
            yield return 1;
        }
        onWall[side] = false;
    }
}
