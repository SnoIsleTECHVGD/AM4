using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiButtons : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene("Screen1");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }
}
