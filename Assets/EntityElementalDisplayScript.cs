using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Spells;
using UnityEngine;

public class EntityElementalDisplayScript : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer fireEffect;
    [SerializeField]
    private SpriteRenderer iceEffect;
    [SerializeField]
    private SpriteRenderer statikEffect;

    [SerializeField] private EnemyInterface _enemyInterface;
    private Dictionary<StatusType, SpriteRenderer> statusTypeSpriteRenderers = new();

    private void Awake()
    {
        statusTypeSpriteRenderers[StatusType.Electric] = statikEffect;
        statusTypeSpriteRenderers[StatusType.Fire] = fireEffect;
        statusTypeSpriteRenderers[StatusType.Ice] = iceEffect;

        foreach (KeyValuePair<StatusType,SpriteRenderer> keyValuePair in statusTypeSpriteRenderers)
        {
            statusTypeSpriteRenderers[keyValuePair.Key].enabled = false;
        }
    }

    private void FixedUpdate()
    {
        foreach (KeyValuePair<StatusType,StatusEffect> keyValuePair in _enemyInterface.StatusEffects)
        {
            if (keyValuePair.Key != StatusType.Null)
            {
                statusTypeSpriteRenderers[keyValuePair.Key].enabled = keyValuePair.Value.isActive();
            }
        }
    }
}
