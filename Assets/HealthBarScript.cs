using System.Collections;
using System.Collections.Generic;
using PlayerBundle;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Image healthBarFill;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // MAybe create event when player's health changes
        float playerMaxHealth = Player.Instance.baseMaxHealth;
        float playerHealth = Player.Instance.health;


        healthBarFill.fillAmount = playerHealth / playerMaxHealth;
    }
}
