using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace MainMenusScripts
{
    public class MainMenu : MonoBehaviour
    {
        private GameManager _gm;
        private void Awake()
        {
            _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        }


        public void Exit()
        {
            Application.Quit();
        }

        public void Play()
        {
            _gm.LoadScene(4);
        }

        public void Options()
        {
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
        }
    
    }
}
