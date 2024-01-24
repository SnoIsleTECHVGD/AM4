using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    public Animator botAnim;
    public Animator ratAnim;
    public GameObject rat;
    public GameObject player;
    public SpriteRenderer fade;

    Vector2 ratStart;
    float t;
    void Start()
    {
        ratStart = rat.transform.position;
        fade.color = new Color(0, 0, 0, 1);
        StartCoroutine(Scene());
    }

    void Update()
    {
        t += Time.deltaTime;
    }

    IEnumerator Scene()
    {
        while (t < 2f)
        {
            fade.color = new Color(0, 0, 0, 1 - t / 2f);
            yield return 1;
        }
        fade.color = new Color(0, 0, 0, 0);
        botAnim.SetTrigger("Wake");
        while (t < 3f)
        {
            yield return 1;
        }
        ratAnim.SetTrigger("Walk");
        while (t < 4f)
        {
            rat.transform.position = ratStart - new Vector2((t - 3f) / 2.5f, 0);
            yield return 1;
        }
        ratAnim.SetTrigger("Idle");
        while (t < 5f)
        {
            yield return 1;
        }
        ratAnim.SetTrigger("Jump");
        ratStart = rat.transform.position;
        while (t < 5.4f)
        {
            rat.transform.position = ratStart - new Vector2((t * 1.5f - 7.5f) / 2f, -Mathf.Sin((t * 1.5f - 7.5f) * Mathf.PI) / 5f);
            yield return 1;
        }
        Destroy(rat);
        botAnim.SetTrigger("Rat");
        while (t < 7f)
        {
            yield return 1;
        }
        player.transform.position = transform.position;
        player.active = true;
        Destroy(gameObject);
    }
}
