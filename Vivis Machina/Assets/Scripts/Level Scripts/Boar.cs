using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : MonoBehaviour
{
    public SpriteRenderer sr;
    public Animator anim;

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
        if (charging)
        {
            transform.position += new Vector3(chargeSpeed * direction * Time.deltaTime, 0, 0);
        }
        else if (!alerted)
        {
            transform.position += new Vector3(speed * direction * Time.deltaTime, 0, 0);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
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

    IEnumerator Charge()
    {
        alerted = true;
        anim.SetBool("Alerted", true);
        yield return new WaitForSeconds(1);
        charging = true;
        anim.SetBool("Charge", true);
        anim.SetBool("Alerted", false);
    }
}
