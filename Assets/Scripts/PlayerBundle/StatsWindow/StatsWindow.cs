using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsWindow : MonoBehaviour
{
    private Canvas canvas;


    public TMP_Text healthText;
    public TMP_Text atkMultiplierText;
    public TMP_Text armorText;

    private Player player;
    
    public static StatsWindow instance { get; private set; }
    

    private void Start()
    {
        canvas = GetComponent<Canvas>();
        canvas.enabled = false;
        instance = this;
    }

    public void Enable()
    {
        player = Player.instance;
        canvas.enabled = true;

        healthText.text = player.health.ToString();
        atkMultiplierText.text = player.atkMultiplier.ToString();
        armorText.text = player.armor.ToString();
    }
    
    public void Disable()
    {
        canvas.enabled = false;
    }
}
