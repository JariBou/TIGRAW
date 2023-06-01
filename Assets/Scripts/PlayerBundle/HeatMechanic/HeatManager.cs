using System;
using PlayerBundle;
using PlayerBundle.BraceletUpgrade;
using ScriptableObjects;
using Spells;
using UI;
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
    public bool infiniteHeat;

    private int heatLevel = 0;

    public int MaxHeatLevel =>
        3 + (int)_gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.NewOverdriveLevel) + (int)_gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.NewOverdriveLevels);
    
    private float _heatTimer;
    private GameManager _gm;

    private float reduceHeatCooldown = 5f;
    private float reduceHeatTimer;

    private VignetteScript _vignetteScript;
    private UIController _uiController;

    private void Awake()
    {
        player = GetComponent<Player>();
        _gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _uiController = GameObject.FindWithTag("UIController").GetComponent<UIController>();
        _vignetteScript = _uiController.vignetteScript;
        
        reduceHeatTimer = reduceHeatCooldown;
        heatLevel = _gm.heatLevelSave;
        
        // For some reason this is called twice... really?
        for (int i = 0; i < _gm.HeatMaluses.Count; i++)
        {
            Debug.Log($"Adding: {_gm.HeatMaluses[i].StatusType.ToString()}");
            player.StatusEffects.Add(_gm.HeatMaluses[i].Copy());
            _uiController.HeatMalusesDisplayList[i].SetActive(false);

            if (heatLevel > i)
            {
                _uiController.HeatMalusesDisplayList[i].SetActive(true);
                player.StatusEffects[i].Duration = 1;
            }
        }
    }

    public int GetMaxHeatLevel()
    {
        return 3 + (int)_gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.NewOverdriveLevel) + (int)_gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.NewOverdriveLevels);
    }

    public float GetMaxHeat()
    {
        return baseMaxHeat + _gm.BraceletUpgradesHandler.GetUpgradedAmount(
            BraceletUpgrades.BonusMaxHeat);
    }

    public float GetHeatRecoveryRate()
    {
        return HeatRecoveryRate * player.OverdriveMultiplier;
    }
    
    public float GetBonusHeatRecoveryRate()
    {
        return HeatRecoveryRate * BonusHeatRecoveryRate * player.OverdriveMultiplier;
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
        if (heatAmount > 0) heatAmount -= GetHeatRecoveryRate() * Time.fixedDeltaTime;
        else if (heatAmount <= 0)
        {
            if (_gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.ReduceHeatLvl) > 0 && heatLevel>0 && reduceHeatTimer <= 0)
            {
                _vignetteScript.ApplyVignette(VignetteColor.Blue);
                heatLevel -= 1;
                reduceHeatTimer = reduceHeatCooldown;
                try
                {
                    player.StatusEffects[heatLevel].Duration = 0;
                    _uiController.HeatMalusesDisplayList[heatLevel].SetActive(false);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                
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
            if (heatAmount > 0) heatAmount -= GetBonusHeatRecoveryRate() * Time.fixedDeltaTime;
        }

        if (heatAmount - graceMargin > GetMaxHeat())
        {
            heatLevel++;
            _vignetteScript.ApplyVignette(VignetteColor.Red);
            heatAmount -= heatAmount + graceMargin;
            reduceHeatTimer = reduceHeatCooldown;
            if (heatLevel == GetMaxHeatLevel())
            {
                EventManager.InvokeFlagEvent(Flag.PlayerDeath);
                return;
            }

            try
            {
                player.StatusEffects[heatLevel - 1].Duration = 1;
                _uiController.HeatMalusesDisplayList[heatLevel - 1].SetActive(true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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
    
    public void CastSpell(InputAction.CallbackContext context, SpellSO spellSo)
    {
        if (player.isTeleporting) return;
        if (!context.performed) return;
        if (player.gamePaused) return;
        
        GameObject spellPrefab = spellSo.spellPrefab;

        Debug.Log($"Casting Spell: {spellPrefab.name}");
        //Spell spellPrefabScript = spellPrefab.GetComponent<Spell>();

        if (spellSo.SpellsType != SpellsType.Dash &&
            _gm.BraceletUpgradesHandler.GetUpgradedAmount(BraceletUpgrades.IFramesWhileDashing) > 0)
        {
            ResetTimer();
        }
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
