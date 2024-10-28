using UnityEngine;
using UnityEngine.UI;


namespace SpaceShooter
{
    public class KillText : MonoBehaviour
    {
        [SerializeField] private Text m_Text;

        private float m_LastNumKills;

        private void Update()
        {
            int numKills = Player.Instance.NumKills;

            if (numKills != m_LastNumKills)
            {
                m_Text.text = ("Kills: " + numKills.ToString());
                m_LastNumKills = numKills;
            }
        }
    }
}