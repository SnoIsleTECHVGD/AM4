using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float ease;

    bool inAir;
    List<Vector2> colPoints;
    bool[] onWall = new bool[] { false, false };
    float xVelocity;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator anim;

    void Update()
    {
        //Collision stuff
        if (!colPoints.Contains(new Vector2(1, 0)))
        {
            onWall[0] = false;
        }
        else
        {
            onWall[0] = true;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            if (colPoints.Contains(new Vector2(0, 1)))
            {
                onWall[0] = false;
            }
        }
        if (!colPoints.Contains(new Vector2(-1, 0)))
        {
            onWall[1] = false;
        }
        else
        {
            onWall[1] = true;
            rb.velocity = new Vector3(0, rb.velocity.y, 0);
            if (colPoints.Contains(new Vector2(0, 1)))
            {
                onWall[1] = false;
            }
        }
        if (colPoints.Contains(new Vector2(0, 1)))
        {
            inAir = false;
        }
        else
        {
            inAir = true;
        }
        //Movement stuff
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
            anim.SetBool("Walking", false);
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
        //Animation stuff
        if (Input.GetKey(KeyCode.RightArrow) && onWall[1])
        {
            anim.SetBool("Pushing", true);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && onWall[0])
        {
            anim.SetBool("Pushing", true);
        }
        else
        {
            anim.SetBool("Pushing", false);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        CollisionSet(collision);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        CollisionSet(collision);
    }

    void CollisionSet(Collision2D collision)
    {
        colPoints = new List<Vector2>();
        for (int value = 0; value < collision.contacts.Length; value++)
        {
            Debug.Log(collision.contacts[value].normal);
            colPoints.Add(collision.contacts[value].normal);
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
