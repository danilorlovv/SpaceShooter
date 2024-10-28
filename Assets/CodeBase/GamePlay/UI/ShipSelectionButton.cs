using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class ShipSelectionButton : MonoBehaviour
    {
        [SerializeField] private MainMenu m_MainMenu;
        [SerializeField] private SpaceShip m_SpaceShipPrefab;

        [SerializeField] private Text m_ShipNameText;
        [SerializeField] private Text m_HitPointsText;
        [SerializeField] private Text m_SpeedText;
        [SerializeField] private Text m_AgilityText;
        [SerializeField] private Image m_PreviewImage;

        private void Start()
        {
            if (m_SpaceShipPrefab == null) return;

            m_ShipNameText.text = m_SpaceShipPrefab.Nickname;
            m_HitPointsText.text = "HP: " + m_SpaceShipPrefab.StartHitPoints.ToString();
            m_SpeedText.text = "Speed: " + m_SpaceShipPrefab.MaxLinearVelocity.ToString("F0");
            m_AgilityText.text = "Agility: " + m_SpaceShipPrefab.MaxAngularVelocity.ToString("F0");
            m_PreviewImage.sprite = m_SpaceShipPrefab.ShipSprite;
        }

        public void SelectShip()
        {
            Player.SelectedSpaceShip = m_SpaceShipPrefab;
            m_MainMenu.ShowMainPanel();
        }
    }
}