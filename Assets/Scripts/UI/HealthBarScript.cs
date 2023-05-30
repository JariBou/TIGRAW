using PlayerBundle;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Image healthBarFill;

    private Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Maybe create event when player's health changes
        float playerMaxHealth = player.MaxHealth;
        float playerHealth = player.health;


        healthBarFill.fillAmount = playerHealth / playerMaxHealth;
    }
}
