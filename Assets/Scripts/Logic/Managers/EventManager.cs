﻿using System;
using UnityEngine;

public enum Flag
{
    StartRun,
    StartLevel,
    EndLevel,
    CancelLevel,
    PlayerDeath,
    PlayerWon,
    UnlockTeleporter, // Might be same as endLevel but lets keep it separated for now
}

public class EventManager
{
    public static event Action<Flag> FlagEvent;

    public static void InvokeFlagEvent(Flag obj)
    {
        Debug.Log($"Invoking event with param: {obj}");
        FlagEvent?.Invoke(obj);
    }
}