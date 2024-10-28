using UnityEngine;
using UnityEngine.UI;

namespace SpaceShooter
{
    public class ResultPanel : MonoBehaviour
    {
        private const string PassedText = "Passed";
        private const string FailedText = "Failed";
        private const string NextText = "Next";
        private const string RestartText = "Restart";
        private const string MenuText = "Menu";
        private const string PrefixKillsText = "Kills: ";
        private const string PrefixScoreText = "Score: ";
        private const string PrefixTimeText = "Time: ";


        [SerializeField] private Text m_Kills;
        [SerializeField] private Text m_Score;
        [SerializeField] private Text m_Time;
        [SerializeField] private Text m_Result;
        [SerializeField] private Text m_ButtonNextText;

        private bool m_LevelPassed = false;


        private void Start()
        {
            gameObject.SetActive(false);

            LevelController.Instance.LevelPassed += OnLevelPassed;

            LevelController.Instance.LevelFailed += OnLevelFail;
        }

        private void OnDestroy()
        {
            LevelController.Instance.LevelPassed -= OnLevelPassed;

            LevelController.Instance.LevelFailed -= OnLevelFail;
        }

        private void OnLevelPassed()
        {
            m_LevelPassed = true;

            FillLevelStatistics();
            m_Result.text = PassedText;

            if (LevelSequencesController.Instance.CurrentLevelIsLast()) 
                m_ButtonNextText.text = MenuText;
            else
                m_ButtonNextText.text = NextText;

            gameObject.SetActive(true);
        }

        private void OnLevelFail()
        {
            m_LevelPassed = false;

            FillLevelStatistics();
            m_Result.text = FailedText;
            m_ButtonNextText.text = RestartText;

            gameObject.SetActive(true);
        }

        private void FillLevelStatistics()
        {
            m_Kills.text = PrefixKillsText + Player.Instance.NumKills.ToString();
            m_Score.text = PrefixScoreText + Player.Instance.Score.ToString();
            m_Time.text = PrefixTimeText + LevelController.Instance.LevelTime.ToString("F0");
            //m_Kills.text = "Kills: " + Player.Instance.NumKills.ToString();
            //m_Kills.text = "Kills: " + Player.Instance.NumKills.ToString();
        }

        public void OnButtonNextAction()
        {
            gameObject.SetActive(false);

            if (m_LevelPassed)
                LevelController.Instance.LoadNextLevel();
            else
                LevelController.Instance.RestartLevel();
        }
    }
}