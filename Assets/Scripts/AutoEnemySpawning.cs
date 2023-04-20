using System;
using System.Collections.Generic;
using System.Linq;
using Enemies.EnemiesAI;
using PlayerBundle;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class AutoEnemySpawning : MonoBehaviour
{
    public List<Transform> spawningPoints;
    public GameObject enemyPrefab;
    public List<EnemySpawn> enemyPrefabs;
    private GameObject player;

    [FormerlySerializedAs("Spawn")] public bool spawn = false;
    public int mobCap = 20;

    [FormerlySerializedAs("Timer")] [SerializeField] private float timer;
    public int respawnTime;
    public int packSize;
    
    private GameManager _gm;

    private void Awake()
    {
        _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        timer = 0f;
        player =  GameObject.FindGameObjectWithTag("Player");
        enemyPrefabs.Sort((a, b) => a.Weight.CompareTo(b.Weight));
        float maxValue = enemyPrefabs.Sum(e => e.Weight);
        foreach (var enemySpawn in enemyPrefabs)
        {
            enemySpawn.Weight = Clamp01Relatively(enemySpawn.Weight, maxValue);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // TODO: Look for player in the vicinity to avoid spawning on top of player 
        if (!spawn) {return;}

        timer += respawnTime * Time.fixedDeltaTime;
        if (!(timer > respawnTime)) return;

        int count = GetCurrentMobCap() + packSize;
        
        if (count >= mobCap) {return;}
        timer = 0f;
        for (int i = 0; i < packSize; i++)
        {
            Transform spawningPoint = spawningPoints[Random.Range(0, spawningPoints.Count)];
            GameObject enemy = Instantiate(DetermineSpawningEnemy(), spawningPoint.position,
                Quaternion.identity, spawningPoint);
            AIMeleeScript script = enemy.GetComponent<AIMeleeScript>();
            script.targetEntity = player;
            script.updateRate = _gm.UpdateRate;
        }
    }

    public int GetCurrentMobCap()
    {
        return spawningPoints.Sum(point => point.childCount); // CurrentMobCount
    }

    public GameObject DetermineSpawningEnemy()
    {
        float totalWeight = 0;
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            totalWeight += enemyPrefabs[i].Weight;
        }

        float randomValue = Random.Range(0f, totalWeight);
        
        float currentMax = 0;
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            currentMax += enemyPrefabs[i].Weight;
            if (randomValue <= currentMax)
            {
                Debug.Log($"Spawning {enemyPrefabs[i].NAME}");

                return enemyPrefabs[i].EnemyPrefab;
            }
        }
        Debug.LogError("WTF IS THIS");
        return enemyPrefab;
    }
    
    private float Clamp01Relatively(float value, float maxValue)
    {
        return ClampRelatively(value, 1f, maxValue);
    }

    private float ClampRelatively(float value, float max, float maxValue)
    {
        float ratio = value / maxValue;
        return ratio * max;
    }
}

[Serializable]
public class EnemySpawn
{
    public string NAME;
    public GameObject EnemyPrefab;
    [Range(0, 1)]
    public float Weight;
}
