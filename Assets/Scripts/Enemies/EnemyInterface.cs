using System;
using UnityEngine;

namespace Enemies
{
    public class EnemyInterface : MonoBehaviour
    {
        public float attack;

        public float health;
        public GameObject self;
        public bool isInRange;
        public static EnemyInterface Instance;

        public float DmgInteractionDelay = 2f;
        public float DmgInteractionTimer = 0f;

        public int id;

        public void Awake()
        {
            id = gameObject.GetInstanceID();
        }


        public void Damage(float amount)
        {
            health -= amount;
        }

        private void FixedUpdate()
        {
            if (DmgInteractionTimer > 0)
            {
                DmgInteractionTimer -= Time.fixedDeltaTime;
            }

            if (DmgInteractionTimer < 0)
            {
                DmgInteractionTimer = 0f;
            }
        }
    }
}
