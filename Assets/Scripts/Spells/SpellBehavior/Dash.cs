using UnityEngine;

namespace Spells.SpellBehavior
{
    public class Dash : MonoBehaviour
    {
        public Spell spell;

        private Vector2 _testVec;

        private Vector2 savedMoveVector;
        // Start is called before the first frame update
        void Start()
        {
            savedMoveVector = spell.player.GetMoveVector();
        
            // Problem when dashing towards positive x
            // Player's body velocity set to 0 when pressing spacebar somehow?? and only affecting towards positive x
            Debug.LogWarning($"moveVector={savedMoveVector}");
            _testVec = new Vector2(savedMoveVector.x, savedMoveVector.y) * spell.dashDistance * Time.fixedDeltaTime;
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
