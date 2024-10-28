using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class LivesText : MonoBehaviour
    {
        [SerializeField] private Text m_Text;
        [SerializeField] private Image m_Icon;

        private float m_LastLives;

        private void Start()
        {
            m_Icon.sprite = Player.Instance.ActiveShip.ShipSprite;
        }

        private void Update()
        {
            int lives = Player.Instance.NumLives;

            if (lives != m_LastLives)
            {
                m_Text.text = lives.ToString();
                m_LastLives = lives;
            }
        }
    }
}