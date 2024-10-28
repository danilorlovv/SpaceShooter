using UnityEngine;
using UnityEngine.UI;


namespace SpaceShooter
{
    public class ScoreText : MonoBehaviour
    {
        [SerializeField] private Text m_Text;

        private float m_LastScore;

        private void Update()
        {
            int score = Player.Instance.Score;

            if (score != m_LastScore)
            {
                m_Text.text = ("Score: " + score.ToString());
                m_LastScore = score;
            }
        }
    }
}