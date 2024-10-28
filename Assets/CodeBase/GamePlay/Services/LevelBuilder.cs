using Common;
using UnityEngine;

namespace SpaceShooter
{
    public class LevelBuilder : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject m_LevelGUIPrefab;
        [SerializeField] private GameObject m_PlayerHUDPrefab;
        [SerializeField] private GameObject m_BackgroundPrefab;

        [Header("Dependencies")]
        [SerializeField] private PlayerSpawner m_PlayerSpawner;
        [SerializeField] private LevelBoundary m_LevelBoundary;
        [SerializeField] private LevelController m_LevelController;

        private void Awake()
        {
            m_LevelBoundary.Init();
            m_LevelController.Init();

            Player player = m_PlayerSpawner.Spawn();

            player.Init();

            Instantiate(m_LevelGUIPrefab);
            Instantiate(m_PlayerHUDPrefab);

            GameObject background = Instantiate(m_BackgroundPrefab);
            background.AddComponent<SyncTransform>().SetTarget(player.FollowCamera.transform);
        }
    }
}