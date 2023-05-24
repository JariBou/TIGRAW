using System;
using System.Collections;
using System.Collections.Generic;
using MainMenusScripts;
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

    public int graceMargin = 3;
    public float heatAmount;
    public float baseMaxHeat = 100f; // Modified by BraceletUpgrades

    public float MaxHeat => baseMaxHeat + _gm.BraceletUpgradesHandler.GetUpgradedAmount(
        BraceletUpgrades.BonusMaxHeat);
    private bool infiniteHeat;

    private int heatLevel = 0;
    
    private float _heatTimer;
    private GameManager _gm;

    private float reduceHeatCooldown = 5f;
    private float reduceHeatTimer;


    private void Awake()
    {
        player = GetComponent<Player>();
        _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        reduceHeatTimer = reduceHeatCooldown;
    }
    
    public void ResetTimer() {
        _heatTimer = 0;
    }
    private void FixedUpdate()
    {
        if (reduceHeatTimer > 0)
        {
            reduceHeatTimer -= Time.fixedDeltaTime;
        }
        if (heatAmount > 0) heatAmount -= HeatRecoveryRate * Time.fixedDeltaTime;
        else if (heatAmount <= 0)
        {
            if (_gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.ReduceHeatLvl) > 0 && heatLevel>0 && reduceHeatTimer <= 0)
            {
                heatLevel -= 1;
                heatAmount = 75;
            }
            else
            {
                heatAmount = 0;
            }
        }

        if (player.IsPerformingAction()) {ResetTimer(); return;}
        
        _heatTimer += Time.fixedDeltaTime;

        if (_heatTimer >= 3) {
            if (heatAmount > 0) heatAmount -= HeatRecoveryRate * Time.fixedDeltaTime * BonusHeatRecoveryRate;
        }

        if (heatAmount - graceMargin > MaxHeat)
        {
            heatLevel++;
            heatAmount -= heatAmount + graceMargin;
            reduceHeatTimer = reduceHeatCooldown;
            if (heatLevel == 3)
            {
                _gm.LoadScene(SceneBuildIndex.DeathScreen);
            }
        }

    }

    public void AddHeat(float heat)
    {
        ResetTimer();
        heatAmount += heat;
    }

    public int GetHeat()
    {
        return (int)heatAmount;
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
        //Spell spellPrefabScript = spellPrefab.GetComponent<Spell>();

        ResetTimer();
        Instantiate(spellPrefab);
        
    }

    public int GetHeatLevel()
    {
        return heatLevel;
    }

    public float GetHeatLvl()
    {
        return heatLevel;
    }
}
