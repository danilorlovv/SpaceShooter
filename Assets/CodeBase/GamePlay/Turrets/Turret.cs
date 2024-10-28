using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class Turret : MonoBehaviour
    {
        #region Properties

        [SerializeField] private TurretMode m_Mode;
        public TurretMode Mode => m_Mode;

        [SerializeField] private TurretProperties m_TurretProperties;

        private float m_RefireTimer;

        public bool m_CanFire => m_RefireTimer <= 0;

        private SpaceShip m_Ship;

        #endregion

        #region UnityEvents

        private void Start()
        {
            m_Ship = transform.GetComponentInParent<SpaceShip>();
        }

        private void Update()
        {
            if (!m_CanFire)
                m_RefireTimer -= Time.deltaTime;
        }

        #endregion

        #region PublicAPI

        public void Fire()
        {
            if (m_TurretProperties == null) return;

            if (!m_CanFire) return;

            if (m_Ship.DrawEnergy(m_TurretProperties.EnergyUsage) == false) return;
            if (m_Ship.DrawAmmo(m_TurretProperties.AmmoUsage) == false) return;

            Projectile projectile = Instantiate(m_TurretProperties.ProjectilePrefab).GetComponent<Projectile>();
            projectile.transform.position = transform.position;
            projectile.transform.up = transform.up;
            projectile.SetParentShooter(m_Ship);

            m_RefireTimer = m_TurretProperties.RateOfFire;

            //SFX
        }

        // Bonuses+
        public void AssignLoadOut(TurretProperties properties)
        {
            if (m_Mode != properties.Mode) return;

            m_RefireTimer = 0;

            m_TurretProperties = properties;
        }

        #endregion
    }
}