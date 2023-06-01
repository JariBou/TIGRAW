using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using PlayerBundle.BraceletUpgrade;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class DummyScript : EnemyInterface
{
    [FormerlySerializedAs("damageText")] [Header("Dummy variables")] public TMP_Text display;
    public float timeUntilReset = 5f;
    public int damageDealt;
    private float timer;
    
    public override void Damage(float amount)
    {
        if (amount <= 0)
        {
            return;
        }

        if (shocked)
        {
            amount *= 1 + _gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.StatikDmgIncrease);
        }

        damageDealt += (int)amount;
        timer = timeUntilReset;
        Debug.Log($"Enemy with id {id} took {amount}DMG || shocked={shocked}");
        StartCoroutine(FlashOnDmg());
        animator.SetTrigger(Hurt);
    }

    public new void FixedUpdate()
    {
        ResolveStatusEffects();
        if (timer > 0)
        {
            timer -= Time.fixedDeltaTime;
        }
        
        if (timer <= 0)
        {
            timer = 0;
            damageDealt = 0;
        }
        display.text = $"Reset Timer: {Math.Round(timer, 3)}<br>Dmg dealt: {damageDealt}";
    }
}
