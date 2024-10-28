using UnityEngine;
using UnityEngine.UI;


namespace SpaceShooter
{
    public class HitPointsBar : MonoBehaviour
    {
        [SerializeField] private Image m_Image;

        private float m_LastHitPoint;

        private void Update()
        {
            float hitPoints = (float) Player.Instance.ActiveShip.HitPoints / (float) Player.Instance.ActiveShip.StartHitPoints;

            if (hitPoints != m_LastHitPoint)
            {
                m_Image.fillAmount = hitPoints;
                m_LastHitPoint = hitPoints;
            }
        }
    }
}