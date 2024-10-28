using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class CollisionDamageApplicator : MonoBehaviour
    {
        public static string IgnoreTag = "WorldBoundary";

        [SerializeField] private int m_VelocityDamageModifier;
        [SerializeField] private int m_DamageConstant;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.tag == IgnoreTag) return;


            var destructible = transform.root.GetComponent<Destructible>();

            if (destructible != null)
            {
                destructible.ApplyDamage(m_DamageConstant + 
                                        (int)(m_VelocityDamageModifier * collision.relativeVelocity.magnitude));
            }
        }
    }
}