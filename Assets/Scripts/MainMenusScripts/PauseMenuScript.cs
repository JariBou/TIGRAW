using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public void ReturnToLobby()
    {
        //TODO: TimeScale stays at 0 when going from scene to scene
        Time.timeScale = 1;
        SceneManager.LoadScene(3);
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}
