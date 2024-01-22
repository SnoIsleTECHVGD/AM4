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
    public static bool paused;
    public static bool respawn;
    public static bool unpause;

    bool inAir;
    Vector2 colPoint;
    bool[] onWall = new bool[] { false, false };
    float xVelocity;
    bool hurtStun;
    bool invincible;
    int hp;
    int deadId;
    bool pushingRb;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public Animator anim;
    public GameObject screenFade;
    public GameObject particles;
    public GameObject pauseMenu;
    public GameObject camera;
    public SpriteRenderer healthBarSr;
    public Sprite[] healthBars;

    void Start()
    {
        hp = defaultHp;
    }

    void Update()
    {
        //Collision stuff
        if (colPoint.x == 1 && !Input.GetKey(KeyCode.RightArrow))
        {
            onWall[0] = true;
            if (Input.GetKey(KeyCode.LeftArrow) && xVelocity < 0 && inAir)
            {
                xVelocity = 0;
            }
        }
        else
        {
            onWall[0] = false;
        }
        if (colPoint.x == -1 && !Input.GetKey(KeyCode.LeftArrow))
        {
            onWall[1] = true;
            if (Input.GetKey(KeyCode.RightArrow) && xVelocity > 0 && inAir)
            {
                xVelocity = 0;
            }
        }
        else
        {
            onWall[1] = false;
        }
        //Control stuff
        if (Input.GetKeyDown(KeyCode.Escape) || unpause)
        {
            if (paused)
            {
                unpause = false;
                paused = false;
                rb.simulated = true;
                screenFade.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0);
                Destroy(GameObject.Find("Pause(Clone)"));
            }
            else
            {
                paused = true;
                rb.simulated = false;
                screenFade.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 0.5f);
                Instantiate(pauseMenu, camera.transform);
            }
        }
        if (respawn)
        {
            StartCoroutine(Respawn(0));
            respawn = false;
            paused = false;
        }
        if (!paused)
        {
            if (Input.GetKey(KeyCode.RightArrow) && (!onWall[1] || pushingRb) && !hurtStun)
            {
                xVelocity = Mathf.Min(xVelocity + getEase(), speed);
                sr.flipX = false;
                onWall[0] = false;
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && (!onWall[0] || pushingRb) && !hurtStun)
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
                anim.SetInteger("DeadId", 0);
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
                if (deadId == 0)
                {
                    anim.SetBool("Hurt", true);
                }
                else
                {
                    anim.SetInteger("DeadId", deadId);
                }
                anim.SetBool("Pushing", false);
                anim.SetBool("Walking", false);
                anim.SetBool("Jumping", false);
                anim.SetBool("Sliding", false);
            }
            anim.SetBool("Boogie", false);
            if (Input.GetKey(KeyCode.B))
            {
                anim.SetBool("Boogie", true);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (Mathf.Abs(collision.contacts[0].normal.x) == 1 && (collision.gameObject.tag != "Movable" || inAir))
        {
            xVelocity = 0;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (Mathf.Abs(collision.contacts[0].normal.x) > 0.8)
        {
            colPoint = new Vector2(Mathf.RoundToInt(collision.contacts[0].normal.x), collision.contacts[0].normal.y);
            if (collision.gameObject.tag == "Movable" && !inAir)
            {
                pushingRb = true;
            }
            else
            {
                pushingRb = false;
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        colPoint = new Vector2();
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (deadId == 0)
        {
            if (!col.isTrigger)
            {
                inAir = false;
            }
            else if (col.name == "Checkpoint")
            {
                respawnPos = col.transform.position;
                col.GetComponent<SpriteRenderer>().color = Color.green;
            }
            else if ((col.name == "Boar" || col.name == "Damage Trigger") && !invincible)
            {
                if (hp > 1)
                {
                    StartCoroutine(Hurt());
                }
                else
                {
                    StartCoroutine(Respawn(1));
                }
            }
            else if (col.name == "Kill Trigger")
            {
                StartCoroutine(Respawn(0));
            }
            else if (col.name == "Spikes")
            {
                StartCoroutine(Respawn(2));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (!col.isTrigger)
        {
            inAir = true;
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
            return speed;
        }
    }

    IEnumerator Respawn(int deathType)
    {
        deadId = deathType + 1;
        invincible = true;
        hurtStun = true;
        if (deathType != 0)
        {
            particles.active = true;
            yield return new WaitForSeconds(1f);
        }
        for (float fade = 0; fade < 1; fade += Time.deltaTime)
        {
            screenFade.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, fade);
            yield return 1;
        }
        hp = defaultHp;
        transform.position = respawnPos;
        xVelocity = 0;
        rb.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(0.25f);
        deadId = 0;
        invincible = false;
        hurtStun = false;
        particles.active = false;
        for (float fade = 1; fade > 0; fade -= Time.deltaTime)
        {
            screenFade.GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, fade);
            yield return 1;
        }
    }

    IEnumerator Hurt()
    {
        hurtStun = true;
        invincible = true;
        hp--;
        healthBarSr.color = Color.white;
        healthBarSr.sprite = healthBars[hp - 1];
        particles.active = true;
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
        particles.active = false;
        invincible = false;
        healthBarSr.color = new Color(0, 0, 0, 0);
    }
}