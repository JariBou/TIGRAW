using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SpellShopManager : MonoBehaviour
{
    public Tooltip Tooltip;

    [Header("Upgrades")] 
    public Sprite cannotBuy;
    public Sprite canBuy;
    public Sprite spellUnlocked;
    
    [Space]
    public GameManager gm;
    [SerializeField]
    private GameObject spellReassignGui;

    public SpellSO reassignedSpell;
    
    [Header("Auto Button Creation")]
    public Transform parentPanel;
    public GameObject spellBuyPrefab;

    private Canvas _canvasRenderer;
    [SerializeField] private CanvasGroup buySpellsPanel;
    private List<SpellBuyButton> _listOfButtons;

    public bool isReassigning;

    public static event Action<string, SpellSO> SpellReassigned; // actionName , SpellSo

    private void Awake()
    {
        spellReassignGui.SetActive(false);
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
        _canvasRenderer = GetComponent<Canvas>();
        _canvasRenderer.enabled = false;

        _listOfButtons = new List<SpellBuyButton>(gm.spellListSO.Size);

        foreach (SpellSO spellSo in gm.spellListSO)
        {
            SpellBuyButton spellBuyButton = Instantiate(spellBuyPrefab, parentPanel).GetComponent<SpellBuyButton>();
            spellBuyButton.manager = this;
            spellBuyButton.spellSo = spellSo;
            _listOfButtons.Add(spellBuyButton);
        }
    }

    // Part 2 of fixing using ESC to cancel spell assignment
    // Happens because for some reason when pressing a key it triggers multiple times
    // And single triggers dont work for some otherworldly reason
    private void Update()
    {
        if (_canvasRenderer.isActiveAndEnabled)
        {
            gm.isInMenu = true;
        }
    }

    public void StartReassignSpell(SpellSO spellSo)
    {
        isReassigning = true;
        spellReassignGui.SetActive(true);
        reassignedSpell = spellSo;
        buySpellsPanel.alpha = 0.5f;
        buySpellsPanel.interactable = false;
    }

    public void StopReassignSpell()
    {
        isReassigning = false;
        spellReassignGui.SetActive(false);
        reassignedSpell = null;
        buySpellsPanel.alpha = 1f;
        buySpellsPanel.interactable = true;
    }

    public static void InvokeOnSpellReassigned(string actionName, SpellSO spellSo)
    {
        SpellReassigned?.Invoke(actionName, spellSo);
    }

    public void Toggle()
    {
        Debug.Log("Toggling");
        _canvasRenderer.enabled = !_canvasRenderer.enabled;
        gm.isInMenu = !gm.isInMenu;
        if (!_canvasRenderer.enabled) return;
        foreach (SpellBuyButton buyButton in _listOfButtons)
        {
            buyButton.UpdateState();
            buyButton.UpdateGraphics();
        }
    }
    
}

