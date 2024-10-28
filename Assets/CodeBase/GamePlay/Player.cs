using UnityEngine;

namespace SpaceShooter
{
    public class Player : SingletonBase<Player>
    {
        public static SpaceShip SelectedSpaceShip;

        [SerializeField] private int m_NumLives;
        public int NumLives => m_NumLives;

        
        [SerializeField] private SpaceShip m_ShipPrefab;

        private FollowCamera m_FollowCamera;
        private ShipInputController m_ShipInputController;
        private Transform m_SpawnPoint;

        private SpaceShip m_Ship;

        public void Construct(FollowCamera followCamera, ShipInputController shipInputController, Transform spawnPosition)
        {
            m_FollowCamera = followCamera;
            m_ShipInputController = shipInputController;
            m_SpawnPoint = spawnPosition;
        }

        public FollowCamera FollowCamera => m_FollowCamera;

        public SpaceShip ActiveShip => m_Ship;

        public int m_Score;
        public int Score => m_Score;

        public int m_NumKills;
        public int NumKills => m_NumKills;

        public SpaceShip ShipPrefab
        {
            get
            {
                if (SelectedSpaceShip == null) 
                    return m_ShipPrefab;
                else
                    return SelectedSpaceShip;
            }
        }

        private void Start()
        {
            Respawn();
        }

        private void OnShipDeath()
        {
            m_NumLives--;

            if (m_NumLives > 0)
                Respawn();
        }

        private void Respawn()
        {
            var newPlayerShip = Instantiate(ShipPrefab, m_SpawnPoint.position, m_SpawnPoint.rotation);

            m_Ship = newPlayerShip.GetComponent<SpaceShip>();
            m_Ship.EventOnDeath.AddListener(OnShipDeath);

            m_FollowCamera.SetTarget(m_Ship.transform);
            m_ShipInputController.SetTarget(m_Ship);
        }

        public void AddScore(int num)
        {
            m_Score += num;
        }

        public void AddKill()
        {
            m_NumKills++;
        }
    }
}