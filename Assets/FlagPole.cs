using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FlagPole : Interactable
{
    private bool _playerInRange;

    public Flag calledFlag;
    public bool isReusable;
    
    public static FlagPole Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _playerInRange = false;
        }
    }

    public override void Interact()
    {
        if (!IsUsable) {return;}
        Debug.Log($"Interacting with {name}");

        EventManager.InvokeFlagEvent(calledFlag);
    }

    public override void OnFlagEvent(Flag flag)
    {
        IsUsable = false;
    }
}
