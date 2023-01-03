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
            if (!spell.groundTilemap.HasTile(spell.groundTilemap.WorldToCell(spell.mousePos)))
            {
                Debug.Log("DESTROYING");
                Destroy(gameObject);
                return;
            }
        
            Player.instance.isTeleporting = true;
            Player.instance.sprite.enabled = false;
            Player.instance.SetVelocity(Vector2.zero);

            Player.instance.heatAmount += spell.heatProduction;
        
            //TODO: Remove assign and coroutine when everything will revert back to normal
            GameObject thingy = Instantiate(spell.startParticles, Player.instance.transform.position, Quaternion.identity);
            Destroy(thingy.gameObject, 0.5f);

            Invoke("DoTeleport", spell.projectileSpeed);

        }


        void DoTeleport()
        {
            Debug.Log("TELEPORTING");
            Debug.Log(Player.instance.transform.position);
            Debug.Log(transform.position);
            Player.instance.transform.position = transform.position;
        
            //TODO: Remove assign and coroutine when everything will revert back to normal
            GameObject thingy = Instantiate(spell.endParticles, transform.position, Quaternion.identity);
            Destroy(thingy.gameObject, 0.5f);

            Player.instance.isTeleporting = false;
            Player.instance.sprite.enabled = true;

            Destroy(this);
        }
        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
