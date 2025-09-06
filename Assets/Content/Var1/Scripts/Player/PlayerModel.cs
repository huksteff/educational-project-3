using System;

namespace Content.Scripts.Player
{
    public class PlayerModel
    {
        public float FrozenStrength;
        public event Action OnFrozen;
        public bool IsInColdZone;
        
        public void IncreaseFrozenStrength(float value)
        {
            FrozenStrength += value;
            if (FrozenStrength >= 100)
            {
                FrozenStrength = 100;
                OnFrozen?.Invoke();
            }
        }

        public void DecreaseFrozenStrength(float value)
        {
            FrozenStrength -= value;
            if (FrozenStrength < 0)
            {
                FrozenStrength = 0;
            }
                       
        }
    }
}