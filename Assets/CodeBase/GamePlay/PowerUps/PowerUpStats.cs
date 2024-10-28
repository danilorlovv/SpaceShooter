using UnityEngine;

namespace SpaceShooter
{
    public class PowerUpStats : PowerUp
    {
        public enum EffectType
        {
            AddEnergy,
            AddAmmo,
            AddImortability,
            AddVelocityBuff
        }

        [SerializeField] private EffectType m_EffectType;
        [SerializeField] private int m_Value;

        protected override void OnPickedUp(SpaceShip ship)
        {
            if (m_EffectType == EffectType.AddEnergy)
                ship.AddEnergy(m_Value);

            if (m_EffectType == EffectType.AddAmmo)
                ship.AddAmmo(m_Value);

            if (m_EffectType == EffectType.AddImortability)
            {
                ship.SwitchIndestructible();
            }

            if (m_EffectType == EffectType.AddVelocityBuff)
            {
                ship.AddSpeedBuff(m_Value);
            }

        }
    }
}