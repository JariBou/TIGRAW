using System;
using System.Collections.Generic;
using Enemies;
using LobbyScripts;
using PlayerBundle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BossRoomLogic : MonoBehaviour
{
    public MeleeBossAI boss;
    
    
    private float TimeToExecute = 3f;
    private float executeTimer;

    public List<Tilemap> groundTilemaps;

    private Player _player;

    private void Awake()
    {
        _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        GameObject.FindWithTag("GameManager").GetComponent<SoundManager>().PlayRandomBossMusic();
    }

    private void Start()
    {
        TimeToExecute = boss.TimeToExecute;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (boss.isInRange)
        {
            executeTimer += Time.fixedDeltaTime;
            if (executeTimer >= TimeToExecute)
            {
                _player.Kill();
            }
        }
        else
        {
            executeTimer -= Time.fixedDeltaTime;
        }
        
        UpdateTilemaps();
        
        if (boss.health <= 0)
        {
            TeleporterScript.Instance.isUsable = true;
        }
    }

    void UpdateTilemaps()
    {
        Vector3 rgb = Vector3.one;
        rgb -= Vector3.one * executeTimer / TimeToExecute;
        foreach (Tilemap tilemap in groundTilemaps)
        {
            tilemap.color = new Color(rgb.x, rgb.y, rgb.z);
        }
    }
}
