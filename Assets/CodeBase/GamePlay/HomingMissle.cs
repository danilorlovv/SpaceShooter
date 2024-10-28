using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class HomingMissle : Projectile
    {
        private GameObject[] m_Enemies;

        private GameObject m_TargetEnemy;

        private float m_MinDist = 100;


        private void Start()
        {
            m_Enemies = GameObject.FindGameObjectsWithTag("Enemy");

            float dist;

            for (int i = 0; i < m_Enemies.Length; i++)
            {
                dist = (m_Enemies[i].transform.position - transform.position).magnitude;

                if (m_MinDist > dist) 
                {
                    m_MinDist = dist;
                    m_TargetEnemy = m_Enemies[i];
                }
            }
        }

        protected override void Update()
        {
            if (m_TargetEnemy == null)
            {
                base.Update();
                return;
            }

            Vector2 direction = (m_TargetEnemy.transform.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, -angle), 10.0f * Time.deltaTime);

            base.Update();
        }
    }
}