using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScriptableObjects;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum BuyState
{
    CannotBuy,
    CanBuy,
    Unlocked
}
public class SpellBuyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // TODO: Spell SO maybe? to pass Spell ID idk... like one that has the Prefab and the Spell ID
    public SpellSO spellSo;
    [SerializeField]
    private Image icon;

    private Image bg;
    public BuyState state;
    
    public SpellShopManager manager;

    private void Awake()
    {
        bg = GetComponent<Image>();
    }

    private void Start()
    {
        UpdateState();
        UpdateGraphics();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            UpdateState();
            UpdateGraphics();
        }
    }

    public void UpdateState()
    {
        if (manager.gm.SpellBuyingHandler.unlockedSpells.Contains(spellSo))
        {
            state = BuyState.Unlocked;
        }
        else
        {
            state = manager.gm.currentSave.playerSouls < spellSo.spellCost ? BuyState.CannotBuy : BuyState.CanBuy;
        }
    }


    public void UpdateGraphics()
    {
        icon.sprite = spellSo.icon;
        icon.color = ColorUtils.ColorWithAlpha(icon.color, 1f);
        switch (state)
        {
            case BuyState.CannotBuy:
                icon.color = ColorUtils.ColorWithAlpha(icon.color, 0.5f);
                bg.sprite = manager.cannotBuy;
                break;
            case BuyState.CanBuy:
                bg.sprite = manager.canBuy;
                break;
            case BuyState.Unlocked:
                bg.sprite = manager.spellUnlocked;
                break;
        }
    }

    public void BuySpell()
    {
        switch (state)
        {
            case BuyState.Unlocked:
                Debug.Log($"Spell {spellSo.spellName} already bought");
                // This is where you let the player assign the spell to a key
                manager.StartReassignSpell(spellSo);
                return;
            case BuyState.CannotBuy:
                return;
            case BuyState.CanBuy:
                manager.gm.currentSave.playerSouls -= spellSo.spellCost;
                manager.gm.SpellBuyingHandler.BuySpell(spellSo);
                state = BuyState.Unlocked;
                return;
        }
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        manager.Tooltip.PassData(spellSo.spellName, spellSo.spellDescription, $"Cost: {spellSo.spellCost} || Overdrive cost: {spellSo.spellOverdriveCost}");
        manager.Tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        manager.Tooltip.SetActive(false);
    }

    private void OnEnable()
    {
        SpellBuyingHandler.SpellBought += OnSpellBought;
    }
    
    private void OnDisable()
    {
        SpellBuyingHandler.SpellBought -= OnSpellBought;
    }

    private void OnSpellBought(SpellSO obj)
    {
        UpdateState();
        UpdateGraphics();
    }
}
