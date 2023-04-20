using System;
using System.Collections;
using System.Collections.Generic;
using PlayerBundle;
using PlayerBundle.BraceletUpgrade;
using Spells;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeatManager: MonoBehaviour
{
    private Player player;
    
    [Header("Heat Mechanic")]
    public float baseHeatRecoveryRate = 2f; // Modified by BraceletUpgrades
    public float HeatRecoveryRate => baseHeatRecoveryRate +
                                     _gm.BraceletUpgradesHandler.GetUpgradedAmount(
                                         BraceletUpgrades.RecoveryRateIncrease);
    
    public float baseBonusHeatRecoveryRate = 2f; // Modified by BraceletUpgrades
    // By how much does the recovery rate get multiplied
    public float BonusHeatRecoveryRate => baseBonusHeatRecoveryRate +
                                     _gm.BraceletUpgradesHandler.GetUpgradedAmount(
                                         BraceletUpgrades.BonusRecoveryRateIncrease);
    
    public float heatAmount;
    public float baseMaxHeat = 100f; // Modified by BraceletUpgrades
    private bool infiniteHeat;
    
    private float _heatTimer;
    private GameManager _gm;


    private void Awake()
    {
        player = GetComponent<Player>();
        _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }
    
    public void ResetTimer() {
        _heatTimer = 0;
    }
    private void FixedUpdate()
    {
        if (heatAmount > 0) heatAmount -= HeatRecoveryRate * Time.fixedDeltaTime;
        else if (heatAmount < 0) heatAmount = 0;

        if (player.IsPerformingAction()) {ResetTimer(); return;}
        
        _heatTimer += 1f * Time.fixedDeltaTime;

        if (_heatTimer >= 3) {
            Debug.Log(HeatRecoveryRate);
            if (heatAmount > 0) heatAmount -= HeatRecoveryRate * Time.fixedDeltaTime * BonusHeatRecoveryRate;
        }

    }

    public void AddHeat(float heat)
    {
        ResetTimer();
        heatAmount += heat;
    }

    public float GetHeat()
    {
        return heatAmount;
    }
    
    void Update()
    {
        if (infiniteHeat)
        {
            heatAmount = 0;
        }
    }
    
    public void CastSpell(InputAction.CallbackContext context, int spellId)
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (player.isTeleporting) return;
        if (!context.performed) return;
        if (player.gamePaused) return;
        
        GameObject spellPrefab;

        try
        {
            spellPrefab = SpellsList.GetSpell(spellId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return;
        }

        Debug.Log($"Casting Spell: {spellPrefab.name}");
        Spell spellPrefabScript = spellPrefab.GetComponent<Spell>();
        if (heatAmount + spellPrefabScript.heatProduction <= baseMaxHeat)
        {
            ResetTimer();
            Instantiate(spellPrefab);
        }
    }
    
}
