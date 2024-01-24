using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartFade : MonoBehaviour
{
    public static bool startUp = true;
    public Image image;
    void Start()
    {
        if (startUp)
        {
            StartCoroutine(Fade());
        }
        else
        {
            Destroy(gameObject);
        }
        startUp = false;
    }

    IEnumerator Fade()
    {
        for (float value = 2; value > 0; value -= Time.deltaTime / 3f)
        {
            image.color = new Color(1, 1, 1, value);
            yield return 1;
        }
        Destroy(gameObject);
    }
}
