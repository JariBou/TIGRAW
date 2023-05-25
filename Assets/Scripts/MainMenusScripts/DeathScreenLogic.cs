using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScreenLogic : MonoBehaviour
{
    private GameManager _gm;
    private void Awake()
    {
        _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    public void MainMenu()
    {
        _gm.LoadScene(SceneBuildIndex.MainMenu);
    }

    public void Lobby()
    {
        _gm.LoadScene(SceneBuildIndex.Lobby);
    }
}
