using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SpellReassignButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private String ActionName;

    [SerializeField] private Image icon;

    [SerializeField] private SpellShopManager manager;
    private SpellSO _spellSo;

    private void OnEnable()
    {
        _spellSo = manager.gm.spellBindingsSo[ActionName];
        Sprite sprite = _spellSo.icon;
        if (sprite == null)
        {
            icon.gameObject.SetActive(false);
            return;
        }
        icon.sprite = _spellSo.icon;
        icon.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        manager.Tooltip.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_spellSo.spellTypeId == -1) {return;}
        manager.Tooltip.PassData(_spellSo.spellName, _spellSo.spellDescription, $"Cost: {_spellSo.spellCost}");
        manager.Tooltip.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        manager.Tooltip.SetActive(false);
    }

    public void OnClick()
    {
        Debug.Log($"Reassigning spell {manager.reassignedSpell.spellName} to action {ActionName}");
        manager.gm.spellBindingsSo[ActionName] = manager.reassignedSpell;
        SpellShopManager.InvokeOnSpellReassigned(ActionName, manager.reassignedSpell);
        manager.StopReassignSpell();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick();
    }
}
