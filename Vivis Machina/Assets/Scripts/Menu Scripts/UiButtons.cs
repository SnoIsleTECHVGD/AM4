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
    public void Resume()
    {
        Player.unpause = true;
    }
    public void Respawn()
    {
        Player.respawn = true;
        Player.unpause = true;
    }
    public void QuitPlay()
    {
        SceneManager.LoadScene("Title");
    }
}
