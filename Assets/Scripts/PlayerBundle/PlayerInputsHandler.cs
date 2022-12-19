using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputsHandler : MonoBehaviour
{
    private PlayerActions playerActions;
    private Player player;
    private void Awake()
    {
        playerActions = new PlayerActions();
    }

    private void Start()
    {
        player = Player.instance;
    }

    private void OnEnable()
    {
        playerActions.Playermaps.Enable();
    }

    private void OnDisable()
    {
        playerActions.Playermaps.Disable();
    }
    
    
    public void Move(InputAction.CallbackContext context) 
    {
        Vector2 moveVector = context.ReadValue<Vector2>().normalized;
        player.SetMoveVector(moveVector);
    
        if ((int)moveVector.x == 1 && (int)moveVector.y == 0)
        {
            player.animator.SetInteger(player.Facing, 3);
        } else if ((int)moveVector.x == -1 && (int)moveVector.y == 0)
        {
            player.animator.SetInteger(player.Facing, 9);
        } else if (Mathf.Abs((int)moveVector.y) == 1)
        {
            player.animator.SetInteger(player.Facing, 9 + 3*(int)moveVector.y);
        } 
        

        player.animator.SetFloat("xMovement", moveVector.x);
        player.animator.SetFloat("yMovement", moveVector.y);
        player.animator.SetFloat("speed", moveVector.sqrMagnitude);

    }
    
    public void Dash(InputAction.CallbackContext context) {
        if (!context.performed){return;}
        
        SpellCasting.CastSpell(context, 4);
    }
    
    public void Teleport(InputAction.CallbackContext context) {
        if (!context.performed){return;}
     
        SpellCasting.CastSpell(context, 3);
    }
    
    public void DoBasicAttack(InputAction.CallbackContext context) {
        if (!context.performed){return;}

        SpellCasting.CastSpell(context, 2);
    }
    
    public void DoThunderStrike(InputAction.CallbackContext context) {
        if (!context.performed){return;}
        
        SpellCasting.CastSpell(context, 1);
    }
    
    //TODO: Game PAusing should be handled by a script 
    public void PauseGame(InputAction.CallbackContext context)
    {
        if (!context.performed) return;
        
        if (player.gamePaused)
        {
            Time.timeScale = 1;
        }
        else
        {
            Time.timeScale = 0;
        }

        player.gamePaused = !player.gamePaused;
        player.pauseMenuCanvas.enabled = player.gamePaused;
        player.SetMoveVector(Vector2.zero);
    }
    
    public void TryInteracting(InputAction.CallbackContext context)
    {
        if (!context.performed) {return;}
        
        Collider2D[] results = Physics2D.OverlapCircleAll(player.GetPosition(), player.interactionRange, player.interactableLayers);

        foreach (Collider2D col in results)
        {
            Debug.LogWarning(col.name);
            if (col.CompareTag("Interactable"))
            {
                col.GetComponent<Interactable>().Interact();
            }
        }
    }

    public void ShowStats(InputAction.CallbackContext context)
    {
        if(context.performed) // the key has been pressed
        {
            player.playerStatsScript.Enable();
        }
        if(context.canceled) //the key has been released
        {
            player.playerStatsScript.Disable();
        }
    }
    
    
}
