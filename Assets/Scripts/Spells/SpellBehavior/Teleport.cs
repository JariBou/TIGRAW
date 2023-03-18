using PlayerBundle;
using UnityEngine;

namespace Spells.SpellBehavior
{
    public class Teleport : MonoBehaviour
    {
    
        public Spell spell;
    
    
        // Start is called before the first frame update
        void Start()
        {
            if (!spell.GroundTilemap.HasTile(spell.GroundTilemap.WorldToCell(spell.MousePos)))
            {
                Debug.Log("DESTROYING");
                Destroy(gameObject);
                return;
            }
        
            Player.Instance.isTeleporting = true;
            Player.Instance.sprite.enabled = false;
            Player.Instance.SetVelocity(Vector2.zero);

            Player.Instance.heatAmount += spell.heatProduction;
            
            GameObject thingy = Instantiate(spell.startParticles, Player.Instance.transform.position, Quaternion.identity);
            Destroy(thingy.gameObject, 0.5f);

            Invoke("DoTeleport", spell.projectileSpeed);

        }


        void DoTeleport()
        {
            Debug.Log("TELEPORTING");
            Debug.Log(Player.Instance.transform.position);
            
            var position = transform.position;
            Debug.Log(position);
            Player.Instance.transform.position = position;
            
            GameObject thingy = Instantiate(spell.endParticles, position, Quaternion.identity);
            Destroy(thingy.gameObject, 0.5f);

            Player.Instance.isTeleporting = false;
            Player.Instance.sprite.enabled = true;

            Destroy(this);
        }
    }
}
