using System;
using System.Collections.Generic;
using LoadingScripts;
using Saves;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerBundle
{
    public class PlayerInputsHandler : MonoBehaviour
    {
        private PlayerActions playerActions;
        private Player player;

        public Dictionary<string, Delegate> bindingLinks; // Dictionnary of <InputAction.name>
        // TODO: Problem if is composite like wasd, using InputAction.name should resolve that
    
        public Dictionary<string, int> spellLinks;
        private static readonly int XMovement = Animator.StringToHash("xMovement");
        private static readonly int YMovement = Animator.StringToHash("yMovement");
        private static readonly int Speed = Animator.StringToHash("speed");

        public static event Action<InputAction.CallbackContext> KeyPressedEvent; 
    
        private void Awake()
        {
            bindingLinks = new Dictionary<string, Delegate>();
            spellLinks = new Dictionary<string, int>();
            playerActions = new PlayerActions();
            bindingLinks.Add("A", new Action<InputAction.CallbackContext>(Move));
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
                        spellLinks[action.name] = int.Parse(action.name.Substring(action.name.Length-1, 1)); // Get the number of the spell, won't work with 2 digits tho so need to change
                        bindingLinks[action.name] = new Action<InputAction.CallbackContext>(ResolveSpellCasted);
                        break;
                }
            
            
            }
        }

        private void Start()
        {
            player = Player.Instance;
            Debug.Log("PlayerInputHandler Start method Called");
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
            Vector2 moveVector = context.ReadValue<Vector2>().normalized;
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
            } else if (Mathf.Abs((int)moveVector.y) == 1)
            {
                player.animator.SetInteger(player.Facing, 6 + 3*(int)moveVector.x);
            } 
    

            player.animator.SetFloat(XMovement, moveVector.x);
            player.animator.SetFloat(YMovement, moveVector.y);
            player.animator.SetFloat(Speed, moveVector.sqrMagnitude);
        }
    
        private void Dash(InputAction.CallbackContext context) {
            if (context.canceled){return;}
        
            SpellCasting.CastSpell(context, 3);
        }

        //TODO: Game Pausing should be handled by a script 
        public void PauseGame(InputAction.CallbackContext context)
        {
            if (context.canceled) return;
            
            Debug.Log("its here");
            
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

        private void ResolveSpellCasted(InputAction.CallbackContext context)
        {
            if (context.canceled) {return;}
            int spellId = spellLinks[context.action.name];
            SpellCasting.CastSpell(context, spellId);
        }


        private void ResolveKeyPressed(InputAction.CallbackContext context)
        {
            //string functionName = bindingLinks[action.bindings[0].name].Method.Name;
            string actionName = context.action.name; // This returns the good thing
        

            //actionName = "Move";
            // This works biatch

            bindingLinks[context.action.name].DynamicInvoke(context);
        }
    
        public void OnKeyPressedTriggerEvent(InputAction.CallbackContext context)
        {
            KeyPressedEvent?.Invoke(context);
        }
    }
}
