using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : MonoBehaviour
{
    public SpriteRenderer sr;
    public Animator anim;
    public Rigidbody2D rb;

    public float speed;
    public float chargeSpeed;
    public float chargeDistance;

    int direction = 1;
    float distance;
    bool charging;
    bool alerted;

    void Update()
    {
        distance = Player.pos.x - transform.position.x;
        if (distance < 0 && distance > -chargeDistance && direction == -1 || distance > 0 && distance < chargeDistance && direction == 1)
        {
            if (!alerted)
            {
                StartCoroutine(Charge());
            }
        }
        else
        {
            charging = false;
            alerted = false;
            anim.SetBool("Charge", false);
        }
        if (!Player.paused)
        {
            if (charging)
            {
                rb.velocity = new Vector2(chargeSpeed * direction, rb.velocity.y);
            }
            else if (!alerted)
            {
                rb.velocity = new Vector2(speed * direction, rb.velocity.y);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.isTrigger && col.name != "Player")
        {
            if (direction == 1)
            {
                direction = -1;
                sr.flipX = true;
            }
            else
            {
                direction = 1;
                sr.flipX = false;
            }
        }
    }

    IEnumerator Charge()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        alerted = true;
        anim.SetBool("Alerted", true);
        yield return new WaitForSeconds(1);
        charging = true;
        anim.SetBool("Charge", true);
        anim.SetBool("Alerted", false);
    }
}
