using UnityEngine;

namespace Enemies.EnemiesAI
{
    public class AIDad: MonoBehaviour
    {
        public GameObject targetEntity;
        [Range(1, 512)]
        public int updateRate = 24; // Sets the number of frames after which the path is updated
        
    }
}