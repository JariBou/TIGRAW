using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulDisplay : MonoBehaviour
{
    public TMP_Text count;

    private GameManager _gm;

    private void Awake()
    {
        _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    private void FixedUpdate()
    {
        count.text = _gm.currentSave.playerSouls.ToString();
    }
}
