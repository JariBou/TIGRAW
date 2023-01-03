using System.Collections.Generic;
using Enemies.EnemiesAI;
using UnityEngine;

public class AutoEnemySpawning : MonoBehaviour
{
    public List<Transform> spawningPoints;
    public GameObject ennemyPrefab;
    public GameObject player;

    public bool Spawn;
    public int mobCap = 20;

    [SerializeField] private float Timer;
    public int respawnTime;
    public int numberOfEnnemies;

    // Start is called before the first frame update
    void Start()
    {
        Timer = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        if (!Spawn) {return;}

        Timer += respawnTime * Time.fixedDeltaTime;
        if (Timer > respawnTime)
        {
            int count = 0;
            for (int i = 0; i < spawningPoints.Count; i++)
            {
                count += spawningPoints[i].childCount;
            }
            if (count >= mobCap) {return;}
            Timer = 0f;
            for (int i = 0; i < numberOfEnnemies; i++)
            {
                Transform spawningPoint = spawningPoints[Random.Range(0, spawningPoints.Count)];
                GameObject ennemy = Instantiate(ennemyPrefab, spawningPoint.position,
                    Quaternion.identity, spawningPoint);
                ennemy.GetComponent<AIMeleeScript>().targetEntity = player;
            }
        }
        
    }
}
