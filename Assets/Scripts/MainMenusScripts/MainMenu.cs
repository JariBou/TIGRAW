using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void Exit()
    {
        Application.Quit();
    }

    public void Play()
    {
        GameManager.LoadScene(3);
    }

    public void Options()
    {
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }
    
}
