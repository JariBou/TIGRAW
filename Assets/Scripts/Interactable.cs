using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public bool IsUsable;

    private void Start()
    {
        EventManager.FlagEvent += OnFlagEvent;
    }

    public abstract void Interact();

    public abstract void OnFlagEvent(Flag flag);
}
