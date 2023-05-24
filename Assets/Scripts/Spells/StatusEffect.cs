using System;
using UnityEngine;

namespace Spells
{
    public enum StatusType
    {
        Null,
        Fire,
        Ice,
        Poison,
        Electric,
    }
    
    [Serializable]
    public class StatusEffect
    {
        public StatusType StatusType;
        public float Dps;
        public float Duration;

        public StatusEffect(StatusType statusType, float dps, float duration)
        {
            StatusType = statusType;
            Dps = dps;
            Duration = duration;
        }

        public float Tick() // Returns amount of health to remove, needs to be called in a fixed update
        {
            if (Duration == 0)
            {
                return 0;
            }
            float time = Mathf.Min(Time.fixedDeltaTime, Duration); // So that you dont take more dmg than needed

            Duration -= time;
            return Dps * time;
        }
//bebou <3
        public StatusEffect Copy()
        {
            return new StatusEffect(StatusType, Dps, Duration);
        }

        public static StatusEffect BestOf(StatusEffect effect1, StatusEffect effect2)
        {
            if (effect1.StatusType != effect2.StatusType)
            {
                throw new InvalidOperationException();
            }
            
            float potential1 = effect1.Dps>0 ? effect1.Duration * effect1.Dps: effect1.Duration;
            float potential2 = effect2.Dps>0 ? effect2.Duration * effect2.Dps: effect2.Duration;

            return potential1 >= potential2 ? effect1 : effect2;
            
        }

    }
}