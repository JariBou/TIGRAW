using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

public class SpellIconDisplay : MonoBehaviour
{
    [SerializeField]
    private String ActionName;

    [SerializeField] private Image icon;

    private GameManager gm;
    
    private void Awake()
    {
        gm = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();
    }

    public void Update()
    {
        Sprite sprite = gm.spellBindingsSo[ActionName].icon;
        if (sprite == null)
        {
            icon.gameObject.SetActive(false);
            return;
        }
        icon.sprite = gm.spellBindingsSo[ActionName].icon;
        icon.gameObject.SetActive(true);
    }
    
    private void Update(string actionName, SpellSO spellSo)
    {
        if (actionName != ActionName)
        {
            return;
            
        }
        
        Sprite sprite = spellSo.icon;
        if (sprite == null) // Just in case
        {
            icon.gameObject.SetActive(false);
            return;
        }
        icon.sprite = spellSo.icon;
        icon.gameObject.SetActive(true);
    }

    private void OnEnable()
    {
        SpellShopManager.SpellReassigned += Update;

        Update();
    }

    private void OnDisable()
    {
        SpellShopManager.SpellReassigned -= Update;
    }
}
