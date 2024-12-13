using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame ()
    {
        SceneManager.LoadScene("pro-1");
        Time.timeScale = 1;
    }
    public void ResumeGame ()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void QuitLevel ()
    {
        SceneManager.LoadScene("pro-0");
        Time.timeScale = 1;
    }
    public void QuitGame ()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }
}
