using System;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class Interactable : MonoBehaviour
{
    public bool isUsable;

    private void Start()
    {
        EventManager.FlagEvent += OnFlagEvent;
    }

    public abstract void Interact();

    protected abstract void OnFlagEvent(Flag flag);

    private void OnDestroy()
    {
        EventManager.FlagEvent -= OnFlagEvent;
    }
}
