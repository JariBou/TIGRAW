using System;
using System.Collections;
using UnityEngine;

// Cast will be used for AOE Attacks around player AND AOE Targeted Attacks
public class Cast : MonoBehaviour
{
    
    public Spell spell;

    
    // Start is called before the first frame update
    void Start()
    {
        Player.instance.heatAmount += spell.heatProduction;
        
        StartCoroutine(DelayedDestroy(spell.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length));

        if (spell.damage > 0)
        {
            DealDamage();
        }
    }

    void DealDamage()
    {
        Collider2D[] results = Physics2D.OverlapCircleAll(transform.position, spell.DamageRadius, spell.enemyLayer);

        for (int i = 0; i < results.Length; i++)
        {
            try
            {
                results[i].GetComponent<MeleeEnemyScript>().Damage(spell.damage);

            }
            catch (Exception e)
            {
                Debug.LogError($"PROBLEM WITH {results[i]} Stacktrace: {e}");
            }
        }
    }
    
    /*private void OnTriggerEnter2D(Collider2D collider)
    {

        if (collider.CompareTag("Enemy"))
        {
            Debug.Log("HIT ENNEMY!");
            collider.GetComponent<MeleeEnemyScript>().Damage(spell.damage);
        }
    }*/
    
    IEnumerator DelayedDestroy(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
