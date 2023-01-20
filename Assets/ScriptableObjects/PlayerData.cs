using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    // SO that stores data that needs to be kept between scenes AND potentially all player data to make the save system easier
    
    // Data that changes between scenes
    public int health;
    public int gold;
    public int crystals;
    
    // Data that is semi-persistant
    // ...
}
