using System.Collections.Generic;
using Enemies.EnemiesAI;
using UnityEngine;
using UnityEngine.Serialization;

public class AutoEnemySpawning : MonoBehaviour
{
    public List<Transform> spawningPoints;
    public GameObject enemyPrefab;
    public GameObject player;

    [FormerlySerializedAs("Spawn")] public bool spawn;
    public int mobCap = 20;

    [FormerlySerializedAs("Timer")] [SerializeField] private float timer;
    public int respawnTime;
    public int numberOfEnemies;

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (!spawn) {return;}

        timer += respawnTime * Time.fixedDeltaTime;
        if (timer > respawnTime)
        {
            int count = 0;
            for (int i = 0; i < spawningPoints.Count; i++)
            {
                count += spawningPoints[i].childCount;
            }
            if (count >= mobCap) {return;}
            timer = 0f;
            for (int i = 0; i < numberOfEnemies; i++)
            {
                Transform spawningPoint = spawningPoints[Random.Range(0, spawningPoints.Count)];
                GameObject enemy = Instantiate(enemyPrefab, spawningPoint.position,
                    Quaternion.identity, spawningPoint);
                enemy.GetComponent<AIMeleeScript>().targetEntity = player;
            }
        }
        
    }
}
