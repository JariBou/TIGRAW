using UnityEngine;

namespace Particles
{
    public class ParticleScript : MonoBehaviour
    {
        public float duration;
        public int id;
        // Start is called before the first frame update
        void Start()
        {
            Destroy(gameObject, duration);
        }

    }
}
