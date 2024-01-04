using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float ease;

    public static Vector2 respawnPos;
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
        if (colPoint != new Vector2(1, 0) || Input.GetKey(KeyCode.RightArrow))
        {
            onWall[0] = false;
        }
        else
        {
            onWall[0] = true;
            xVelocity = 0;
        }
        if (colPoint != new Vector2(-1, 0) || Input.GetKey(KeyCode.LeftArrow))
        {
            onWall[1] = false;
        }
        else
        {
            onWall[1] = true;
            xVelocity = 0;
        }
        //Control stuff
        if (Input.GetKey(KeyCode.R))
        {
            Respawn();
        }
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
            if (onWall[0] && Input.GetKey(KeyCode.LeftArrow) || onWall[1] && Input.GetKey(KeyCode.RightArrow))
            {
                anim.SetBool("Sliding", true);
                anim.SetBool("Jumping", false);
                anim.SetBool("Pushing", false);
                anim.SetBool("Walking", false);

            }
            else
            {
                anim.SetBool("Jumping", true);
                anim.SetBool("Pushing", false);
                anim.SetBool("Walking", false);
                anim.SetBool("Sliding", false);
            }
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
            anim.SetBool("Sliding", false);
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
            anim.SetBool("Sliding", false);
        }
        else
        {
            anim.SetBool("Pushing", false);
            anim.SetBool("Walking", false);
            anim.SetBool("Jumping", false);
            anim.SetBool("Sliding", false);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        colPoint = collision.contacts[0].normal;
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        colPoint = new Vector2();
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

    private void Respawn()
    {
        transform.position = respawnPos;
        xVelocity = 0;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "Checkpoint")
        {
            respawnPos = col.transform.position;
            col.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
}
