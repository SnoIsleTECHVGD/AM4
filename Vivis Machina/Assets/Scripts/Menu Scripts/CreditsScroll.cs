using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsScroll : MonoBehaviour
{
    int goTo;

    void Update()
    {
        transform.position -= new Vector3((transform.position.x - goTo) * Time.deltaTime * 4f, 0, 0);
        if (Input.GetKeyDown(KeyCode.RightArrow) && goTo < 80)
        {
            goTo += 20;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && goTo > 0)
        {
            goTo -= 20;
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
    }
}
