using PlayerBundle;
using UnityEngine;
using UnityEngine.Serialization;

namespace Enemies.EnemiesAI
{
    public class AIDad: MonoBehaviour
    {
        [FormerlySerializedAs("targetEntity")] public Player targetPlayer;
        [Range(1, 512)]
        public int updateRate = 24; // Sets the number of frames after which the path is updated

        public EnemyInterface _enemyInstance;

    }
}