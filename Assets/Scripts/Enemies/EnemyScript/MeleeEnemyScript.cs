using PlayerBundle;
using UnityEngine;

namespace Enemies.EnemyScript
{
    public class MeleeEnemyScript : EnemyInterface
    {

        public CircleCollider2D playerDetection;
        
        
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Player"))
            {
                // if (DmgInteractionTimer == 0)
                // {
                //     Debug.Log("ZAFUCK");
                //     col.GetComponent<Player>().health -= attack;
                //     DmgInteractionTimer = DmgInteractionDelay;
                // }
                isInRange = true;
            } 
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isInRange = false;
            }
        }
    }
}
