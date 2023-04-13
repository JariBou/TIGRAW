using System.Collections;
using System.Collections.Generic;
using LobbyScripts;
using Unity.VisualScripting;
using UnityEngine;

public class TimerRoomLogic : MonoBehaviour
{
    public bool doCountdown;
    public float time;
    public float startDelay;
    
    private float timer;
    // Start is called before the first frame update
    void Start()
    {
        timer = time;
        StartCoroutine(DelayedStart());
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!doCountdown) {return;}
        timer -= Time.fixedDeltaTime;
        if (timer <= 0)
        {
            TeleporterScript.Instance.IsUsable = true;
            doCountdown = false;
        }
    }

    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(startDelay);
        doCountdown = true;
        AutoEnemySpawning.Instance.spawn = true;
    }
}
