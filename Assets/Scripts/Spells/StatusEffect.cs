using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Spells
{
    public enum StatusType
    {
        Null,
        Fire,
        Ice,
        Poison,
        Electric,
        Slow,
        DmgTakenIncrease,
        HeatReductionRateDecrease,
    }
    
    [Serializable]
    public class StatusEffect
    {
        public StatusType StatusType;
        [FormerlySerializedAs("Dps")] public float value;
        public float Duration;
        // For Overdrive Maluses if duration is 1 the its active else its not
        public StatusEffect(StatusType statusType, float Value, float duration)
        {
            StatusType = statusType;
            value = Value;
            Duration = duration;
        }

        public StatusEffect Copy()
        {
            return new StatusEffect(StatusType, value, Duration);
        }

        public bool IsActive()
        {
            return Duration > 0;
        }

        public static StatusEffect BestOf(StatusEffect effect1, StatusEffect effect2)
        {
            if (effect1.StatusType != effect2.StatusType)
            {
                throw new InvalidOperationException();
            }
            
            float potential1 = effect1.value>0 ? effect1.Duration * effect1.value: effect1.Duration;
            float potential2 = effect2.value>0 ? effect2.Duration * effect2.value: effect2.Duration;

            return potential1 >= potential2 ? effect1 : effect2;
            
        }

        public void Tick(float fixedDeltaTime)
        {
            Duration -= fixedDeltaTime;
        }
    }
}