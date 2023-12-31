using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float ease;
    public float wallJumpForce;
    public int defaultHp;

    public static Vector2 respawnPos;
    public static Vector2 pos;

    bool inAir;
    Vector2 colPoint;
    bool[] onWall = new bool[] { false, false };
    float xVelocity;
    bool hurtStun;
    bool invincible;
    int hp;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator anim;

    void Start()
    {
        hp = defaultHp;
    }

    void Update()
    {
        //Collision stuff
        if (colPoint == new Vector2(1, 0) && !Input.GetKey(KeyCode.RightArrow))
        {
            onWall[0] = true;
            if (Input.GetKey(KeyCode.LeftArrow) && xVelocity < 0)
            {
                xVelocity = 0;
            }
        }
        else
        {
            onWall[0] = false;
        }
        if (colPoint == new Vector2(-1, 0) && !Input.GetKey(KeyCode.LeftArrow))
        {
            onWall[1] = true;
            if (Input.GetKey(KeyCode.RightArrow) && xVelocity > 0)
            {
                xVelocity = 0;
            }
        }
        else
        {
            onWall[1] = false;
        }
        //Control stuff
        if (Input.GetKey(KeyCode.R))
        {
            Respawn();
        }
        if (Input.GetKey(KeyCode.RightArrow) && !onWall[1] && !hurtStun)
        {
            xVelocity = Mathf.Min(xVelocity + getEase(), speed);
            sr.flipX = false;
            onWall[0] = false;
        }
        else if (Input.GetKey(KeyCode.LeftArrow) && !onWall[0] && !hurtStun)
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
        if (!hurtStun)
        {
            if (Input.GetKeyDown(KeyCode.X) && !inAir)
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
            }
            else if (Input.GetKeyDown(KeyCode.X) && onWall[0])
            {
                xVelocity = speed * wallJumpForce;
                onWall[0] = false;
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
                colPoint = new Vector2();
            }
            else if (Input.GetKeyDown(KeyCode.X) && onWall[1])
            {
                xVelocity = -speed * wallJumpForce;
                onWall[1] = false;
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);
                colPoint = new Vector2();
            }
        }
        rb.velocity = new Vector3(xVelocity, rb.velocity.y, 0);
        pos = transform.position;
        //Animation stuff
        if (!hurtStun)
        {
            anim.SetBool("Hurt", false);
            if (inAir)
            {
                if (onWall[0] || onWall[1])
                {
                    anim.SetBool("Sliding", true);
                    anim.SetBool("Jumping", false);
                }
                else
                {
                    anim.SetBool("Jumping", true);
                    anim.SetBool("Sliding", false);
                }
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
        else
        {
            anim.SetBool("Hurt", true);
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
        hp = defaultHp;
        transform.position = respawnPos;
        xVelocity = 0;
        rb.velocity = new Vector2(0, 0);
    }

    IEnumerator Hurt()
    {
        hurtStun = true;
        invincible = true;
        hp--;
        for (int value = 0; value < 8; value++)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.05f);
            sr.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
        hurtStun = false;
        for (int value = 0; value < 4; value++)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.05f);
            sr.enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
        invincible = false;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.name == "Checkpoint")
        {
            respawnPos = col.transform.position;
            col.GetComponent<SpriteRenderer>().color = Color.green;
        }
        if (col.name == "Boar" && !invincible)
        {
            if (hp > 1)
            {
                StartCoroutine(Hurt());
            }
            else
            {
                Respawn();
            }
        }
    }
}
