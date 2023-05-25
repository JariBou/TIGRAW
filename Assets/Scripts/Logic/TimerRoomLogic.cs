using System;
using System.Globalization;
using TMPro;
using UnityEngine;

public class TimerRoomLogic : MonoBehaviour
{
    public bool doCountdown;
    public float time;
    public float startDelay;
    private bool started = false;
    public TMP_Text timerText;
    
    private float timer;
    private AutoEnemySpawning _autoEnemySpawning;
    
    private void Awake()
    {
        _autoEnemySpawning = GameObject.FindWithTag("SpawnHandler").GetComponent<AutoEnemySpawning>();
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = time;
        EventManager.FlagEvent += OnFlagEvent;
    }

    private void OnDestroy()
    {
        EventManager.FlagEvent -= OnFlagEvent;
    }

    private void OnFlagEvent(Flag obj)
    {
        if (obj == Flag.StartLevel)
        {
            started = true;
            doCountdown = true;
            _autoEnemySpawning.spawn = true;
            timerText.text = Math.Round(timer, 1).ToString(CultureInfo.InvariantCulture);
        }
        else if (obj == Flag.UnlockTeleporter)
        {
            Destroy(this);
        }
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!started) {return;}
        if (!doCountdown)
        {
            if (_autoEnemySpawning.GetCurrentMobCap() == 0)
            {
                EventManager.InvokeFlagEvent(Flag.UnlockTeleporter);
            }
            return;
        }
        timer -= Time.fixedDeltaTime;
        timerText.text = Math.Round(timer, 1).ToString(CultureInfo.InvariantCulture);
        if (timer <= 0)
        {
            _autoEnemySpawning.spawn = false;
            doCountdown = false;
        }
    }
    
}
