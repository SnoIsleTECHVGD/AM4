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
    public Animator anim;

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow) && !onWall[1])
        {
            xVelocity = Mathf.Min(xVelocity + getEase(), speed);
            sr.flipX = false;
            onWall[0] = false;

        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !onWall[0])
        {
            xVelocity = Mathf.Max(xVelocity - getEase(), -speed);
            sr.flipX = true;
            onWall[1] = false;
        }
        else if (Mathf.Abs(xVelocity) > 0.05f && inAir)
        {
            xVelocity -= Mathf.Sign(xVelocity) * Time.deltaTime * ease;
        }
        else
        {
            anim.SetBool("Walking", false);
            xVelocity = 0;
        }
        if (Input.GetKeyDown(KeyCode.X) && !inAir)
        {
            inAir = true;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
        }
        else if (Input.GetKeyDown(KeyCode.X) && onWall[0])
        {
            xVelocity = speed;
            onWall[0] = false;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
        }
        else if (Input.GetKeyDown(KeyCode.X) && onWall[1])
        {
            xVelocity = -speed;
            onWall[1] = false;
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
        }
        rb.velocity = new Vector3(xVelocity, rb.velocity.y, 0);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        colPoint = collision.contacts[0].normal;
        if (colPoint == new Vector2(0, 1))
        {
            inAir = false;
            onWall = new bool[] { false, false };
        }
        else if (Mathf.Abs(colPoint.x) == 1)
        {
            onWall[Mathf.RoundToInt(-colPoint.x / 2 + 0.5f)] = true;
            xVelocity = 0;
        }
    }

    float getEase()
    {
        if (inAir)
        {
            return Time.deltaTime * ease;
        }
        else
        {
            anim.SetBool("Walking", true);
            return speed;
        }
    }
}
