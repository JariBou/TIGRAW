using PlayerBundle;
using UnityEngine;

namespace Spells.SpellBehavior
{
    public class Dash : MonoBehaviour
    {
        public Spell spell;

        private Vector2 _testVec;
        // Start is called before the first frame update
        void Start()
        {
            Vector2 moveVector = spell.player.GetMoveVector();
        
            // Problem when dashing towards positive x
            // Player's body velocity set to 0 when pressing spacebar somehow?? and only affecting towards positive x
            Debug.LogWarning($"moveVector={moveVector}");
            _testVec = new Vector2(moveVector.x, moveVector.y) * spell.dashDistance * Time.fixedDeltaTime;
            Debug.LogWarning($"testVec={_testVec}");
            spell.player.ApplyForce(_testVec);
            Invoke("EndDash", 0.25f);
        }

        void EndDash()
        {
            spell.player.isDashing = false;
            Destroy(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            transform.position = spell.player.GetPosition();
        }
    }
}
