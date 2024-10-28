using UnityEngine;

namespace SpaceShooter
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject m_MainPanel;
        [SerializeField] private GameObject m_ShipSelectionPanel;
        [SerializeField] private GameObject m_LevelSelectionPanel;

        private void Start()
        {
            ShowMainPanel();
        }

        public void ShowMainPanel()
        {
            m_MainPanel.SetActive(true);
            m_LevelSelectionPanel.SetActive(false);
            m_ShipSelectionPanel.SetActive(false);
        }

        public void ShowLevelSelectionPanel()
        {
            m_LevelSelectionPanel.SetActive(true);
            m_MainPanel.SetActive(false);
        }

        public void ShowShipSelectionPanel()
        {
            m_ShipSelectionPanel.SetActive(true);
            m_MainPanel.SetActive(false);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}