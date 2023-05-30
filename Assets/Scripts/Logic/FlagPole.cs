using UnityEngine;


public class FlagPole : Interactable
{
    private bool _playerInRange;

    public Flag calledFlag;
    public bool isReusable;
    

    private void Awake()
    {
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
        if (!isUsable) {return;}
        Debug.Log($"Interacting with {name}");

        EventManager.InvokeFlagEvent(calledFlag);
    }

    protected override void OnFlagEvent(Flag flag)
    {
        if (isReusable) {return;}
        Destroy(gameObject);
    }
}
