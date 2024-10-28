using UnityEngine;

namespace Common
{
    public abstract class ProjectileBase : Entity
    {
        [SerializeField] protected float m_Velocity;
        [SerializeField] private float m_LifeTime;
        [SerializeField] private int m_Damage;

        protected virtual void OnHit(Destructible dest) { }
        protected virtual void OnHit(Collider2D dest) { }
        protected virtual void OnProjectileLifeEnd(Collider2D col, Vector2 pos) { }

        protected virtual void Update()
        {
            float stepLenght = Time.deltaTime * m_Velocity;

            Vector2 step = transform.up * stepLenght;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLenght);

            if (hit)
            {
                OnHit(hit.collider);

                Destructible dest = hit.collider.transform.root.GetComponent<Destructible>();

                if (dest != null && dest != m_Parent)
                {
                    dest.ApplyDamage(m_Damage);

                    OnHit(dest);
                    OnProjectileLifeEnd(hit.collider, hit.point);
                } 
            }

            Destroy(gameObject, m_LifeTime);

            transform.position += new Vector3(step.x, step.y, 0);
        }

        protected Destructible m_Parent;

        public void SetParentShooter(Destructible parent)
        {
            m_Parent = parent;
        }
    }
}