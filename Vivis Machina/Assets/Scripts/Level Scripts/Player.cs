using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float ease;

    public static Vector2 pos;

    bool inAir;
    Vector2 colPoint;
    bool[] onWall = new bool[] { false, false };
    float xVelocity;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator anim;

    void Update()
    {
        //Collision stuff
        if (colPoint != new Vector2(1, 0))
        {
            onWall[0] = false;
        }
        else
        {
            onWall[0] = true;
            xVelocity = 0;
            if (colPoint == new Vector2(0, 1))
            {
                onWall[0] = false;
            }
        }
        if (colPoint != new Vector2(-1, 0))
        {
            onWall[1] = false;
        }
        else
        {
            onWall[1] = true;
            xVelocity = 0;
            if (colPoint == new Vector2(0, 1))
            {
                onWall[1] = false;
            }
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
            xVelocity -= Mathf.Sign(xVelocity) * Time.deltaTime * ease;
        }
        else
        {
            xVelocity = 0;
        }
        if (Input.GetKeyDown(KeyCode.X) && !inAir)
        {
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
        pos = transform.position;
        //Animation stuff
        if (inAir)
        {
            anim.SetBool("Jumping", true);
            anim.SetBool("Pushing", false);
            anim.SetBool("Walking", false);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (onWall[1])
            {
                anim.SetBool("Pushing", true);
                anim.SetBool("Walking", false);
            }
            else
            {
                anim.SetBool("Walking", true);
                anim.SetBool("Pushing", false);
            }
            anim.SetBool("Jumping", false);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (onWall[0])
            {
                anim.SetBool("Pushing", true);
                anim.SetBool("Walking", false);
            }
            else
            {
                anim.SetBool("Walking", true);
                anim.SetBool("Pushing", false);
            }
            anim.SetBool("Jumping", false);
        }
        else
        {
            anim.SetBool("Pushing", false);
            anim.SetBool("Walking", false);
            anim.SetBool("Jumping", false);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        colPoint = collision.contacts[0].normal;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        colPoint = collision.contacts[0].normal;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        inAir = false;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        inAir = true;
    } 

    float getEase()
    {
        if (inAir)
        {
            return Time.deltaTime * ease;
        }
        else
        {
            return speed;
        }
    }
}
