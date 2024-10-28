using UnityEngine;

namespace SpaceShooter
{
    public class ExplosionController : MonoBehaviour
    {
        [SerializeField] private GameObject m_ExplosionPrefab;
        [SerializeField] private SpaceShip m_ShipExplode;
        private GameObject explosion;

        private void Start()
        {
            m_ShipExplode?.EventOnDeath.AddListener(OnExplode);
        }

        private void OnExplode()
        {
            explosion = Instantiate(m_ExplosionPrefab, transform.position, transform.rotation);
            Destroy(explosion, 1.0f);
        }
    }
}