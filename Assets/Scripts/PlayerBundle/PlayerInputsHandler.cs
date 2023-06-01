using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace PlayerBundle
{
    public class PlayerInputsHandler : MonoBehaviour
    {
        private PlayerActions playerActions;
        private Player player;

        public Dictionary<string, Delegate> bindingLinks; // Dictionnary of <InputAction.name>
        // TODO: Problem if is composite like wasd, using InputAction.name should resolve that
    
        public Dictionary<string, SpellSO> spellSoLinks;

        private static readonly int XMovement = Animator.StringToHash("xMovement");
        private static readonly int YMovement = Animator.StringToHash("yMovement");
        private static readonly int Speed = Animator.StringToHash("speed");

        public static event Action<InputAction.CallbackContext> KeyPressedEvent;

        public bool canCastSpells = true;
        private GameManager _gm;

        private List<string> lastInputs = new(16);

        private void Awake()
        {
            bindingLinks = new Dictionary<string, Delegate>();
            playerActions = new PlayerActions();
            bindingLinks.Add("A", new Action<InputAction.CallbackContext>(Move));
            _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
            _gm.PlayerInputsHandler = this;
            
            // bindingLinks.Add("RightClick", new Action<InputAction.CallbackContext, int>(SpellCasting.CastSpell));
            // bindingLinks["A"].DynamicInvoke(new InputAction.CallbackContext());
            // Debug.Log(bindingLinks["A"].Method.Name); // Returns 'Move'
            //Debug.Log(bindingLinks["RightClick"].Method.Name); // Returns 'CastSpell'


            // This goes through the bindings without composite duplicates
            // This should be only movement with sprint and menus and stuff

            List<InputAction> spellsInputActions = new List<InputAction>();

            foreach (var action in playerActions.Playermaps.Get().actions)
            {
                if (action.name.StartsWith("Spell"))
                {
                    spellsInputActions.Add(action);
                }

                action.performed += OnKeyPressedTriggerEvent;
                action.canceled += OnKeyPressedTriggerEvent;
                
                // Could have those in static functions, yeah whatever we'll see
                switch (action.name)
                {
                    case "Movement":
                        bindingLinks[action.name] = new Action<InputAction.CallbackContext>(Move);
                        break;
                    case "Dash":
                        bindingLinks[action.name] = new Action<InputAction.CallbackContext>(Dash);
                        break;
                    case "Escape":
                        bindingLinks[action.name] = new Action<InputAction.CallbackContext>(PauseGame);
                        break;
                    case "Interact":
                        bindingLinks[action.name] = new Action<InputAction.CallbackContext>(TryInteracting);
                        break;
                    case "OpenStats":
                        bindingLinks[action.name] = new Action<InputAction.CallbackContext>(ShowStats);
                        break;
                    case "Run":
                        bindingLinks[action.name] = new Action<InputAction.CallbackContext>(Run);
                        break;
                    case not null when action.name.StartsWith("Spell"):
                        bindingLinks[action.name] = new Action<InputAction.CallbackContext>(ResolveSpellCastedBySo);
                        break;
                }
            }
            player = GameObject.FindWithTag("Player").GetComponent<Player>();

        }

        private void Start()
        {
            player.animator.SetFloat(Speed, 0);
            player.animator.SetInteger(player.Facing, 6); // facing downwards
            // Debug.Log($"Player Animator: {player.animator}");
        }

        private void OnEnable()
        {
            playerActions.Playermaps.Enable();
            KeyPressedEvent += ResolveKeyPressed;
        }

        private void OnDisable()
        {
            playerActions.Playermaps.Disable();
            KeyPressedEvent -= ResolveKeyPressed;
        }

        private void Run(InputAction.CallbackContext context)
        {
            player.running = context.performed;
        }
        
        private void Move(InputAction.CallbackContext context) 
        {
            Move(context.ReadValue<Vector2>());
        }
        
        private void Move(Vector2 moveVector) 
        {
            //Debug.Log($"moveVector= {moveVector}");
            player.SetMoveVector(moveVector);

            // if ((int)moveVector.x == 1 && (int)moveVector.y == 0)
            // {
            //     player.animator.SetInteger(player.Facing, 3);
            // } else if ((int)moveVector.x == -1 && (int)moveVector.y == 0)
            // {
            //     player.animator.SetInteger(player.Facing, 9);
            // } else 

            if (Mathf.Abs((int)moveVector.y) == 1)
            {
                player.animator.SetInteger(player.Facing, 9 + 3*(int)moveVector.y);
            } else if (Mathf.Abs((int)moveVector.x) == 1)
            {
                player.animator.SetInteger(player.Facing, 6 - 3*(int)moveVector.x);
            } 
    

            player.animator.SetFloat(XMovement, moveVector.x);
            player.animator.SetFloat(YMovement, moveVector.y);
            player.animator.SetFloat(Speed, moveVector.sqrMagnitude);
        }
    
        private void Dash(InputAction.CallbackContext context) {
            if (context.canceled){return;}
        
            player.heatManager.CastSpell(context, 3);
        }

        //TODO: Game Pausing should be handled by a script 
        public void PauseGame(InputAction.CallbackContext context)
        {
            if (context.canceled) return;
            
            //For Testing only ======================
            // SaveManager.SaveToJson(JsonSaveData.Initialise());
            // SaveManager.LoadFromJson();
            // ======================================
            
            Time.timeScale = player.gamePaused ? 1 : 0;

            player.gamePaused = !player.gamePaused;
            player.pauseMenuCanvas.enabled = player.gamePaused;
            player.SetMoveVector(Vector2.zero);
        }
    
        public void TryInteracting(InputAction.CallbackContext context)
        {
            if (context.canceled) {return;}
            //Debug.Log("Trying to interact");
            Collider2D[] results = Physics2D.OverlapCircleAll(player.GetPosition(), player.interactionRange, player.interactableLayers);

            foreach (Collider2D col in results)
            {
                if (col.CompareTag("Interactable") && col.isTrigger)
                {
                    try
                    {
                        col.GetComponent<Interactable>().Interact();
                        Move(Vector2.zero);
                    }
                    catch (Exception e)
                    {
                        Debug.Log("Exception in TryInteracting script PlayerInputHandler");
                    }
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

        // NEW
        private void ResolveSpellCastedBySo(InputAction.CallbackContext context)
        {
            if (!context.performed || !canCastSpells) {return;}

            string actionName = context.action.name;
            if (lastInputs.Contains(actionName)) {return;}

            StartCoroutine(DoubleCastPrevent(actionName));
            
            SpellSO spellSo = _gm.spellBindingsSo[actionName];
            //Debug.Log($"SpellSo: {spellSo.spellName} VIA {actionName}");
            if (spellSo.spellTypeId == -1) // null spell
            {
                return;
            }
            player.heatManager.CastSpell(context, spellSo);
        }


        private void ResolveKeyPressed(InputAction.CallbackContext context)
        {
            //string functionName = bindingLinks[action.bindings[0].name].Method.Name;

            if (player.isTeleporting)
            {
                return;
            }

            string actionName = context.action.name; // This returns the good thing
            
            if (_gm.isInMenu)
            {

                if (actionName is "Escape" or "Interact")
                {
                    //Debug.Log("InMenu!");
                    TryInteracting(context);
                }
                return;
            }

            //actionName = "Move";

            bindingLinks[context.action.name].DynamicInvoke(context);
        }

        // Workaround for Left click triggering twice.. thanks Unity, small indie company right?
        IEnumerator DoubleCastPrevent(string actionName)
        {
            lastInputs.Add(actionName);
            yield return new WaitForSeconds(0.1f);
            lastInputs.Remove(actionName);
        }
    
        public void OnKeyPressedTriggerEvent(InputAction.CallbackContext context)
        {
            KeyPressedEvent?.Invoke(context);
        }
    }
}
