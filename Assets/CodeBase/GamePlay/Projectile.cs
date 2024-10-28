using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class Projectile : ProjectileBase
    {
        [SerializeField] private GameObject m_ImpactEffectPrefab;

        private GameObject m_ImpactEffect;

        protected override void OnHit(Destructible dest)
        {
            if (dest.HitPoints <= 0)
            {
                if (m_Parent == Player.Instance.ActiveShip)
                {
                    Player.Instance.AddScore(dest.ScoreValue);

                    if (dest is SpaceShip)
                    {
                        Player.Instance.AddKill();
                    }
                }
            }
        }

        protected override void OnProjectileLifeEnd(Collider2D col, Vector2 pos)
        {
            if (m_ImpactEffectPrefab != null)
            {
                m_ImpactEffect = Instantiate(m_ImpactEffectPrefab, pos, Quaternion.identity);
                Destroy(m_ImpactEffect, 1.0f);
            }

            Destroy(gameObject, 0);
        }
    }
}