using System.Collections;
using System.Collections.Generic;
using Enemies;
using LobbyScripts;
using UnityEngine;

public class BossRoomLogic : MonoBehaviour
{
    public EnemyInterface boss;


    // Update is called once per frame
    void Update()
    {
        if (boss.health <= 0)
        {
            TeleporterScript.Instance.isUsable = true;
        }
    }
}
