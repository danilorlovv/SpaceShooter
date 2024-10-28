using UnityEngine;
using Common;

namespace SpaceShooter
{
    public class AsteroidMultiple : Destructible
    {
        private enum AsteroidSize
        {
            Small,
            Default
        }

        [SerializeField] private GameObject m_AsteroidPrefab;

        [SerializeField] private AsteroidSize m_Size;

        protected override void OnDeath()
        {
            if (m_Size == AsteroidSize.Default)
            {
                for (int i = 0; i < 2; i++)
                {
                    GameObject miniAsteroid = Instantiate(m_AsteroidPrefab);
                    miniAsteroid.transform.position = transform.position + new Vector3(i, i, i);
                    miniAsteroid.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                    miniAsteroid.GetComponent<AsteroidMultiple>().m_Size = AsteroidSize.Small;
                }

            }
            base.OnDeath();
        }
    }
}