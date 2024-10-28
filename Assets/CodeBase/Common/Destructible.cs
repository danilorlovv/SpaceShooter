using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    /// <summary>
    /// ������������ ������ �� �����. ��, ��� ����� ����� ���������.
    /// </summary>
    public class Destructible : Entity
    {
        #region Properties

        /// <summary>
        /// ������ ���������� �����������. 
        /// </summary>
        [SerializeField] protected bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        /// <summary>
        /// ��������� ���-�� ����������.
        /// </summary>
        [SerializeField] private int m_HitPoints;
        public int StartHitPoints => m_HitPoints;

        /// <summary>
        /// ������� ���-�� ����������.
        /// </summary>
        private int m_CurrentHitPoints;
        public int HitPoints => m_CurrentHitPoints;

        #endregion

        #region UnityEvents

        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoints;

            transform.SetParent(null);
        }

        #endregion

        #region Public API

        /// <summary>
        /// ���������� ����� � �������.
        /// </summary>
        /// <param name="damage">����, ��������� �������</param>
        public void ApplyDamage(int damage)
        {
            if (m_Indestructible) return;

            else m_CurrentHitPoints -= damage;
            if (m_CurrentHitPoints <= 0) OnDeath();
        }

        #endregion


        /// <summary>
        /// ���������������� ������� ����������� �������.
        /// </summary>
        protected virtual void OnDeath()
        {
            Destroy(gameObject);

            m_EventOnDeath?.Invoke();
        }

        private static HashSet<Destructible> m_AllDestructibles;

        public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null)
                m_AllDestructibles = new HashSet<Destructible>();

            m_AllDestructibles.Add(this);
        }

        protected virtual void OnDestroy()
        {
            m_AllDestructibles?.Remove(this);
        }

        public const int TeamIdNeutral = 0;

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;


        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;
    }
}