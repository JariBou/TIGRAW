using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Particles
{
    public enum AnimationMode{
        Oneshot, 
        Repeat, 
        Duration
    }
    public class ParticleScript : MonoBehaviour
    {
        public float duration;
        public AnimationMode animMode;
        public int repeatTimes;
        public int id; // Rn this is useless tho
        void Start()
        {
            switch (animMode)
            {
                case AnimationMode.Duration:
                    StartCoroutine(DelayedDestroy(duration));
                    break;
                
                case AnimationMode.Oneshot:
                    StartCoroutine(DelayedDestroy(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length));
                    break;
                
                case AnimationMode.Repeat:
                    StartCoroutine(DelayedDestroy(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length * repeatTimes));
                    break;
            }
        }
        
        IEnumerator DelayedDestroy(float delay = 0)
        {
            yield return new WaitForSeconds(delay);
            Destroy(gameObject);
        }

    }
}
