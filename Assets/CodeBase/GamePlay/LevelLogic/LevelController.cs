using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace SpaceShooter
{
    public class LevelController : SingletonBase<LevelController>
    {
        private const string MainMenuSceneName = "MainMenu";

        public event UnityAction LevelPassed;
        public event UnityAction LevelFailed;

        [SerializeField] private LevelCondition[] m_Conditions;

        private bool IsLevelCompleted;

        private float m_LevelTime;
        public float LevelTime => m_LevelTime;

        private LevelSequencesController m_LevelSequencesController;
        private LevelProperties m_CurrentLevelProperties;

        private void Start()
        {
            Time.timeScale = 1;
            m_LevelTime = 0.0f;

            m_LevelSequencesController = LevelSequencesController.Instance;
            m_CurrentLevelProperties = m_LevelSequencesController.GetCurrentLoadedLevel();
        }

        private void Update()
        {
            if (!IsLevelCompleted)
            {
                m_LevelTime += Time.deltaTime;
                CheckLevelConditions();
            }
            

            if (Player.Instance.NumLives <= 0)
            {
                Fail();
            }
        }

        private void CheckLevelConditions()
        {
            int numCompleted = 0;

            for (int i = 0; i < m_Conditions.Length; i++)
            {
                if (m_Conditions[i].IsCompleted) numCompleted++;
            }

            if (numCompleted == m_Conditions.Length)
            {
                IsLevelCompleted = true;

                Pass();
            }
        }

        private void Pass()
        {
            LevelPassed?.Invoke();
            Time.timeScale = 0;
        }

        private void Fail()
        {
            LevelFailed?.Invoke();
            Time.timeScale = 0;
        }

        public void LoadNextLevel()
        {
            if (m_LevelSequencesController.CurrentLevelIsLast() == false)
            {
                string nextLevelSceneName = m_LevelSequencesController.GetNextLevelProperties(m_CurrentLevelProperties).SceneName;
                SceneManager.LoadScene(nextLevelSceneName);
            }    
            else SceneManager.LoadScene(MainMenuSceneName);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(m_CurrentLevelProperties.SceneName);
        }
    }
}