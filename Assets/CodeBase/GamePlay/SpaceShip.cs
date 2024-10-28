using UnityEngine;
using Common;

namespace SpaceShooter
{
    /// <summary>
    /// Класс космического корабля.
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class SpaceShip : Destructible
    {
        #region Properties

        [SerializeField] private Sprite m_ShipSprite;

        /// <summary>
        /// Масса для автоматической установки у ригида.
        /// </summary>
        [Header("SpaceShip")]
        [SerializeField] private float m_Mass;

        /// <summary>
        /// Толкающая вперёд сила.
        /// </summary>
        [SerializeField] private float m_Thrust;
        private float m_DefaultThrust;

        /// <summary>
        /// Вращающая сила.
        /// </summary>
        [SerializeField] private float m_Mobility;

        /// <summary>
        /// Максимальная линейная скорость.
        /// </summary>
        [SerializeField] private float m_MaxLinearVelocity;

        /// <summary>
        /// Максимальная вращательная скорость. В градусах/сек.
        /// </summary>
        [SerializeField] private float m_MaxAngularVelocity;

        /// <summary>
        /// Сохранённая ссылка на ригид.
        /// </summary>
        private Rigidbody2D m_Rigid;

        public float MaxLinearVelocity => m_MaxLinearVelocity;
        public float MaxAngularVelocity => m_MaxAngularVelocity;
        public Sprite ShipSprite => m_ShipSprite;

        private float m_Timer;

        #endregion

        #region Unity Events

        protected override void Start()
        {
            base.Start();

            m_DefaultThrust = m_Thrust;

            m_Rigid = GetComponent<Rigidbody2D>();
            m_Rigid.mass = m_Mass;

            m_Rigid.inertia = 1;

            InitOffensive();
        }

        private void FixedUpdate()
        {
            UpdateRigidBody();

            UpdateEnergyRegen();

            if (m_Indestructible)
            {
                m_Shield.SetActive(true);
                if (m_Timer < 5.0f)
                    m_Timer += Time.deltaTime;
                else
                    m_Indestructible = false;
            }
            else m_Shield.SetActive(false);

            if (m_Thrust > m_DefaultThrust)
            {
                if (m_Timer < 5.0f)
                    m_Timer += Time.deltaTime;
                else
                    m_Thrust = m_DefaultThrust;
            }
        }

        #endregion

        #region Public API

        /// <summary>
        /// Управление линейной тягой. -1.0 - 1.0.
        /// </summary>
        public float ThrustControl { get; set; }

        /// <summary>
        /// Управление вращательной тягой. -1.0 - 1.0.
        /// </summary>
        public float TorqueControl { get; set; }

        #endregion

        /// <summary>
        /// Метод добавления сил кораблю для движения.
        /// </summary>
        private void UpdateRigidBody()
        {
            m_Rigid.AddForce(ThrustControl * m_Thrust * transform.up * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddForce(-m_Rigid.velocity * (m_Thrust / m_MaxLinearVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(TorqueControl * m_Mobility * Time.fixedDeltaTime, ForceMode2D.Force);

            m_Rigid.AddTorque(-m_Rigid.angularVelocity * (m_Mobility / m_MaxAngularVelocity) * Time.fixedDeltaTime, ForceMode2D.Force);
        }

        [SerializeField] private Turret[] m_Turrets;
        
        public void Fire(TurretMode mode)
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                if (m_Turrets[i].Mode == mode)
                {
                    m_Turrets[i].Fire();
                }
            }
        }

        [SerializeField] private int m_MaxEnergy;
        [SerializeField] private int m_MaxAmmo;
        [SerializeField] private int m_EnergyRegenPerSecond;

        private float m_CurrentEnergy;
        private int m_CurrentAmmo;

        public void AddEnergy(int energy)
        {
            m_CurrentEnergy = Mathf.Clamp(m_CurrentEnergy + energy, 0, m_MaxEnergy);
        }

        public void AddAmmo(int ammo)
        {
            m_CurrentAmmo = Mathf.Clamp(m_CurrentAmmo + ammo, 0, m_MaxAmmo);
        }

        private void InitOffensive()
        {
            m_CurrentEnergy = m_MaxEnergy;
            m_CurrentAmmo = m_MaxAmmo;
        }

        private void UpdateEnergyRegen()
        {
            if (m_CurrentEnergy < m_MaxEnergy)
                m_CurrentEnergy += (float)m_EnergyRegenPerSecond * Time.fixedDeltaTime;
        }

        public bool DrawEnergy(int count)
        {
            if (count == 0) return true;

            if (m_CurrentEnergy >= count)
            {
                m_CurrentEnergy -= count;

                return true;
            }

            return false;
        }

        public bool DrawAmmo(int count)
        {
            if (count == 0) return true;

            if (m_CurrentAmmo >= count)
            {
                m_CurrentAmmo -= count;

                return true;
            }

            return false;
        }

        public void AssignWeapon(TurretProperties properties)
        {
            for (int i = 0; i < m_Turrets.Length; i++)
            {
                m_Turrets[i].AssignLoadOut(properties);
            }
        }

        public void SwitchIndestructible()
        {
            m_Indestructible = true;
            m_Timer = 0;
        }

        [SerializeField] private GameObject m_Shield;

        public void AddSpeedBuff(int buff)
        {
            m_Thrust *= buff;
            m_Timer = 0;
            
        }
    }
}
