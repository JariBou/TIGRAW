using System.Collections;
using System.Collections.Generic;
using Enemies;
using PlayerBundle;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SummonedMeleeEntity : EnemyInterface
{
    private bool _canMove = false;

    private Transform targetEntity;

    private bool destroyOnContact = false;

    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EndSummoningSequence(GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length));
        targetEntity = GameObject.FindWithTag("Player").gameObject.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        base.FixedUpdate();
        Vector2 direction = (targetEntity.transform.position - transform.position);
        direction /= direction.magnitude;

        
        rb.velocity = direction * (speed * Time.fixedDeltaTime); // This one is actually in a fixed update so keep it like that
        
        if (isInRange)
        {
            AttackPlayer();
        }
    }
    
    IEnumerator EndSummoningSequence(float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        _canMove = true;
    }
    
    private void AttackPlayer()
    {
        if (DmgInteractionTimer > 0)
        {
            return;
        }
        else
        {
            targetEntity.GetComponent<Player>().Damage(attack);
            if (destroyOnContact)
            {
                Kill();
            }
            InitInteractionTimer();
        }

    }
    
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            // if (DmgInteractionTimer == 0)
            // {
            //     Debug.Log("ZAFUCK");
            //     col.GetComponent<Player>().health -= attack;
            //     DmgInteractionTimer = DmgInteractionDelay;
            // }
            isInRange = true;
        } 
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
}
