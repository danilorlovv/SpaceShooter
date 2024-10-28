using UnityEngine;

namespace Common
{
    public class ImpactEffect : MonoBehaviour
    {
        [SerializeField] private float m_LifeTime;

        private void Start()
        {
            Destroy(gameObject, m_LifeTime);
        }
    }
}