using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    // SO that stores data that needs to be kept between scenes AND potentially all player data to make the save system easier
    
    // Data that changes between scenes
    [SerializeField]
    private float maxHealth;
    public float MaxHealth => maxHealth;
    public float health = 1;
    public int gold;
    public int crystals;

    public void ResetRun()
    {
        health = maxHealth;
    }

    // Data that is semi-persistant
    // ...
}
