using UnityEngine;

namespace Enemies
{
    public class EnemyInterface : MonoBehaviour
    {
        public float attack;

        public float health;
        public GameObject _self;
        public bool isInRange;
        public static EnemyInterface Instance;

        public int id;

        public void Awake()
        {
            id = gameObject.GetInstanceID();
        }


        public void Damage(float amount)
        {
            health -= amount;
        }

    
    }
}
